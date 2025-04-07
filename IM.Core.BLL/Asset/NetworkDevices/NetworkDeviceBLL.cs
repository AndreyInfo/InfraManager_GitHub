using AutoMapper;
using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Asset.NetworkDevices;

internal class NetworkDeviceBLL : EquipmentBaseBLL<int, NetworkDevice, NetworkDeviceData, NetworkDeviceDetails>
    , INetworkDeviceBLL
    , ISelfRegisteredService<INetworkDeviceBLL>
{
    public NetworkDeviceBLL(
          IRepository<NetworkDevice> repository
        , ILogger<NetworkDeviceBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<NetworkDeviceDetails, NetworkDevice> detailsBuilder
        , IInsertEntityBLL<NetworkDevice, NetworkDeviceData> insertEntityBLL
        , IModifyEntityBLL<int, NetworkDevice, NetworkDeviceData, NetworkDeviceDetails> modifyEntityBLL
        , IRemoveEntityBLL<int, NetworkDevice> removeEntityBLL
        , IGetEntityBLL<int, NetworkDevice, NetworkDeviceDetails> detailsBLL
        , IGetEntityArrayBLL<int, NetworkDevice, NetworkDeviceDetails, BaseFilter> detailsArrayBLL
        , IMapper mapper
        , IAssetBLL assetBLL
        )
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
            , mapper
            , assetBLL)
    {
    }
}
