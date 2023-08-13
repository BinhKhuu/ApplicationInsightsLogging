using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ApplicationInsightsLogging.Infrastructure.DBContext;
using System.Linq;
using Microsoft.ApplicationInsights.Extensibility;
using ApplicationInsightsLogging.Api.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using ApplicationInsightsLogging.Core.Models;
using System.Collections.Generic;

namespace ApplicationInsightsLogging.Api.Endpoints
{
    public class Trace
    {
        private ApplicationInsightsLoggerContext _loggerContext;
        private TelementryService _telementryService;
        public Trace(ApplicationInsightsLoggerContext loggerContext, TelementryService telementryService)
        {
            _loggerContext = loggerContext;
            _telementryService = telementryService;
        }

        [FunctionName("TraceProps")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var item = _loggerContext.Products.First();

            var client = _telementryService.Client;
            // prop__ set directly
            var traceTelementry = new TraceTelemetry();
            traceTelementry.Properties["RequestDate"] = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");
            traceTelementry.Message = "Request Date";

            // prop__ set through Dictionary<string,string>
            client.TrackTrace("First Item Name", SeverityLevel.Information, new Dictionary<string, string> { { "FirstItemName", item.Name } });
            client.TrackTrace(traceTelementry);
    
            // prop__ set through scope
            using(log.BeginScope(new Dictionary<string, object> { ["FirstItem"] = item }))
            {
                // prop__ set through template only works with ILogger
                log.LogInformation("First Item is {FirstItem}", JsonConvert.SerializeObject(item));
            }


            return new OkObjectResult("Ugh");
        }
    }
}
