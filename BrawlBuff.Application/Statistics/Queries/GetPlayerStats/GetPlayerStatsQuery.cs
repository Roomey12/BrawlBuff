using MediatR;

namespace BrawlBuff.Application.Statistics.Queries.GetPersonalStats;

public class GetPlayerStatsQuery : IRequest<GetPlayerStatsQueryResult>
{
    public string PlayerTag { get; set; }
}