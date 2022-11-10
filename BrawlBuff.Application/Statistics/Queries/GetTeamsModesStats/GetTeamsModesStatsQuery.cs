using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsModesStats;

public class GetTeamsModesStatsQuery : IRequest<GetTeamsModesStatsQueryResult>
{
    public string PlayerTag { get; set; }
}