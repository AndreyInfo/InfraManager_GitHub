using AutoMapper;
using InfraManager.DAL.Asset;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.NetworkDevices;

internal class NetworkDeviceProfile : Profile
{
    public NetworkDeviceProfile()
    {
        CreateMap<NetworkDevice, NetworkDeviceDetails>()
            .ForMember(dest => dest.BuildingName, mapper => mapper.MapFrom(src => src.Room.Floor.Building.Name))
            .ForMember(dest => dest.FloorName, mapper => mapper.MapFrom(src => src.Room.Floor.Name))
            .ForMember(dest => dest.ManufacturerName, mapper => mapper.MapFrom(src => src.Model.Manufacturer.Name))
            .ForMember(dest => dest.ProductCatalogModelCode, mapper => mapper.MapFrom(src => src.Model.Code));

        CreateMap<AssetDetails, NetworkDeviceDetails>()
            .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.DeviceID));
        CreateMap<AssetEntity, NetworkDeviceDetails>()
            .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.DeviceID))
            .ForMember(dest => dest.LifeCycleStateName, mapper => mapper.MapFrom(src => src.LifeCycleState.Name));


        CreateMap<NetworkDeviceData, NetworkDevice>();
        CreateMap<NetworkDeviceData, AssetData>();
        CreateMap<NetworkDevice, AssetData>()
            .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.IMObjID))
            .ForMember(dest => dest.DeviceID, mapper => mapper.MapFrom(src => src.ID));

    }
}
