using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KedyNakupit
{
    public static class BlobTriggerCSharpDemo
    {
        [FunctionName("UploadCashRegisterRecordsBlob")]
        public static void Run([BlobTrigger("cash-register-records/{name}", Connection = "kedynakupitdemo_STORAGE")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"UploadCashRegisterRecordsBlob started");

            using (StreamReader sr = new StreamReader(myBlob))
            {
                var jsonString = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<CashRegisterRecord[]>(jsonString);

                var rows = data as CashRegisterRecord[];

                var totalCount = 0;
                if (rows != null)
                {
                    log.LogInformation($"Processing started. Count: {rows.Length}");

                    var processor = new RecordProcessorFast(log);
                    totalCount = processor.ProcessRecords(rows);

                    log.LogInformation($"Processed {totalCount}/{rows.Length}");
                }
            }

            log.LogInformation($"UploadCashRegisterRecordsBlob finished");
        }
    }
}
