using Inframanager;
using InfraManager.DAL.Asset;
using System;

namespace InfraManager.DAL.ConfigurationUnits;

[ObjectClassMapping(ObjectClass.NetworkNode)]
[OperationIdMapping(ObjectAction.Insert, OperationID.NetworkNode_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.NetworkNode_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.NetworkNode_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.NetworkNode_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.NetworkNode_Properties)]
public class NetworkNode : ConfigurationUnitBase
{
    public string IPAddress { get; init; }
    public string IPMask { get; set; }
    public int? NetworkDeviceID { get; set; }
    public int? TerminalDeviceID { get; set; }
    public Guid? DeviceApplicationID { get; init; }

    public virtual NetworkDevice NetworkDevice { get; }
    public virtual TerminalDevice TermialDevice { get; }
    // public virtual DeviceApplication DeviceApplication { get; } TODO: После влития КЕ-Приложение
}
