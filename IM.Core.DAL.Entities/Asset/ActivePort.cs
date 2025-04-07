using Inframanager;
using InfraManager.DAL.Configuration;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.ActivePort)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ActivePort_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ActivePort_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ActivePort_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ActivePort_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ActivePort_Properties)]
public partial class ActivePort
{
    public ActivePort() { }

    public ActivePort(int? activeEquipmentID, Guid? iMObjID, int? portNumber, int? jackTypeID, int? technologyTypeID)
    {
        ActiveEquipmentID = activeEquipmentID;
        IMObjID = iMObjID;
        PortNumber = portNumber;
        JackTypeID = jackTypeID;
        TechnologyTypeID = technologyTypeID;
    }

    public int ID { get; init; }
    public string PortName { get; set; }
    public int? JackTypeID { get; init; }
    public int? TechnologyTypeID { get; init; }
    public string PortAddress { get; set; }
    public string PortIPX { get; set; }
    public string GroupNumber { get; set; }
    public string PortSpeed { get; set; }
    public int? PortVLAN { get; set; }
    public string PortFilter { get; set; }
    public int? PortState { get; set; }
    public int PortStatus { get; set; }
    public Guid? PortModule { get; set; }
    public int? SlotNumber { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public Guid? IMObjID { get; init; }
    public int? ActiveEquipmentID { get; init; }
    public int? PortNumber { get; init; }
    public int? Connected { get; set; }
    public string Connection1 { get; set; }
    public int? VisioID { get; set; }
    public string ExternalID { get; set; }
    public int? Number { get; set; }
    public int ClassID { get; set; }
    public int? TelephoneLineTypeID { get; set; }
    public string TelephoneNumber { get; set; }
    public Guid TelephoneCategoryID { get; set; }
    public int? VoiceMail { get; set; }
    public int? RingGroup { get; set; }
    public int? PickUpGroup { get; set; }
    public int? HuntingGroup { get; set; }
    public int? PermisionGroup { get; set; }
    public int? PageGroup { get; set; }
    public string Connection { get; set; }
    public int? ConnectorID { get; set; }
    public int? ConnectedPortId { get; set; }
    public Guid? PeripheralDatabaseID { get; set; }
    public int? ComplementaryID { get; set; }

    public virtual ConnectorType JackType { get; init; }
    public virtual TechnologyType TechnologyType { get; init; }
}