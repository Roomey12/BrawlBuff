using BrawlBuff.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Domain.Extensions
{
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
}
