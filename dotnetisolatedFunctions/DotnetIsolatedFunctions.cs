using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace dotnetisolatedFunctions
{
    public class DotnetIsolatedFunctions
    {
        private readonly ILogger _logger;
        public DotnetIsolatedFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DotnetIsolatedFunctions>();
        }

        [Function("SaveToStorage")]       
        public async Task<MyOutputType> SaveToStorage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SaveToStorage/{name}")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Content to store in Storage
            var data = await req.ReadAsStringAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Welcome to Azure Functions!");

            return new MyOutputType()
            {
                 HttpResponse = response,
                 Data = data
            };
        }

        public class MyOutputType
        {
            // This is how output binding needs to address.
            [BlobOutput("sampledata/{name}")]
            public string Data { get; set; }
            public HttpResponseData HttpResponse { get; set; }
        }
    }
}
