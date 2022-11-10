using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsStats;

public class GetBrawlersMapsStatsQuery : IRequest<GetBrawlersMapsStatsQueryResult>
{
    public string PlayerTag { get; set; }
}