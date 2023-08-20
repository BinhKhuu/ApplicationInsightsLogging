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
using System.Collections.Generic;

namespace ApplicationInsightsLogging.Api.Endpoints
{
    public class CustomEvents
    {
        private TelementryService _telementryService;
        public CustomEvents(TelementryService telementryService)
        {
            _telementryService = telementryService;
        }


        [FunctionName("CustomEvents")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var client = _telementryService.Client;
            // custom event with name = 'UGH Custom Event'
            using (var operation = client.StartOperation<RequestTelemetry>("UGH Custom Event"))
            {
                client.TrackEvent("WinGame");
                operation.Telemetry.ResponseCode = "233";
                client.StopOperation(operation);
            }

            // no out of the box customEvent logging for ILogger

            return new OkObjectResult("Ugh");
        }
    }
}
