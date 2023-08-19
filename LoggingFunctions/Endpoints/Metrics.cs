using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ApplicationInsightsLogging.Api.Services;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace ApplicationInsightsLogging.Api.Endpoints
{
    public class Metrics
    {
        private TelementryService _telementryService;
        public Metrics(TelementryService telementryService) { 
            _telementryService = telementryService;
        }

        [FunctionName("Metrics")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            /*
            
            customEvents
            | union customMetrics 
             
             */
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            stopwatch.Start();

            var client = _telementryService.Client;
            var sample = new MetricTelemetry();
            sample.Name = "queueLength";
            sample.Sum = 42.3;
            client.TrackMetric(sample);

            var metrics = new Dictionary<string, double>
            {
                {"FromTracer Processing Time MilliSeconds", stopwatch.Elapsed.TotalMilliseconds }
            };
            var properties = new Dictionary<string, string>
            {
                {"signalSource", "Ugh2"}
            };

            using (var operation = client.StartOperation<RequestTelemetry>("Ugh Metric Request"))
            {
                operation.Telemetry.ResponseCode = "232";
                client.TrackEvent("Ugh event", properties, metrics);
            }

            var myScope = new Dictionary<string, string>
            {
                { "MyScope", "102"}
            };

            using (log.BeginScope(myScope))
            {
                log.LogMetric("From ILogger Metric", stopwatch.Elapsed.TotalMilliseconds);
            }

            stopwatch.Stop();

            return new OkObjectResult("Ugh");
        }
    }
}
