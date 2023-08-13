using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace ApplicationInsightsLogging.Api.Services
{
    public class TelementryService
    {
        public TelemetryClient Client { get; set; }
        public TelementryService(string connectionString) {
            var telementryConfig = TelemetryConfiguration.CreateDefault();
            telementryConfig.ConnectionString = connectionString;
            Client = new TelemetryClient(telementryConfig); 
        }
    }
}
