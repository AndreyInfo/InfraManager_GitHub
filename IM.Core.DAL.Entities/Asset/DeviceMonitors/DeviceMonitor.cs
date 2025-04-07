using System;


namespace InfraManager.DAL.Asset.DeviceMonitors;

//TODO найти или добавить права
public class DeviceMonitor
{
    public Guid ID { get; init; }
    public Guid ObjectID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public bool Supervise { get; init; }
    public bool Template { get; init; }
    public int IntervisitInterval { get; init; }
    public int PeriodOfStorage { get; init; }
    public byte[] RowVersion { get; init; }

}
