using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InProcessFunction
{
    public static class InProcessFunctions
    {
        [FunctionName("SaveToStorage")]
        public static async Task<IActionResult> SaveToStorage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "SaveToStorage/{name}")] HttpRequest req,
            [Blob("sampledata/{name}", FileAccess.Write)] Stream samplefile,            
            string name,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");           

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            await samplefile.WriteAsync(System.Text.Encoding.UTF8.GetBytes(requestBody));

            string responseMessage = $"File {name} save successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
