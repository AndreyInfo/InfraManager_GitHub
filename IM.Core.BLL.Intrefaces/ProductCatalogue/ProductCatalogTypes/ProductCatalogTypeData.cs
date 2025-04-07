using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractFeatures;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes.ServiceContractTypeAgreements;
using InfraManager.DAL.ProductCatalogue;
using System;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public class ProductCatalogTypeData 
{
    public string Name { get; init; }

    public string IconName { get; init; }

    public string ExternalID { get; init; }

    public string ExternalName { get; init; }

    public bool? IsSubjectAsset { get; init; }

    public bool CanBuy { get; init; }

    public Guid? LifeCycleID { get; init; }

    public Guid? FormID { get; init; }

    public Guid ParentID { get; init; }

    public ProductTemplate TemplateID { get; init; }

    public ServiceContractTypeAgreementData Agreement { get; init; }

    public ServiceContractFeatureData[] ServiceContractFeatures { get; init; }

}