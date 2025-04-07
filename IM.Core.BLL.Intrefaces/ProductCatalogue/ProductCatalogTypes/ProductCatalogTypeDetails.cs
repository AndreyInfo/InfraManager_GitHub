using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractFeatures;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractTypeAgreements;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public class ProductCatalogTypeDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public string IconName { get; init; }

    public string ExternalID { get; init; }

    public string ExternalName { get; init; }

    public bool? IsSubjectAsset { get; set; }

    public bool CanBuy { get; init; }

    public Guid? LifeCycleID { get; init; }

    public LifeCycleType? LifeCycleType { get; init; }

    public Guid? FormID { get; init; }

    public Guid ParentID { get; init; }

    public ProductTemplate TemplateID { get; init; }

    public string LifeCycleName { get; init; }

    public ObjectClass ClassID => ObjectClass.ProductCatalogType;

    public ObjectClass TemplateClassID { get; init; }

    public string TemplateName { get; init; }

    public ObjectClass? ModelClassID { get; set; }

    public ServiceContractTypeAgreementDetails Agreement { get; set; }

    public ServiceContractFeatureDetails[] ServiceContractFeatures { get; set; }

}