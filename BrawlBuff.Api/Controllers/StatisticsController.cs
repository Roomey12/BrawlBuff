using BrawlBuff.Application.Brawlers.Queries.GetBrawlersStats;
using BrawlBuff.Application.Players.Queries.GetPersonalStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BrawlBuff.Api.Controllers
{
    public class StatisticsController : ApiControllerBase
    {
        [HttpGet("stats")]
        public async Task<IActionResult> GetPlayerStats()
        {
            var result = await Mediator.Send(new GetPlayerStatsQuery { PlayerTag = "#Q2L9C0QLQ" });
            return Ok(result);
        }

        [HttpGet("brawlers")]
        public async Task<IActionResult> GetCharacters([FromQuery] string? tag)
        {
            var result = await Mediator.Send(new GetBrawlersStatsQuery { PlayerTag = tag });
            return Ok(result);
        }
    }
}
