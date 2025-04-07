using System;

namespace InfraManager.DAL.Asset.Subclasses;

public class Keyboard
{
    public Guid ID { get; init; }
    public string DelayPeriod { get; set; }
    public string NumberKeys { get; set; }
    public string Layout { get; set; }
    public string ConnectorType { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
