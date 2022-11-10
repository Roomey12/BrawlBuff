using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using BrawlBuff.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BrawlBuff.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IPlayerService, PlayerService>();
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddHttpClient<BrawlStarsApiHttpService>();
        services.AddHttpClient<BrawlApiHttpService>();

        return services;
    }
}