using System;
using System.IO;
using System.Threading.Tasks;
using Amazerrr;
using Microsoft.Extensions.Logging.Abstractions;

namespace AmazerrrConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Amazerrr");
            if (args.Length == 1)
            {
                var imageData = await File.ReadAllBytesAsync(args[0]);

                var imageAnalyzer = new ImageAnalyzer(NullLogger.Instance);
                var input = imageAnalyzer.Analyze(imageData);

                Console.WriteLine("Board:");
                Console.WriteLine(input);
                Console.WriteLine();

                var parser = new Parser(NullLogger.Instance);
                var board = parser.Parse(input);
                
                var solver = new Solver(NullLogger.Instance);
                var output = solver.Solve(board);

                Console.WriteLine($"Solution in {output.Count} moves:");
                foreach (var item in output)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
    }
}
