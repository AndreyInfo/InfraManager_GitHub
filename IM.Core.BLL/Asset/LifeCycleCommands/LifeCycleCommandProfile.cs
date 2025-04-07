using AutoMapper;
using InfraManager.BLL.AutoMapper;

namespace InfraManager.BLL.Asset.LifeCycleCommands;
internal sealed class LifeCycleCommandProfile : Profile
{
    public LifeCycleCommandProfile()
    {
        CreateMap<LifeCycleCommandBaseData, AssetData>()
            .ForMember(dest => dest.UserID, mapper => mapper.MapFrom(src => src.MOL))
            .IgnoreNulls();
        CreateMap<LifeCycleCommandResultItem, LifeCycleCommandAlertDetails>();
    }
}
