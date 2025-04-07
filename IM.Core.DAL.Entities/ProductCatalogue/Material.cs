using Inframanager;
using InfraManager.DAL.Location;
using System;

namespace InfraManager.DAL.ProductCatalogue;

[ObjectClassMapping(ObjectClass.Material)]
//TODO узнать права на материалы
//[OperationIdMapping(ObjectAction.Insert, OperationID.NetworkDevice_Add)]
//[OperationIdMapping(ObjectAction.Update, OperationID.NetworkDevice_Update)]
//[OperationIdMapping(ObjectAction.Delete, OperationID.NetworkDevice_Delete)]
//[OperationIdMapping(ObjectAction.ViewDetails, OperationID.NetworkDevice_Properties)]
//[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.NetworkDevice_Properties)]
public class Material : IProduct<MaterialModel>
{
    public Guid MaterialID { get; init; }

    public int MaterialOperationID { get; init; }

    public Guid? MaterialModelID { get; init; }

    public DateTime Date { get; init; }

    public decimal Amount { get; init; }

    public decimal Cost { get; init; }

    public Guid? SupplierID { get; init; }

    public string Document { get; init; }

    public DateTime? UseByDate { get; init; }

    public int? ResponsibleUserID { get; init; }

    public int? ExecutiveUserID { get; init; }

    public int? RoomID { get; init; }

    public int? PreviousRoomID { get; init; }

    public string DeviceID { get; init; }

    public string Device { get; init; }

    public string Note { get; init; }

    public Guid? PeripheralDatabaseID { get; init; }

    public Guid? ComplementaryID { get; init; }

    public Guid? LifeCycleStateID { get; init; }

    public Guid? GoodsInvoiceID { get; init; }

    public Guid? OwnerID { get; init; }

    public ObjectClass OwnerClassID { get; init; }

    public Guid StorageLocationID { get; init; }

    public virtual MaterialModel Model { get; init; }
    public virtual StorageLocation StorageLocation { get; init; }
}

