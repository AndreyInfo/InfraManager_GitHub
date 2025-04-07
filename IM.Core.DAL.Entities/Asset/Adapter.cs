using Inframanager;
using InfraManager.DAL.Location;
using System;
using InfraManager.DAL.Asset.History;

namespace InfraManager.DAL.Asset
{
    [ObjectClassMapping(ObjectClass.Adapter)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Adapter_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Adapter_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Adapter_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Adapter_Details)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Adapter_Details)]
    public class Adapter : IProduct<AdapterType>, IHardwareAsset, IHistoryNamedEntity, IGloballyIdentifiedEntity
    {
        public Adapter()
        {
            IMObjID = Guid.NewGuid();
        }
        public Guid IMObjID { get; init; }

        public Guid? AdapterTypeID { get; set; }

        public string Name { get; set; }

        public string SerialNumber { get; set; }

        public string Note { get; set; }

        public int ID { get; init; }

        public bool Integrated { get; set; }

        public int StateID { get; set; }

        public int RoomID { get; set; }

        public ObjectClass? ClassID { get; set; }

        public Guid? PeripheralDatabaseID { get; init; }

        public Guid? ComplementaryID { get; init; }

        public int? ComplementaryIntID { get; init; }

        public string Identifier { get; set; }

        public string Code { get; set; }

        public int? SlotTypeID { get; init; }

        public string ExternalID { get; set; }

        public int? TerminalDeviceID { get; set; }

        public int? NetworkDeviceID { get; set; }

        public byte[] RowVersion { get; init; }

        public string InventoryNumber { get; set; }

        public virtual AdapterType Model { get; set; }
        public virtual TerminalDevice TerminalDevice { get; init; }
        public virtual Room Room { get; init; }
        public virtual NetworkDevice NetworkDevice { get; init; }
        public virtual SlotType SlotType { get; init; }

        public string GetObjectName() => $"{Name} {SerialNumber}";
    }
}