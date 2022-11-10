using BrawlBuff.Api.Filters;
using BrawlBuff.Application;
using BrawlBuff.Infrastructure;
using BrawlBuff.Infrastructure.Persistence;

namespace BrawlBuff.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        services.AddLogging();
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();
        services.AddControllers(options =>
        {
            options.Filters.Add<UnhandledExceptionFilterAttribute>();
        });
        services.AddHealthChecks()
            .AddDbContextCheck<BrawlBuffDbContext>();

        // Customise default API behaviour
        //services.Configure<ApiBehaviorOptions>(options =>
        //    options.SuppressModelStateInvalidFilter = true);

        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        //app.UseHealthChecks("/health");
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}