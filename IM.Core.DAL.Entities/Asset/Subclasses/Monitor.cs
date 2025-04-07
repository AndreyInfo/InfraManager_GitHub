using System;

namespace InfraManager.DAL.Asset.Subclasses;

public class Monitor
{
    public Guid ID { get; init; }
    public string Resolution { get; set; }
    public string FontResolution { get; set; }
    public string Diagonal { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public Guid? ComplementaryID { get; set; }
}
