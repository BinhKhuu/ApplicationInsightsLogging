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

namespace ApplicationInsightsLogging.Api.Endpoints
{
    public class Trace
    {
        private ApplicationInsightsLoggerContext _loggerContext;
        public Trace(ApplicationInsightsLoggerContext loggerContext)
        {
            _loggerContext = loggerContext;
        }

        [FunctionName("Trace")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var item = _loggerContext.Products.First();


            return new OkObjectResult("Ugh");
        }
    }
}
