using System;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.NetworkDeviceModels;

internal class NetworkProductModelBLL :
    StandardBLL<Guid, NetworkDeviceModel, ProductModelData, ProductModelDetails, ProductModelListFilter>,
    INetworkDeviceModelBLL, ISelfRegisteredService<INetworkDeviceModelBLL>
{
    public NetworkProductModelBLL(
        IRepository<NetworkDeviceModel> repository,
        ILogger<NetworkProductModelBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<ProductModelDetails, NetworkDeviceModel> detailsBuilder,
        IInsertEntityBLL<NetworkDeviceModel, ProductModelData> insertEntityBLL,
        IModifyEntityBLL<Guid, NetworkDeviceModel, ProductModelData, ProductModelDetails> modifyEntityBLL,
        IRemoveEntityBLL<Guid, NetworkDeviceModel> removeEntityBLL,
        IGetEntityBLL<Guid, NetworkDeviceModel, ProductModelDetails> detailsBLL,
        IGetEntityArrayBLL<Guid, NetworkDeviceModel, ProductModelDetails, ProductModelListFilter> detailsArrayBLL)
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