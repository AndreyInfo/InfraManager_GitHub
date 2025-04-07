using AutoMapper;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

public class PersonalLicenceProfile : Profile
{
    public PersonalLicenceProfile()
    {
        CreateMap<UserPersonalLicence, UserPersonalLicenceDetails>()
            .ForMember(x => x.LoginName, x => x.MapFrom(x => x.User.LoginName))
            .ForMember(x => x.FullName, x => x.MapFrom(x => x.User.FullName))
            .ForMember(x => x.SubdivisionFullName, x => x.MapFrom(x => x.User.SubdivisionName))
            .ForMember(x => x.Number, x => x.MapFrom(x => x.User.Number));
    }
}