using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Domain.Common;
using BrawlBuff.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Infrastructure.Persistence
{
    public class BrawlBuffDbContext : DbContext, IBrawlBuffDbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<BattleDetail> BattleDetails { get; set; }
        public DbSet<Team> Teams { get; set; }
        public override DatabaseFacade Database => base.Database;
        private readonly IDateTime _dateTime;
        private static bool initialized = false;
        public BrawlBuffDbContext(
            DbContextOptions<BrawlBuffDbContext> options,
            IDateTime dateTime)
               : base(options)
        {
            //if (!initialized)
            //{
            //    Database.EnsureDeleted();
            //    Database.EnsureCreated();
            //    initialized = true;
            //}
            _dateTime = dateTime;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedOn = _dateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
