using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogTypeModifier : 
    IModifyObject<ProductCatalogType, ProductCatalogTypeData>
    , ISelfRegisteredService<IModifyObject<ProductCatalogType, ProductCatalogTypeData>>

{
    private readonly IMapper _mapper;
    private readonly IRepository<ServiceContractFeature> _serviceContractFeatures;
    private readonly IRepository<ServiceContractTypeAgreement> _serviceContractTypeAgreements;
    //private readonly IAssetCheckerBLL _assetChecker;

    public ProductCatalogTypeModifier(IMapper mapper
        , IRepository<ServiceContractFeature> serviceContractFeatures
        , IRepository<ServiceContractTypeAgreement> serviceContractTypeAgreements)
    {
        _mapper = mapper;
        _serviceContractFeatures = serviceContractFeatures;
        _serviceContractTypeAgreements = serviceContractTypeAgreements;
    }

    public async Task ModifyAsync(
        ProductCatalogType entity, 
        ProductCatalogTypeData data,
        CancellationToken cancellationToken = default)
    {
        //TODO понять почему доп соглашения и features не обновляются вместе с типом при маппинге
        _mapper.Map(data, entity);

        if (data.Agreement is not null)
        {
            var agreements = await _serviceContractTypeAgreements.ToArrayAsync(c => c.ProductCatalogTypeID == entity.IMObjID, cancellationToken);
            agreements.ForEach(a => _serviceContractTypeAgreements.Delete(a));
            _serviceContractTypeAgreements.Insert(entity.ServiceContractTypeAgreement);
        }

        if (entity.ServiceContractFeatures is not null)
        {
            var features = await _serviceContractFeatures.ToArrayAsync(c => c.ProductCatalogTypeID == entity.IMObjID, cancellationToken);
            features.ForEach(a => _serviceContractFeatures.Delete(a));
            entity.ServiceContractFeatures.ForEach(c => _serviceContractFeatures.Insert(c));
        }
        //TODO уточнить со бизнес логику, обновление типа
        //if (_assetChecker.HasAsset(entity))
        //    entity.IsAccountingAsset = data.IsSubjectAsset;

        //return Task.CompletedTask;
    }

    public void SetModifiedDate(ProductCatalogType entity)
    {
    }
}