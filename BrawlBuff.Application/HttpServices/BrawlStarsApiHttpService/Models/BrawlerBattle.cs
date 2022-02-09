using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models
{
    public class BrawlerBattle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int Trophies { get; set; }
        public int TrophyChange { get; set; }
    }
}
