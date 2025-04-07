using Inframanager;
using InfraManager.DAL.ProductCatalogue;
using System;

namespace InfraManager.DAL.Asset;

[ObjectClassMapping(ObjectClass.ServiceContractModel)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ServiceContractLicence_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ServiceContractLicence_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ServiceContractModel_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ServiceContractModel_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ProductCatalogType_Details)]
public class ServiceContractModel : IMarkableForDelete
    , IProductModel<Guid>
{
    public Guid IMObjID { get; init; }
    public string Name { get; init; }
    public Guid ManufacturerID { get; init; }
    public string ContractSubject { get; init; }
    public bool UpdateAvailable { get; init; }
    public string Note { get; init; }
    public bool Removed { get; protected set; }
    public byte[] RowVersion { get; init; }
    public string ExternalID { get; init; }
    public bool CanBuy { get; init; }
    public Guid ProductCatalogTypeID { get; init; }
    public virtual ProductCatalogType ProductCatalogType { get; init; }

    public void MarkForDelete()
    {
        Removed = true;
    }
}