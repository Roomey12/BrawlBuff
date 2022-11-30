using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetTMMStats;

public class GetTMMStatsQuery : IRequest<GetTMMStatsQueryResult>
{
    public string PlayerTag { get; set; }
    public bool IsTeamRequest { get; set; } = true;
    public bool IsMapRequest { get; set; } = true;
    public bool IsModeRequest { get; set; } = true;
}