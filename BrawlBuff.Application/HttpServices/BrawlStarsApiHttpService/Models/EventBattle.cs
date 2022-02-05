using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models
{
    public class EventBattle
    {
        public int Id { get; set; }
        public string Mode { get; set; }
        public string Map { get; set; }
    }
}
