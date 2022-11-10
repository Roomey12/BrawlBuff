using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetMapsModesStats;

public class GetMapsModesStatsQuery : IRequest<GetMapsModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
}