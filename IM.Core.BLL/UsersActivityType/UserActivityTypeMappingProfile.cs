using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.UsersActivityType;

public class UserActivityTypeMappingProfile : Profile
{
    public UserActivityTypeMappingProfile()
    {
        CreateMap<UserActivityType, UserActivityTypeDetails>();

        CreateMap<UserActivityTypeReference, UserActivityTypeReferenceDetails>();
    }
}