using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using BrawlBuff.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BrawlBuff.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IPlayerService, PlayerService>();

            services.AddHttpClient<BrawlStarsApiHttpService>();
            services.AddHttpClient<BrawlApiHttpService>();

            return services;
        }
    }
}
