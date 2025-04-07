using Inframanager;
using System;

namespace InfraManager.DAL.Asset.DeviceMonitors;

public class AllowedTypeForLabelPrinting
{
    public Guid ID { get; init; }
    public Guid ObjectID { get; init; }
    public ObjectClass ClassID { get; init; }
    public string Name { get; init; }

}
