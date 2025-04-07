using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
namespace InfraManager.DAL.ProductCatalogue;
public class ServiceContractTypeAgreement
{
    protected ServiceContractTypeAgreement()
    {

    }

    public ServiceContractTypeAgreement(Guid productCatalogTypeID, Guid agreementLifeCycleID)
    {
        ProductCatalogTypeID = productCatalogTypeID;
        AgreementLifeCycleID = agreementLifeCycleID;
    }

    public Guid ProductCatalogTypeID { get; init;  }
    public Guid AgreementLifeCycleID { get; init; }

    public virtual LifeCycle LifeCycle { get; init; }
    public virtual ProductCatalogType ProductCatalogType { get; init; }
}
