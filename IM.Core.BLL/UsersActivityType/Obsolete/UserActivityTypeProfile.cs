using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    public class UserActivityTypeProfile : Profile
    {
        public UserActivityTypeProfile()
        {
            CreateMap<UserActivityTypeDetails, UserActivityType>()
                                .ForMember(x => x.ID, opt => opt.Ignore());

            CreateMap<UserActivityType, UserActivityTypeDetails>();

            CreateMap<UserActivityType, UserActivityType>();

            CreateMap<UserActivityType, UserActivityTypeBaseDetails>()
                .ReverseMap();
        }
    }
}
