using System;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.PeripherialTypes;

internal class PeripheralModelBLL :
    StandardBLL<Guid, PeripheralType, ProductModelData, ProductModelDetails, ProductModelListFilter>,
    IPeripheralModelBLL, ISelfRegisteredService<IPeripheralModelBLL>
{
    public PeripheralModelBLL(
        IRepository<PeripheralType> repository,
        ILogger<PeripheralModelBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<ProductModelDetails, PeripheralType> detailsBuilder,
        IInsertEntityBLL<PeripheralType, ProductModelData> insertEntityBLL,
        IModifyEntityBLL<Guid, PeripheralType, ProductModelData, ProductModelDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, PeripheralType> removeEntityBLL,
        IGetEntityBLL<Guid, PeripheralType, ProductModelDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, PeripheralType, ProductModelDetails, ProductModelListFilter> detailsArrayBLL)
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