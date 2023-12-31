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

        [FunctionName("Trace")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var item = _loggerContext.Products.First();

            var client = _telementryService.Client;

            // prop__ set directly
            /*
            traces
            | where isnotnull(customDimensions["RequestDate"]) or isnotnull( customDimensions["prop__FirstItem"])

            requests
            | where resultCode == "222"
            */
            using (var operation = client.StartOperation<RequestTelemetry>("UGH Request"))
            {
                operation.Telemetry.ResponseCode = "222";
                var traceTelementry = new TraceTelemetry();
                // Props set like this appear in custom dimensions without the props__ prefix
                traceTelementry.Properties["RequestDate"] = DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss");
                traceTelementry.Message = "Request Date";

                // prop__ set through Dictionary<string,string>
                client.TrackTrace("Trace First Item Name", SeverityLevel.Information, new Dictionary<string, string> { { "FirstItemName", item.Name } });
                client.TrackTrace(traceTelementry);

                // prop__ set through scope
                using (log.BeginScope(new Dictionary<string, object> { ["FirstItem"] = item }))
                {
                    // prop__ set through template only works with ILogger
                    log.LogInformation("Trace First Item is {FirstItem}", JsonConvert.SerializeObject(item));
                }

                client.StopOperation(operation);
            }

            return new OkObjectResult("Ugh");
        }
    }
}
