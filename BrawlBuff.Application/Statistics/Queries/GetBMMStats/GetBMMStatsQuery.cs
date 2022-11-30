using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetBMMStats;

public class GetBMMStatsQuery : IRequest<GetBMMStatsQueryResult>
{
    public string PlayerTag { get; set; }
    public bool IsBrawlerRequest { get; set; } = true;
    public bool IsMapRequest { get; set; } = true;
    public bool IsModeRequest { get; set; } = true;
}