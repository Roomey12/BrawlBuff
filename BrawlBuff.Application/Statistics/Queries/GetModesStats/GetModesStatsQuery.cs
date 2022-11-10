using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetModesStats;

public class GetModesStatsQuery : IRequest<GetModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
}