using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlApiHttpService.Models
{
    public class Environment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Path { get; set; }
        public int Version { get; set; }
        public string ImageUrl { get; set; }
    }
}
