using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    public class UserActivityTypeReferenceProfile : Profile
    {
        public UserActivityTypeReferenceProfile()
        {
            CreateMap<UserActivityTypeReferenceDetails, UserActivityTypeReference>()
                        .ForMember(x => x.ID, opt => opt.Ignore());

            CreateMap<UserActivityTypeReference, UserActivityTypeReferenceDetails>();

            CreateMap<UserActivityTypeReference, UserActivityTypeReference>();

            CreateMap<UserActivityTypeWithChildsDetails, UserActivityTypeReference>();

            CreateMap<UserActivityTypeReference, UserActivityTypeWithChildsDetails>();

            CreateMap<UserActivityTypeWithChildsDetails, UserActivityTypePathDetails>()
                                .ForMember(dst => dst.Path, m => m.MapFrom(scr => scr.Path))
                                .ForMember(dst => dst.Pathes, m => m.MapFrom(scr => scr.Types));

            CreateMap<UserActivityTypePathDetails, UserActivityTypeWithChildsDetails>();
        }
    }
}
