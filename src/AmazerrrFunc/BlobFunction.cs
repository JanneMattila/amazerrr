using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazerrr;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AmazerrrFunc
{
    public static class BlobFunction
    {
        [FunctionName("BlobFunction")]
        [return: Table("puzzles", Connection = "Storage")]
        public static SolutionOutput Run([BlobTrigger("puzzles/{name}", Connection = "Storage")]Stream imageBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {imageBlob.Length} Bytes");

            using var reader = new BinaryReader(imageBlob);
            var imageData = reader.ReadBytes((int)imageBlob.Length);

            var imageAnalyzer = new ImageAnalyzer(log);
            var input = imageAnalyzer.Analyze(imageData);

            var parser = new Parser(log);
            var board = parser.Parse(input);

            var solver = new Solver();
            var output = solver.Solve(board);

            return new SolutionOutput()
            {
                PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                RowKey = DateTime.UtcNow.ToString("HH-mm-ss-ff"),
                Solution = new List<string>(output.Select(o => o.ToString()))
            };
        }

        public class SolutionOutput
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public List<string> Solution { get; set; }
        }
    }
}
