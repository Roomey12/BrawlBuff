using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetMapsStats;

public class GetMapsStatsQuery : IRequest<GetMapsStatsQueryResult>
{
    public string PlayerTag { get; set; }
}