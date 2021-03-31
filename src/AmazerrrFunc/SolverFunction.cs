using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazerrr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AmazerrrFunc
{
    public static class SolverFunction
    {
        [FunctionName("Solver")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using var scope = log.BeginScope("Solver");
            log.LogInformation("Solver function processing request.");

            string input;

            if (req.ContentType == null ||
                req.ContentType.Contains("text/plain"))
            {
                log.LogTrace("Content-type is text");
                using var reader = new StreamReader(req.Body);
                input = await reader.ReadToEndAsync();
            }
            else
            {
                log.LogTrace("Content-type is assumed to be image");
                if (!req.ContentLength.HasValue)
                {
                    log.LogWarning("Content-Length is required header.");
                    return new BadRequestObjectResult("Content-Length is required header.");
                }

                const int maxSize = 10_000_000;
                if (req.ContentLength.Value > maxSize)
                {
                    log.LogWarning("Too large content. Try with smaller image.");
                    return new BadRequestObjectResult("Too large content. Try with smaller image.");
                }

                using var reader = new BinaryReader(req.Body);
                var imageData = reader.ReadBytes((int)req.ContentLength.Value);

                var imageAnalyzer = new ImageAnalyzer(log);
                input = imageAnalyzer.Analyze(imageData);
            }

            var parser = new Parser(log);
            var board = parser.Parse(input);

            var solver = new Solver(log);
            var output = solver.Solve(board);

            return new OkObjectResult(output.Select(o => o.ToString()));
        }
    }
}
