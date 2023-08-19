using ApplicationInsightsLogging.Api.Services;
using ApplicationInsightsLogging.Infrastructure.DBContext;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

[assembly: FunctionsStartup(typeof(LoggingFunctions.Startup))]
namespace LoggingFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var sqlConnectionString = Environment.GetEnvironmentVariable("SqlServerConnectionString");
            builder.Services.AddDbContext<ApplicationInsightsLoggerContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, sqlConnectionString)
            );


            var applicationInsightsConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(applicationInsightsConnectionString))
                applicationInsightsConnectionString = Environment.GetEnvironmentVariable("TelementryConnectionString");

            //builder.Services.Configure<TelemetryConfiguration>(c => c.ConnectionString = applicationInsightsConnectionString);
            // Telementry service set connection string
            builder.Services.AddScoped(t => new TelementryService(applicationInsightsConnectionString));
        }
    }
}
