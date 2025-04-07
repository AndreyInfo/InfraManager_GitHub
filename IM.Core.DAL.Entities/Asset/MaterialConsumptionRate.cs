using System;
using Inframanager;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.DAL.Asset;

[OperationIdMapping(ObjectAction.Insert, OperationID.MaterialConsumption_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.MaterialConsumption_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.MaterialConsumption_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.MaterialConsumption_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.MaterialConsumption_Properties)]
public class MaterialConsumptionRate : IProduct<MaterialModel>
{
    public Guid ID { get; init; }
    
    public string DeviceModelID { get; init; }

    public int DeviceCategoryID { get; init; }

    public Guid MaterialModelID { get; init; }

    public decimal Amount { get; init; }
    
    public short? UseBWPrint { get; init; }

    public short? UseColorPrint { get; init; }

    public short? UsePhotoPrint { get; init; }

    public virtual MaterialModel Model { get; init; }
    
}