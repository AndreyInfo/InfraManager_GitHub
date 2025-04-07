using AutoMapper;
using InfraManager.DAL.Settings.UserFields;

namespace InfraManager.BLL.Settings.UserFields;
internal sealed class UserFieldProfile : Profile
{
    public UserFieldProfile()
    {
        CreateMap<UserFieldData, AssetUserFieldName>()
            .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.Text));

        CreateMap<UserFieldData, CallUserFieldName>()
            .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.Text));

        CreateMap<UserFieldData, ProblemUserFieldName>()
            .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.Text));

        CreateMap<UserFieldData, WorkOrderUserFieldName>()
            .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.Text));
    }
}
