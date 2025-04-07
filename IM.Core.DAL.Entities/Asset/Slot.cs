using Inframanager;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.Slot)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Slot_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Slot_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Slot_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Slot_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Slot_Properties)]
public class Slot : SlotBase
{
    public Slot() { }

    public Slot(Guid objectID, int objectClassID, int number, int slotTypeID)
    {
        ObjectID = objectID;
        ObjectClassID = objectClassID;
        Number = number;
        SlotTypeID = slotTypeID;
    }

    public string Note { get; init; }
    public Guid? AdapterID { get; init; }

    public virtual Adapter Adapter { get; init; }
}