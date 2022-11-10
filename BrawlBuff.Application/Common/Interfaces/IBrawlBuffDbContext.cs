using BrawlBuff.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BrawlBuff.Application.Common.Interfaces;

public interface IBrawlBuffDbContext
{
    DbSet<Player> Players { get; set; }
    DbSet<Event> Events { get; set; }
    DbSet<Battle> Battles { get; set; }
    DbSet<BattleDetail> BattleDetails { get; set; }
    DbSet<Team> Teams { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    DatabaseFacade Database { get; }
}