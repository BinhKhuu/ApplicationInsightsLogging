using ApplicationInsightsLogging.Infrastructure.DBContext;
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
            var connStr = Environment.GetEnvironmentVariable("SqlServerConnectionString");
            builder.Services.AddDbContext<ApplicationInsightsLoggerContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connStr)
            ); 
        }
    }
}
