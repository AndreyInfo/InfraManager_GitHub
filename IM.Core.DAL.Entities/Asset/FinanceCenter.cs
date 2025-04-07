using System;

namespace InfraManager.DAL.Asset;

public class FinanceCenter
{
    public Guid ID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public string Identifier { get; init; }
    public byte[] RowVersion { get; init; }
    public string ExternalID { get; init; }
}