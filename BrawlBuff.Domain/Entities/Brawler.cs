﻿using BrawlBuff.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Domain.Entities
{
    public class Brawler : AuditableEntity
    {
        public int Id { get; set; }
    }
}
