using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset;
internal class AssetBLL : StandardBLL<Guid, AssetEntity, AssetData, AssetDetails, BaseFilter>
    , IAssetBLL
    , ISelfRegisteredService<IAssetBLL>
{

    public AssetBLL(IRepository<AssetEntity> repository
        , ILogger<AssetEntity> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<AssetDetails, AssetEntity> detailsBuilder
        , IInsertEntityBLL<AssetEntity, AssetData> insertEntityBLL
        , IModifyEntityBLL<Guid, AssetEntity, AssetData, AssetDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, AssetEntity> removeEntityBLL
        , IGetEntityBLL<Guid, AssetEntity, AssetDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, AssetEntity, AssetDetails, BaseFilter> detailsArrayBLL)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL
            )
    {

    }
}
