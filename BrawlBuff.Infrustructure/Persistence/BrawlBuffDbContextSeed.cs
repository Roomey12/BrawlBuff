using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Domain.Entities;

namespace BrawlBuff.Infrastructure.Persistence;

public class BrawlBuffDbContextSeed
{
    public static async Task SeedSampleDataAsync(BrawlBuffDbContext context, BrawlApiHttpService brawlApiHttpService)
    {
        if (!context.Events.Any())
        {
            var maps = await brawlApiHttpService.GetMapsAsync();

            var events = maps.Select(x =>
            {
                var mode = x.GameMode.Name.Replace(" ", "");
                return new Event()
                {
                    BrawlEventId = x.Id,
                    Map = x.Name,
                    ImageUrl = x.ImageUrl,
                    Mode = string.Concat(mode[0].ToString().ToLower(), mode.AsSpan(1)),
                };
            }).ToList();

            await context.Events.AddRangeAsync(events);

            await context.SaveChangesAsync();
        }
    }
}