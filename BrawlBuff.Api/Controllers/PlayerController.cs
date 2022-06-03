using BrawlBuff.Api.Controllers;
using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using Microsoft.AspNetCore.Mvc;

namespace BrawlBuffApi.Controllers
{
    public class PlayerController : ApiControllerBase
    {
        private readonly IPlayerService _playerService;


        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }


        [HttpGet("{tag}")]
        public async Task<IActionResult> GetPlayerBattleStats(string tag)
        {
            NormalizeTag(ref tag);
            var playerStats = await _playerService.GetPlayerBattleStatsAsync(tag);
            return Ok(playerStats);
        }
 

        private void NormalizeTag(ref string tag)
        {
            tag = "%23" + tag;
        }
    }
}
