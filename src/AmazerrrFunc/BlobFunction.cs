using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AmazerrrFunc
{
    public static class BlobFunction
    {
        [FunctionName("BlobFunction")]
        public static void Run([BlobTrigger("puzzles/{name}", Connection = "Storage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
