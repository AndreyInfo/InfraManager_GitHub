using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.ServiceDesk;

internal class PerformerProfile : Profile
{
    public PerformerProfile()
    {
        CreateMap<User, PerformerDetails>()
            .ForMember(dst => dst.ClassID, m=> m.MapFrom(scr => ObjectClass.User))
            .ForMember(dst => dst.Name, m=> m.MapFrom(scr => scr.FullName))
            .ForMember(dst => dst.DepartamentName, m=> m.MapFrom(scr => scr.SubdivisionName))
            .ForMember(dst => dst.UID, m=> m.MapFrom(scr => scr.IMObjID))
            .ReverseMap();

        CreateMap<Group, PerformerDetails>()
            .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => ObjectClass.Group))
            .ForMember(dst => dst.ResponsibleName, m=> m.MapFrom(scr => scr.ResponsibleUser.FullName))
            .ForMember(dst => dst.UID, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(dst => dst.ID, m => m.Ignore())
            .ReverseMap();

        CreateMap<PerformerDetails, OrganizationItemGroup>()
            .ForMember(dst => dst.ItemClassID, m => m.MapFrom(scr => scr.ClassID))
            .ForMember(dst => dst.ItemID, m => m.MapFrom(scr => scr.UID))
            .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.ServiceUnitID));
    }
}
