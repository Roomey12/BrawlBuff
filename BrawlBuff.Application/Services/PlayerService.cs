using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using BrawlBuff.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using PlayerBattle = BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models.PlayerBattle;

namespace BrawlBuff.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly BrawlStarsApiHttpService _brawlStarsApiHttpService;
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        // cover everything with cancellation tokens
        public PlayerService(BrawlStarsApiHttpService brawlStarsApiHttpService, IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlStarsApiHttpService = brawlStarsApiHttpService;
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<IEnumerable<BattleDetail>> GetPlayerBattleStatsAsync(string tag)
        {
            var player = await _brawlStarsApiHttpService.GetPlayerByTagAsync(tag);

            if (player == null)
            {
                throw new Exception("There are no player with such tag.");
            }

            var dbPlayer = await _brawlBuffDbContext.Players.FirstOrDefaultAsync(x => x.Tag == tag);

            IEnumerable<BattleDetail> battles;
            if (dbPlayer == null)
            {
                battles = await StartRecordingPlayerStatistics(player.Tag);
            }
            else
            {
                battles = await _brawlBuffDbContext.BattleDetails.Include(x => x.Battle).Where(x => x.PlayerTag == player.Tag).ToListAsync();
            }

            return battles;
        }

        private async Task<IEnumerable<BattleDetail>> StartRecordingPlayerStatistics(string playerTag)
        {
            var newPlayer = new Player(playerTag);

            using var transaction = await _brawlBuffDbContext.Database.BeginTransactionAsync();
            var battleDetails = new List<BattleDetail>();
            try
            {
                await _brawlBuffDbContext.Players.AddAsync(newPlayer);
                await _brawlBuffDbContext.SaveChangesAsync();

                var battleLogs = await _brawlStarsApiHttpService.GetRecentBattlesByPlayersTagAsync(newPlayer.Tag, true);

                foreach (var log in battleLogs)
                {
                    // TODO: check if this battle already exists
                    var battleDetail = new BattleDetail()
                    {
                        PlayerId = newPlayer.Id,
                        PlayerTag = newPlayer.Tag,
                        TrophyChange = log.Battle.TrophyChange,
                        Result = log.Battle.Result,
                        Battle = new Battle()
                        {
                            // TODO: if event is null, then make a call to the events api to update db data
                            EventId = await GetDbEventId(log.Event.Id),
                            StarPlayerTag = log.Battle.StarPlayer?.Tag,
                            Type = log.Battle.Type,
                            BattleTime = DateTime.ParseExact(log.BattleTime, "yyyyMMddTHHmmss'.'fffZ", CultureInfo.InvariantCulture),
                            Duration = log.Battle.Duration,
                        }
                    };

                    if (log.Battle.Teams != null)
                    {
                        // rewrite this method?
                        var is3vs3Mode = Is3vs3Mode(log.Battle.Teams);

                        foreach (var team in log.Battle.Teams)
                        {
                            // fix team places
                            var newTeam = new Team();

                            if (is3vs3Mode)
                            {
                                var isMainPlayerInThisTeam = team.Any(x => x.Tag == newPlayer.Tag);
                                newTeam.Place = Get3vs3PlaceByResult(battleDetail.Result, !isMainPlayerInThisTeam);
                            }

                            foreach (var teamPlayer in team)
                            {
                                // if - 3vs3 - find mainPlayer teamMates and write same result
                                // then write opposite result to the enemy team

                                if(teamPlayer.Tag == newPlayer.Tag)
                                {
                                    battleDetail.Brawler = teamPlayer.Brawler.Name;
                                    battleDetail.Team = newTeam;
                                    continue;
                                }

                                // if teammate - set mainPlayerResult either !mainPlayerResult
                                var newBattleDetail = new BattleDetail()
                                {
                                    Team = newTeam,
                                    PlayerTag = teamPlayer.Tag,
                                    Brawler = teamPlayer.Brawler.Name,
                                    Result = newTeam.Place == 1 ? "victory" : "defeat",
                                    Battle = battleDetail.Battle
                                };

                                battleDetails.Add(newBattleDetail);
                            }
                        }
                    }
                    else
                    {
                        var gamePlayer = log.Battle.Players.FirstOrDefault(x => x.Tag == newPlayer.Tag);
                        battleDetail.Place = log.Battle.Rank ?? log.Battle.Players.IndexOf(gamePlayer) + 1;
                        battleDetail.Brawler = gamePlayer.Brawler.Name;
                    }

                    battleDetails.Add(battleDetail);
                }

                await _brawlBuffDbContext.BattleDetails.AddRangeAsync(battleDetails);

                var x = await _brawlBuffDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return battleDetails;
        }

        private async Task<int?> GetDbEventId(int id)
        {
            var dbEvent = await _brawlBuffDbContext.Events.FirstOrDefaultAsync(e => e.BrawlEventId == id);
            return dbEvent?.Id;
        }

        private bool ArePlayersInOneTeam()
        {
            return false;
        }

        private bool Is3vs3Mode(List<List<PlayerBattle>> teams)
        {
            return teams?.Count() == 2;
        }

        private int Get3vs3PlaceByResult(string result, bool getOppositeResult = false)
        {
            var place = result == "victory" ? 1 : 2;

            if (getOppositeResult)
            {
                place = place == 1 ? 2 : 1;
            }

            return place;
        }

        //private bool IsTeammate(string mainPlayerTag, string comparePlayerTag, List<PlayerBattle> team)
        //{

        //}
    }
}
