using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.DAL;

namespace InfraManager.BLL.ServiceDesk.Mapping
{
    public class GroupQueueUserProfile : Profile
    {
        public GroupQueueUserProfile()
        {
            CreateMap<User, GroupQueueUserDetails>()
                .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.ID))
                .ForMember(dst => dst.RoleName, m => m.MapFrom(scr => scr.Position.Name ?? string.Empty))
                .ForMember(dst => dst.DepartamentName, m => m.MapFrom(scr => scr.Subdivision.Name))
                .ForMember(dst => dst.UID, m => m.MapFrom(scr => scr.IMObjID))
                .ReverseMap()
                .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.ID));
        }
    }
}
