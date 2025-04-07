using System;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.TerminalDeviceModels;

internal class TerminalProductModelBLL :
    StandardBLL<Guid, TerminalDeviceModel, ProductModelData, ProductModelDetails, ProductModelListFilter>,
    ITerminalDeviceModelBLL, ISelfRegisteredService<ITerminalDeviceModelBLL>
{
    public TerminalProductModelBLL(
        IRepository<TerminalDeviceModel> repository,
        ILogger<TerminalProductModelBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<ProductModelDetails, TerminalDeviceModel> detailsBuilder,
        IInsertEntityBLL<TerminalDeviceModel, ProductModelData> insertEntityBLL,
        IModifyEntityBLL<Guid, TerminalDeviceModel, ProductModelData, ProductModelDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, TerminalDeviceModel> removeEntityBLL,
        IGetEntityBLL<Guid, TerminalDeviceModel, ProductModelDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, TerminalDeviceModel, ProductModelDetails, ProductModelListFilter> detailsArrayBLL)
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