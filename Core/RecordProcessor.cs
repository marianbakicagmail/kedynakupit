using System;
using Microsoft.Extensions.Logging;

public class RecordProcessor
{
    private ILogger _log;
    public RecordProcessor(ILogger log)
    {
        _log = log;
    }

    public int ProcessRecords(CashRegisterRecord[] records)
    {
        int count = 0;
        using (var ctx = new KedyNakupitContext())
        {
            foreach (var record in records)
            {
                try
                {
                    ctx.CashRegisterRecord.Add(record);

                    if (count % 2000 == 0)
                    {
                        _log.LogInformation($"Processed {count}");
                        ctx.SaveChanges();
                    }

                    count++;
                }
                catch (Exception ex)
                {
                    var innerText = ex.InnerException != null ? ex.InnerException.Message : "";
                    _log.LogWarning($"Could not import rows count: {count} ex: {ex.Message} inner: {innerText}");
                }
            }

            ctx.SaveChanges();
        }

        return count;
    }
}