using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models
{
    public class Root
    {
        public List<BattleLog> Items { get; set; }
    }
}
