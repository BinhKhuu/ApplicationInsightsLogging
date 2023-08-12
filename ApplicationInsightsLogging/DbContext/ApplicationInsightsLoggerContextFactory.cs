using ApplicationInsightsLogging.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsightsLogging.Infrastructure.DbContext
{
    public class ApplicationInsightsLoggerContextFactory : IDesignTimeDbContextFactory<ApplicationInsightsLoggerContext>
    {
        public ApplicationInsightsLoggerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationInsightsLoggerContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApplicationInsightsLogger;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            return new ApplicationInsightsLoggerContext(optionsBuilder.Options);
        }
    }
}
