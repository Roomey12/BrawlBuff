using BrawlBuff.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Domain.Entities
{
    public class Player : AuditableEntity
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public DateTime StatsUpdatedOn { get; set; }
        public Player(string tag)
        {
            Tag = tag;
        }
        public Player() { }
    }
}
