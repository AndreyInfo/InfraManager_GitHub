using Inframanager;
using System;

namespace InfraManager.DAL.Asset;

[OperationIdMapping(ObjectAction.Insert, OperationID.CartridgeType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.CartridgeType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.CartridgeType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.CartridgeType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.CartridgeType_Properties)]
public class CartridgeType : Catalog<Guid>
{
    public Guid? ComplementaryID { get; set; }
}
