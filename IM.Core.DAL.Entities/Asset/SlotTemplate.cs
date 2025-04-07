using Inframanager;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.SlotTemplate)]
[OperationIdMapping(ObjectAction.Insert, OperationID.SlotTemplate_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.SlotTemplate_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.SlotTemplate_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.SlotTemplate_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SlotTemplate_Properties)]
public class SlotTemplate : SlotBase
{
}
