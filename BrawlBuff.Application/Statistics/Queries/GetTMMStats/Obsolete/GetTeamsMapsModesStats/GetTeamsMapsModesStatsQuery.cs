using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsModesStats;

public class GetTeamsMapsModesStatsQuery : IRequest<GetTeamsMapsModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
}