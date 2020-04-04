using System;

public partial class CashRegisterRecord
{ 
    public Guid RecordId { get; set; }
    public DateTime TimeStamp { get; set; }
    public string LegalEntityIdentifier { get; set; } // Slovak IČO
    public string Address { get; set; }
    public int TransactionCount { get; set; } // in case they send grouped result (e.g. by hour)
}

public class LegalEntity
{    
    public string LegalEntityIdentifier { get; set; }
    public string Name { get; set; }
}