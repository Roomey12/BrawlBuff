using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsStats;

public class GetTeamsMapsStatsQuery : IRequest<GetTeamsMapsStatsQueryResult>
{
    public string PlayerTag { get; set; }
}