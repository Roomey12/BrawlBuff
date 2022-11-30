using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsModesStats;

public class GetBrawlersMapsModesStatsQuery : IRequest<GetBrawlersMapsModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
}