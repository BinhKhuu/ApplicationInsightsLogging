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
using ApplicationInsightsLogging.Infrastructure.DBContext;
using System.Linq;
using System.Collections.Generic;

namespace ApplicationInsightsLogging.Api.Endpoints
{
    public class ExceptionLog
    {
        private ApplicationInsightsLoggerContext _loggerContext;
        private TelementryService _telementryService;
        public ExceptionLog(ApplicationInsightsLoggerContext loggerContext, TelementryService telementryService) {
            _loggerContext = loggerContext;
            _telementryService = telementryService;
        }

        [FunctionName("Exception")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var client = _telementryService.Client;
            try
            {
                using (log.BeginScope(new Dictionary<string, object> { ["MyScopedProps"] = 101 }))
                {
                    var item = _loggerContext.Products.First();
                    // application insights will automatically log an exception for you, logged exception 1
                    throw new Exception("Exception Thrown in /Exception");
                }

            }
            catch (Exception ex) when (FlushLog(ex, log))
            {
                client.TrackException(ex);
                var exceptionProperties = new Dictionary<string, string>()
                {
                    ["MyExceptionProperty1"] = ex.StackTrace,
                    ["MyExceptionProperty2"] = JsonConvert.SerializeObject(ex.InnerException),
                };
                // logged exception 3
                client.TrackException(ex, exceptionProperties);
            }

            return new OkObjectResult("Ugh");
        }

        private static bool FlushLog(Exception ex, ILogger log)
        {
            // logged exception 2
            log.LogError(ex, "Exception thrown at {time}, flushing Logs", DateTime.Now);
            return true;
        }
    }
}
