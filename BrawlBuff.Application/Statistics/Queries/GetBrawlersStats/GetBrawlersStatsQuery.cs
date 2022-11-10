using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersStats;

public class GetBrawlersStatsQuery : IRequest<GetBrawlersStatsQueryResult>
{
    public string PlayerTag { get; set; }
}