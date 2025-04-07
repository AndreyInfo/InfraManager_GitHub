using AutoMapper;
using InfraManager.BLL.Asset.LifeCycleCommands;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Asset.History;

namespace InfraManager.BLL.Asset.History;
internal sealed class AssetHistoryProfile : Profile
{
    public AssetHistoryProfile()
    {

        CreateMap<User, AssetHistory>()
            .ForMember(dest => dest.ID, mapper => mapper.Ignore())
            .ForMember(dest => dest.UserID, mapper => mapper.MapFrom(src => src.IMObjID))
            .ForMember(dest => dest.UserFullName, mapper => mapper.MapFrom(src => src.FullName));

        CreateMap<LifeCycleCommandResultItem, AssetHistoryBaseData>();
        CreateMap<AssetHistoryBaseData, AssetHistory>();

        CreateMap<AssetHistoryBaseData, AssetHistoryToRepair>();
        CreateMap<AssetHistoryBaseData, AssetHistoryFromRepair>();
        CreateMap<AssetHistoryBaseData, AssetHistoryMove>()
            .ForMember(dest => dest.NewLocationID, mapper => mapper.MapFrom(src => src.LocationID))
            .ForMember(dest => dest.NewLocationClassID, mapper => mapper.MapFrom(src => src.LocationClassID));
        CreateMap<AssetHistoryBaseData, AssetHistoryRegistration>()
            .ForMember(dest => dest.NewLocationID, mapper => mapper.MapFrom(src => src.LocationID))
            .ForMember(dest => dest.NewLocationClassID, mapper => mapper.MapFrom(src => src.LocationClassID));

        CreateMap<LifeCycleCommandBaseData, AssetHistoryBaseData>()
            .ForMember(dest => dest.LocationID, mapper => mapper.MapFrom(src => src.RoomID ?? src.NetworkDeviceID ?? src.RackID))
            .IgnoreNulls();
    }
}
