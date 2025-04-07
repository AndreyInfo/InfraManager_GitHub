using System;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.AdapterTypes;

internal class AdapterModelBLL :
    StandardBLL<Guid, AdapterType, ProductModelData, ProductModelDetails, ProductModelListFilter>,
    IAdapterModelBLL, ISelfRegisteredService<IAdapterModelBLL>
{
    public AdapterModelBLL(
        IRepository<AdapterType> repository,
        ILogger<AdapterModelBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<ProductModelDetails, AdapterType> detailsBuilder,
        IInsertEntityBLL<AdapterType, ProductModelData> insertEntityBLL,
        IModifyEntityBLL<Guid, AdapterType, ProductModelData, ProductModelDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, AdapterType> removeEntityBLL,
        IGetEntityBLL<Guid, AdapterType, ProductModelDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, AdapterType, ProductModelDetails, ProductModelListFilter> detailsArrayBLL)
        : base(
            repository,
            logger,
            unitOfWork,
            currentUser,
            detailsBuilder,
            insertEntityBLL,
            modifyEntityBLL,
            removeEntityBLL,
            detailsBLL,
            detailsArrayBLL)
    {
    }
}