using BrawlBuff.Application;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using BrawlBuff.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.IO;
using BrawlBuff.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(BrawlBuff.RecordData.Startup))]

namespace BrawlBuff.RecordData
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(x =>
            {
                x.AddConfiguration(builder.GetContext().Configuration.GetSection("Logging"));
            });
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.GetContext().Configuration);
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            builder.ConfigurationBuilder.AddAzureKeyVault();
        }
    }
}
