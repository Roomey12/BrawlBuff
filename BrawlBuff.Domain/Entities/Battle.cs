using BrawlBuff.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Domain.Entities
{
    public class Battle : AuditableEntity
    {
        public int Id { get; set; }
        public Event? Event { get; set; }
        public int? EventId { get; set; }
        public string? StarPlayerTag { get; set; }
        public string? Type { get; set; }
        public DateTime BattleTime { get; set; }
        public int? Duration { get; set; }
    }
}
