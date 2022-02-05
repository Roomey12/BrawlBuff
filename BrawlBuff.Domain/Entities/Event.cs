using BrawlBuff.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Domain.Entities
{
    public class Event : AuditableEntity
    {
        public int Id { get; set; }
        public int BrawlEventId { get; set; }
        public string? Mode { get; set; }
        public string? Map { get; set; }
        public string? ImageUrl { get; set; }
    }
}
