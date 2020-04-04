using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace KedyNakupit
{
    public static class HttpTriggerUploadCashRegisterRecords
    {
        [FunctionName("UploadCashRegisterRecords")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("UploadCashRegisterRecords started");

            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<CashRegisterRecord[]>(requestBody);
            var rows = data as CashRegisterRecord[];

            var totalCount = 0;
            if (rows != null)
            {
                var processor = new RecordProcessor(log);
                totalCount = processor.ProcessRecords(rows);

                log.LogInformation($"Processed {totalCount}/{rows.Length}");
            }

            return new OkObjectResult($"Processed {totalCount} items");
        }
    }
}
