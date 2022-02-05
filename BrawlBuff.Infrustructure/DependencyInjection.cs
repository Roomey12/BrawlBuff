using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Infrastructure.Persistence;
using BrawlBuff.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrawlBuff.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BrawlBuffDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("BrawlBuffConnectionString"),
                    new MySqlServerVersion(new Version(8, 0, 26)),
                    b => b.MigrationsAssembly(typeof(BrawlBuffDbContext).Assembly.FullName)));

            services.AddScoped<IBrawlBuffDbContext>(provider => provider.GetRequiredService<BrawlBuffDbContext>());

            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
