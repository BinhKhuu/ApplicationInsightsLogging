using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationInsightsLogging.Core.Models;
using ApplicationInsightsLogging.Infrastructure.DBContext.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ApplicationInsightsLogging.Infrastructure.DBContext
{
    public class ApplicationInsightsLoggerContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationInsightsLoggerContext(DbContextOptions options) : base(options) { 
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

        public DbSet<Product> Products { get; set; }
    }


}
