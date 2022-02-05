using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using Microsoft.AspNetCore.Mvc;

namespace BrawlBuffApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        private readonly BrawlStarsApiHttpService _brawlStarsApiHttpService;

        public PlayerController(
            IPlayerService playerService,
            BrawlStarsApiHttpService brawlStarsApiHttpService)
        {
            _brawlStarsApiHttpService = brawlStarsApiHttpService;
            _playerService = playerService;
        }

        [HttpGet("sas")]
        public async Task<IActionResult> GetSas()
        {
            var x = await _brawlStarsApiHttpService.GetRecentBattlesByPlayersTagAsync("%23Q2L9C0QLQ");
            return Ok(x);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetPlayerBattleStats()
        {
            var playerStats = await _playerService.GetPlayerBattleStatsAsync("%23Q2L9C0QLQ");
            return Ok(playerStats);
        }

        //[HttpGet("{tag}")]
        //public async Task<IActionResult> GetPlayerByTag(string tag)
        //{
        //    NormalizeTag(ref tag);

        //    var player = await _playerService.GetPlayerByTagAsync(tag);

        //    if (player == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(player);
        //}

        //[HttpGet("{tag}/battlelog")]
        //public async Task<IActionResult> GetRecentBattlesByPlayersTag(string tag)
        //{
        //    NormalizeTag(ref tag);

        //    var battleLog = await _playerService.GetRecentBattlesByPlayersTagAsync(tag);

        //    return Ok(battleLog);
        //}

        //[HttpGet("{tag}/fromdb")]
        //public async Task<IActionResult> GetPlayerFromDb(string tag)
        //{
        //    NormalizeTag(ref tag);

        //    var player = await _playerService.GetPlayerFromDbAsync(tag);

        //    if (player == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(player);
        //}

        //[HttpGet("{tag}/stats")]
        //public async Task<IActionResult> GetPlayerStats(string tag)
        //{
        //    NormalizeTag(ref tag);

        //    var battles = await _playerService.GetPlayerBattleStats(tag);

        //    return Ok(battles);
        //}


        //private void NormalizeTag(ref string tag)
        //{
        //    tag = "%23" + tag;
        //}
    }
}
