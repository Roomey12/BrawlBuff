using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsStats;

public class GetTeamsStatsQuery : IRequest<GetTeamsStatsQueryResult>
{
    public string PlayerTag { get; set; }
}