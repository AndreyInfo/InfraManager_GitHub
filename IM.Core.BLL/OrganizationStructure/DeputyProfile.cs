using AutoMapper;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure;

internal class DeputyProfile : Profile
{
    public DeputyProfile()
    {
        CreateMap<DeputyUser, DeputyUserDetails>()
            .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.IMObjID))
            .ReverseMap()
            .ForMember(dst => dst.IMObjID, opt => opt.MapFrom(src => src.ID))
            ;
    }
}