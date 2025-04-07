using System;

namespace InfraManager.DAL.Asset.Subclasses;

public class Mouse
{
    public Guid ID { get; init; }
    public string ConnectorType { get; set; }
    public string NumberButtons { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
