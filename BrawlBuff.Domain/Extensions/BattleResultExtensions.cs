using BrawlBuff.Domain.Enums;

namespace BrawlBuff.Domain.Extensions;

public static class BattleResultExtensions
{
    public static string GetString(this BattleResult battleResult)
    {
        switch (battleResult)
        {
            case BattleResult.Defeat:
                return "defeat";
            case BattleResult.Victory:
                return "victory";
            default:
                return null;
        }
    }
}