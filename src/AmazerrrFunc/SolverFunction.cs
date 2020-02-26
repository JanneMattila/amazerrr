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
            log.LogInformation("C# HTTP trigger function processed a request.");

            using var reader = new StreamReader(req.Body);
            var input = await reader.ReadToEndAsync();

            var parser = new Parser();
            var board = parser.Parse(input);

            var solver = new Solver();
            var output = solver.Solve(board);

            return new OkObjectResult(output.Select(o => o.ToString()));
        }
    }
}
