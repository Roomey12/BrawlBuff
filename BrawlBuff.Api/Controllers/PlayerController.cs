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


        [HttpPost("{tag}")]
        [HttpGet("{tag}")]
        public async Task<IActionResult> RegisterPlayer(string tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            await _playerService.RegisterPlayerAsync(tag);
            return Ok();
        }

        protected void NormalizeTag(ref string tag)
        {
            tag = "%23" + tag;
        }
    }
}
