using AutoMapper;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.AccessManagement;

internal class AccessElementsDetailsProfile : Profile
{
    public AccessElementsDetailsProfile()
    {
        CreateMap<ObjectAccess, AccessElementsDetails>()
            .ForMember(c => c.AccessType, m => m.MapFrom(scr => scr.Type))
            .ForMember(dst => dst.IsSelectFull, m => m.MapFrom(scr => scr.Propagate))
            .ForMember(dst => dst.IsSelectPart, m => m.MapFrom(scr => !scr.Propagate))
            .ForMember(dst => dst.ObjectClassID, m => m.MapFrom(scr => scr.ClassID))
            .ReverseMap()
            .ForMember(c => c.Type, m => m.MapFrom(scr => scr.AccessType))
            .ForMember(dst => dst.Propagate, m => m.MapFrom(scr => scr.IsSelectFull))
            .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => scr.ObjectClassID))
            ;

        CreateMap<AccessElementsData, ObjectAccess>()
            .ForMember(c => c.Type, m => m.MapFrom(scr => scr.AccessType))
            .ForMember(dst => dst.Propagate, m => m.MapFrom(scr => scr.IsSelectFull))
            .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => scr.ObjectClassID));
    }
}
