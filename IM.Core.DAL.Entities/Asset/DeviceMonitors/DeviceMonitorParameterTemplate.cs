using Inframanager;
using System;

namespace InfraManager.DAL.Asset.DeviceMonitors;

[ObjectClassMapping(ObjectClass.DeviceMonitorParameterTemplate)]
[OperationIdMapping(ObjectAction.Insert, OperationID.DeviceMonitorParameterTemplate_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.DeviceMonitorParameterTemplate_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.DeviceMonitorParameterTemplate_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.DeviceMonitorParameterTemplate_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.DeviceMonitorParameterTemplate_Properties)]
public class DeviceMonitorParameterTemplate
{
    public Guid ID { get; init; }
    public Guid ObjectID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    //TODO скорее всего, есть enum завести или создать
    public byte Type { get; set; }
    public byte[] Value { get; set; }
    public byte[] RowVersion { get; init; }
}
