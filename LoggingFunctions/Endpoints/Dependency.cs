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
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using ApplicationInsightsLogging.Infrastructure.DBContext;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.DependencyModel;

namespace ApplicationInsightsLogging.Api.Endpoints
{
    public class Dependency
    {
        private ApplicationInsightsLoggerContext _loggerContext;
        private readonly TelementryService _telementryService;
        public Dependency(ApplicationInsightsLoggerContext loggerContext, TelementryService telementryService)
        {
            _telementryService = telementryService;
            _loggerContext = loggerContext;
        }
        // https://learn.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics#trackdependency
        [FunctionName("Dependency")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var client = _telementryService.Client;

            // start operation to correlate performance
            // https://learn.microsoft.com/en-us/azure/azure-monitor/app/custom-operations-tracking#outgoing-dependencies-tracking
            using (var operation = client.StartOperation<DependencyTelemetry>("Custom Dependency"))
            {
                try
                {
                    // dependency will be auto logged by insights
                    var item = _loggerContext.Products.First();
                    operation.Telemetry.Success = true;
                    operation.Telemetry.ResultCode = "200";
                }
                catch (Exception ex) 
                {
                    operation.Telemetry.Success = false;
                    operation.Telemetry.ResultCode = "500";
                    client.TrackException(ex);
                }
                finally
                {
                    client.StopOperation(operation);
                }
            }

            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var item = _loggerContext.Products.First();
            }
            catch (Exception ex)
            {
                success = false;
                client.TrackException(ex);
                throw new Exception("Operation went wrong", ex);
            }
            finally
            {
                timer.Stop();
                client.TrackDependency("DependencyType", "myDependency", "myCall", startTime, timer.Elapsed, success);
            }


            return new OkObjectResult("ugh");
        }
    }
}
