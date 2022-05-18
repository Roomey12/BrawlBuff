using BrawlBuff.Application.Characters.Queries.GetPersonalStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BrawlBuff.Api.Controllers
{
    public class StatisticsController : ApiControllerBase
    {
        public StatisticsController()
        {

        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetPlayerStats()
        {
            var result = await Mediator.Send(new GetPersonalStatsQuery { PlayerTag = "#Q2L9C0QLQ" });
            return Ok(result);
        }

        [HttpGet("characters")]
        public async Task<IActionResult> GetCharacters()
        {
            return Ok();
        }
    }
}
