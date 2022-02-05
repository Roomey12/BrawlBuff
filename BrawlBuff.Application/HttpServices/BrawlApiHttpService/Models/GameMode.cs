using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlApiHttpService.Models
{
    public class GameMode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public int Version { get; set; }
        public string Color { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
    }
}
