using System.IO;
using Amazerrr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AmazerrrFunc
{
    public static class ImageAnalyzerFunction
    {
        [FunctionName("ImageAnalyzer")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!req.ContentLength.HasValue)
            {
                return new BadRequestObjectResult("Content-Length is required header.");
            }

            const int maxSize = 10_000_000;
            if (req.ContentLength.Value > maxSize)
            {
                return new BadRequestObjectResult("Too large content. Try with smaller image.");
            }

            using var reader = new BinaryReader(req.Body);
            var imageData = reader.ReadBytes((int)req.ContentLength.Value);

            var imageAnalyzer = new ImageAnalyzer(log);
            var output = imageAnalyzer.Analyze(imageData);

            return new OkObjectResult(output);
        }
    }
}
