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
            case BattleResult.Draw:
                return "draw";
            default:
                return null;
        }
    }

    public static BattleResult GetEnum(string battleResult)
    {
        switch (battleResult)
        {
            case "defeat":
                return BattleResult.Defeat;
            case "victory":
                return BattleResult.Victory;
            case "draw":
                return BattleResult.Draw;
            default:
                return BattleResult.Unknown;
        }
    }
}