using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.Asset;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.Software;

namespace InfraManager.DAL.ProductCatalogue;

[ObjectClassMapping(ObjectClass.ProductCatalogType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ProductCatalogType_Insert)]
[OperationIdMapping(ObjectAction.Update, OperationID.ProductCatalogType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ProductCatalogType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ProductCatalogType_Details)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ProductCatalogType_Details)]
public class ProductCatalogType : IMarkableForDelete, IGloballyIdentifiedEntity
{
    protected ProductCatalogType()
    {

    }

    public ProductCatalogType(string name)
    {
        IMObjID = Guid.NewGuid();
        Name = name;
    }
    public Guid IMObjID { get; init; }

    public string Name { get; set; }

    public bool? IsLogical { get; set; }

    public byte[] Icon { get; set; }

    public string IconName { get; set; }

    public bool Removed { get; private set; }

    public ProductTemplate ProductCatalogTemplateID { get; set; }

    public Guid ProductCatalogCategoryID { get; set; }

    public Guid? LifeCycleID { get; set; }

    public byte[] RowVersion { get; init; }

    public string ExternalID { get; set; }

    public string ExternalName { get; set; }

    public bool CanBuy { get; set; }

    public bool? IsAccountingAsset { get; set; }
    public Guid? FormID { get; set; }

    public void MarkForDelete() => Removed = true;

    public virtual Form Form { get; init; }
    public virtual LifeCycle LifeCycle { get; init; }
    public virtual ProductCatalogTemplate ProductCatalogTemplate { get; init; }
    public virtual ProductCatalogCategory ProductCatalogCategory { get; init; }
    public virtual ServiceContractTypeAgreement ServiceContractTypeAgreement { get; init; }
    public virtual ICollection<AdapterType> AdapterTypes { get; init; }
    public virtual ICollection<SoftwareLicence> SoftwareLicence { get; init; }
    public virtual ICollection<NetworkDeviceModel> NetworkDeviceModel { get; init; }
    public virtual ICollection<TerminalDeviceModel> TerminalDeviceModel { get; init; }
    public virtual ICollection<PeripheralType> PeripheralType { get; init; }
    public virtual ICollection<CabinetType> RackTypes { get; init; }
    public virtual ICollection<ServiceContractFeature> ServiceContractFeatures { get; init; }
}