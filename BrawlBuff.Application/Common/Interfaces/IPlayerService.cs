using BrawlBuff.Domain.Entities;

namespace BrawlBuff.Application.Common.Interfaces
{
    public interface IPlayerService
    {
        Task RegisterPlayerAsync(string tag);
        Task RecordPlayerBattleStatsAsync(Player player);
    }
}
