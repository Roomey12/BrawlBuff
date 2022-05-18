using BrawlBuff.Application.Common.Comparers;
using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using BrawlBuff.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using PlayerBattle = BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models.PlayerBattle;
using EventBattle = BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models.EventBattle;
using BattleLog = BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models.BattleLog;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;

namespace BrawlBuff.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly BrawlStarsApiHttpService _brawlStarsApiHttpService;
        private readonly BrawlApiHttpService _brawlApiHttpService;
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        // cover everything with cancellation tokens
        public PlayerService(BrawlStarsApiHttpService brawlStarsApiHttpService,
            BrawlApiHttpService brawlApiHttpService,
            IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlStarsApiHttpService = brawlStarsApiHttpService;
            _brawlApiHttpService = brawlApiHttpService;
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<IEnumerable<BattleDetail>> GetPlayerBattleStatsAsync(string tag)
        {
            var player = await _brawlStarsApiHttpService.GetPlayerByTagAsync(tag);

            if (player == null)
            {
                throw new Exception("There are no player with such tag.");
            }

            var dbPlayer = await _brawlBuffDbContext.Players.FirstOrDefaultAsync(x => x.Tag == player.Tag);

            if (dbPlayer == null)
            {
                var newPlayer = new Player(player.Tag);
                await RecordPlayerBattleStatsAsync(newPlayer);
            }

            var battles = await _brawlBuffDbContext.BattleDetails.Include(x => x.Battle).Where(x => x.PlayerTag == player.Tag).ToListAsync();

            return battles;
        }

        public async Task RecordPlayerBattleStatsAsync(Player player)
        {
            if(player == null)
            {
                throw new Exception("todo");
            }

            using var transaction = await _brawlBuffDbContext.Database.BeginTransactionAsync();
            var battleDetails = new List<BattleDetail>();
            try
            {
                player.StatsUpdatedOn = DateTime.Now;
                if(player.Id == 0)
                {
                    await _brawlBuffDbContext.Players.AddAsync(player);
                    await _brawlBuffDbContext.SaveChangesAsync();
                }

                var battleLogs = await _brawlStarsApiHttpService.GetRecentBattlesByPlayersTagAsync(player.Tag, true);

                foreach (var log in battleLogs)
                {
                    var battleTime = DateTime.ParseExact(log.BattleTime, "yyyyMMddTHHmmss'.'fffZ", CultureInfo.InvariantCulture);
                    var dbEventId = (await _brawlBuffDbContext.Events.FirstOrDefaultAsync(x => x.BrawlEventId == log.Event.Id))?.Id;
                    var battle = await _brawlBuffDbContext.Battles.FirstOrDefaultAsync(x => x.BattleTime == battleTime && x.EventId == dbEventId);

                    if (battle != null)
                    {
                        var existingBattleDetail = await _brawlBuffDbContext.BattleDetails.FirstOrDefaultAsync(x => x.PlayerTag == player.Tag && x.BattleId == battle.Id);
                        
                        if (existingBattleDetail != null)
                        {
                            if (existingBattleDetail.PlayerId == player.Id)
                            {
                                break;
                            }

                            if (existingBattleDetail.PlayerId == null)
                            {
                                ExpandExistingBattleDetail(log, existingBattleDetail, player);
                                continue;
                            }
                        }
                    }

                    var battleDetail = new BattleDetail()
                    {
                        PlayerId = player.Id,
                        PlayerTag = player.Tag,
                        TrophyChange = log.Battle.TrophyChange,
                        Result = log.Battle.Result,
                        Battle = new Battle()
                        {
                            EventId = await GetDbEventIdAsync(log.Event.Id),
                            StarPlayerTag = log.Battle.StarPlayer?.Tag,
                            Type = log.Battle.Type,
                            BattleTime = battleTime,
                            Duration = log.Battle.Duration,
                        }
                    };

                    battleDetails.AddRange(GetBattleDetails(log, battleDetail, player));
                }

                await _brawlBuffDbContext.BattleDetails.AddRangeAsync(battleDetails);

                await _brawlBuffDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public BattleDetail ExpandExistingBattleDetail(BattleLog log, BattleDetail battleDetail, Player newPlayer)
        {
            battleDetail.PlayerId = newPlayer.Id;
            var eventType = Event.GetEventType(log.Event.Mode);
            battleDetail.TrophyChange = log.Battle.TrophyChange != null ? log.Battle.TrophyChange : 
                (eventType == EventType.Event1vs1 ? log.Battle.Players.FirstOrDefault(x => x.Tag == newPlayer.Tag).Brawlers.Sum(x => x.Trophies) : null);
            return battleDetail;
        }

        public List<BattleDetail> GetBattleDetails(BattleLog log, BattleDetail battleDetail, Player newPlayer)
        {
            var eventType = Event.GetEventType(log.Event.Mode ?? log.Battle.Mode);
            var battleDetails = new List<BattleDetail>();

            switch (eventType)
            {
                case EventType.Event3vs3 or EventType.Event5of2:
                    battleDetails.AddRange(HandleTeamGame(log, battleDetail, newPlayer, eventType));
                    break;
                case EventType.Event1vs1:
                    battleDetails.AddRange(Handle1vs1Game(log, battleDetail, newPlayer));
                    break;
                case EventType.EventSoloPlayers:
                    battleDetails.Add(HandleSoloPlayersGame(log, battleDetail, newPlayer));
                    break;
                //case EventType.Event5vs1: // skip
                //    break;
                //case EventType.EventSolo: // skip
                //    break;
                //case EventType.Unknown:
                //    break;
                //default:
                //    break;
            }

            return battleDetails;
        }

        public List<BattleDetail> Handle1vs1Game(BattleLog log, BattleDetail battleDetail, Player newPlayer)
        {
            var battleDetails = new List<BattleDetail>();

            foreach(var player in log.Battle.Players)
            {
                var isMainPlayer = player.Tag == newPlayer.Tag;
                var place = GetPlaceByResult(battleDetail.Result, !isMainPlayer);

                if (isMainPlayer)
                {
                    battleDetail.TrophyChange = player.Brawlers.Sum(x => x.TrophyChange);
                    battleDetail.Place = place;
                    battleDetail.Brawler = player.Brawlers.FirstOrDefault()?.Name;
                    battleDetails.Add(battleDetail);
                    continue;
                }

                var newBattleDetail = new BattleDetail()
                {
                    Battle = battleDetail.Battle,
                    PlayerTag = player.Tag,
                    Brawler = player.Brawlers.FirstOrDefault()?.Name,
                    TrophyChange = player.Brawlers.Sum(x => x.TrophyChange),
                    Place = place,
                    Result = GetResultByPlace(place, EventType.Event1vs1)
                };

                battleDetails.Add(newBattleDetail);
            }

            return battleDetails;
        }

        public BattleDetail HandleSoloPlayersGame(BattleLog log, BattleDetail battleDetail, Player newPlayer)
        {
            var gamePlayer = log.Battle.Players.FirstOrDefault(x => x.Tag == newPlayer.Tag);
            battleDetail.Place = log.Battle.Rank ?? log.Battle.Players.IndexOf(gamePlayer) + 1;
            battleDetail.Brawler = gamePlayer.Brawler.Name;
            var eventType = Event.GetEventType(log.Event.Mode ?? log.Battle.Mode);
            battleDetail.Result = GetResultByPlace(battleDetail.Place.Value, eventType);
            return battleDetail;
        }

        public List<BattleDetail> HandleTeamGame(BattleLog log, BattleDetail battleDetail, Player newPlayer, EventType eventType)
        {
            var battleDetails = new List<BattleDetail>();

            foreach (var team in log.Battle.Teams)
            {
                var newTeam = new Team();

                if (eventType == EventType.Event3vs3)
                {
                    var isMainPlayerInThisTeam = team.Any(x => x.Tag == newPlayer.Tag);
                    newTeam.Place = GetPlaceByResult(battleDetail.Result, !isMainPlayerInThisTeam);
                }
                else if (eventType == EventType.Event5of2)
                {
                    newTeam.Place = log.Battle.Teams.IndexOf(team) + 1;
                    //set result
                }

                foreach (var teamPlayer in team)
                {
                    if (teamPlayer.Tag == newPlayer.Tag)
                    {
                        battleDetail.Brawler = teamPlayer.Brawler.Name;
                        battleDetail.Team = newTeam;
                        battleDetail.Result = GetResultByPlace(newTeam.Place, eventType);
                        battleDetails.Add(battleDetail);
                        continue;
                    }

                    var newBattleDetail = new BattleDetail()
                    {
                        Team = newTeam,
                        PlayerTag = teamPlayer.Tag,
                        Brawler = teamPlayer.Brawler.Name,
                        Result = GetResultByPlace(newTeam.Place, eventType),
                        Battle = battleDetail.Battle
                    };

                    battleDetails.Add(newBattleDetail);
                }
            }

            return battleDetails;
        }

        private async Task<int?> GetDbEventIdAsync(int id)
        {
            var dbEvent = await _brawlBuffDbContext.Events.FirstOrDefaultAsync(e => e.BrawlEventId == id);

            // if id == 0, get Battle.mode instead

            if (id != 0 && dbEvent == null)
            {
                var dbEvents = await _brawlBuffDbContext.Events.ToListAsync();

                var apiMaps = await _brawlApiHttpService.GetMapsAsync();
                var apiEvents = apiMaps.Select(x =>
                {
                    var mode = x.GameMode.Name.Replace(" ", "");
                    return new Event()
                    {
                        BrawlEventId = x.Id,
                        Map = x.Name,
                        ImageUrl = x.ImageUrl,
                        Mode = string.Concat(mode[0].ToString().ToLower(), mode.AsSpan(1)),
                    };
                }).ToList();

                var newEvents = dbEvents.Except(apiEvents, new EventComparer());

                // check new Event Mode, if there are no events with such mode in the db - add new logic

                await _brawlBuffDbContext.Events.AddRangeAsync(newEvents);
                await _brawlBuffDbContext.SaveChangesAsync();

                dbEvent = newEvents.FirstOrDefault(x => x.BrawlEventId == id); 
            }

            return dbEvent?.Id;
        }

        // used only for battles with 2 teams
        private int GetPlaceByResult(string result, bool getOppositeResult = false)
        {
            var place = result == "victory" ? 1 : 2;

            if (getOppositeResult)
            {
                place = place == 1 ? 2 : 1;
            }

            return place;
        }

        private string GetResultByPlace(int place, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.EventSoloPlayers:
                    return place <= 4 ? "victory" : "defeat";
                case EventType.Event5of2:
                    return place <= 2 ? "victory" : "defeat";
                case EventType.Event3vs3 or EventType.Event1vs1:
                    return place == 1 ? "victory" : "defeat";
                default:
                    return null;
            }
        }
    }
}
