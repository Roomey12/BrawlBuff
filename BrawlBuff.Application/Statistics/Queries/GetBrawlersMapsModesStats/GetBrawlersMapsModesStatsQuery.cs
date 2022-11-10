using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsModesStats;

public class GetBrawlersMapsModesStatsQuery : IRequest<GetBrawlersMapsModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
    public bool IsBrawlerRequest { get; set; }
    public bool IsMapRequest { get; set; }
    public bool IsModeRequest { get; set; }
}