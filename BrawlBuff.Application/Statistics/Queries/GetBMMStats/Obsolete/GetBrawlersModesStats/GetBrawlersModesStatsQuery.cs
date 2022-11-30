using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersModesStats;

public class GetBrawlersModesStatsQuery : IRequest<GetBrawlersModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
}