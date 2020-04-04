using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class RecordProcessorFast
{
    private ILogger _log;
    public RecordProcessorFast(ILogger log)
    {
        _log = log;
    }

    public int ProcessRecords(CashRegisterRecord[] records)
    {
        int count = 0;
        try
        {
            var cString = "";
            using (var ctx = new KedyNakupitContext())
            {
                cString = ctx.Database.GetDbConnection().ConnectionString;
            }

            var table = new DataTable();
            table.Columns.Add("RecordId", typeof(Guid));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            table.Columns.Add("LegalEntityIdentifier", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("TransactionCount", typeof(int));

            _log.LogInformation($"Started adding rows to datatable");

            foreach (var record in records)
            {
                table.Rows.Add(record.RecordId, record.TimeStamp, record.LegalEntityIdentifier, record.Address, record.TransactionCount);
            }

            _log.LogInformation($"Finished adding rows to datatable ({records.Length})");


            var conn = new SqlConnection(cString);
            conn.Open();
            using (var bulk = new SqlBulkCopy(conn))
            {
                bulk.DestinationTableName = "CashRegisterRecord";
                bulk.WriteToServer(table);
            }
            conn.Close();
        }
        catch (Exception ex)
        {
            var innerText = ex.InnerException != null ? ex.InnerException.Message : "";
            _log.LogWarning($"Could not import rows count: {count} ex: {ex.Message} inner: {innerText}");
        }

        return count;
    }
}