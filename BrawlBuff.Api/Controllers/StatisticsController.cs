using BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsModesStats;
using BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsStats;
using BrawlBuff.Application.Statistics.Queries.GetBrawlersModesStats;
using BrawlBuff.Application.Statistics.Queries.GetBrawlersStats;
using BrawlBuff.Application.Statistics.Queries.GetMapsModesStats;
using BrawlBuff.Application.Statistics.Queries.GetMapsStats;
using BrawlBuff.Application.Statistics.Queries.GetModesStats;
using BrawlBuff.Application.Statistics.Queries.GetPersonalStats;
using BrawlBuff.Application.Statistics.Queries.GetTeamsMapsModesStats;
using BrawlBuff.Application.Statistics.Queries.GetTeamsMapsStats;
using BrawlBuff.Application.Statistics.Queries.GetTeamsModesStats;
using BrawlBuff.Application.Statistics.Queries.GetTeamsStats;
using Microsoft.AspNetCore.Mvc;

namespace BrawlBuff.Api.Controllers
{
    // Q2L9C0QLQ
    public class StatisticsController : ApiControllerBase
    {
        [HttpGet("player/{tag}")]
        public async Task<IActionResult> GetPlayerStats(string tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetPlayerStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("brawlers")]
        public async Task<IActionResult> GetBrawlersStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetBrawlersStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("maps")]
        public async Task<IActionResult> GetMapsStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetMapsStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("modes")]
        public async Task<IActionResult> GetModesStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetModesStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeamStats([FromQuery] string tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetTeamsStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("maps_modes")]
        public async Task<IActionResult> GetMapsModesStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetMapsModesStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("brawlers_maps")]
        public async Task<IActionResult> GetBrawlersMapsStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetBrawlersMapsStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("brawlers_modes")]
        public async Task<IActionResult> GetBrawlersModesStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetBrawlersModesStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("brawlers_maps_modes")]
        public async Task<IActionResult> GetBrawlersMapsModesStats([FromQuery] string? tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetBrawlersMapsModesStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("teams_modes")]
        public async Task<IActionResult> GetTeamsModesStats([FromQuery] string tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetTeamsModesStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("teams_maps")]
        public async Task<IActionResult> GetTeamsMapsStats([FromQuery] string tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetTeamsMapsStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        [HttpGet("teams_maps_modes")]
        public async Task<IActionResult> GetTeamsMapsModesStats([FromQuery] string tag = "Q2L9C0QLQ")
        {
            NormalizeTag(ref tag);
            var result = await Mediator.Send(new GetTeamsMapsModesStatsQuery { PlayerTag = tag });
            return Ok(result);
        }

        private void NormalizeTag(ref string tag)
        {
            tag = !string.IsNullOrEmpty(tag) ? "#" + tag : tag;
        }
    }
}
