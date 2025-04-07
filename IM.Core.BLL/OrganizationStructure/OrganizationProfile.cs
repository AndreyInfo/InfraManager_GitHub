using AutoMapper;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure;

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, OrganizationStructureNodeModelDetails>()
            .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.Organizaton))
            .ForMember(x => x.ObjectID, x => x.MapFrom(y => y.ID));
        
        CreateMap<OrganizationDetails, OrganizationStructureNodeModelDetails>()
            .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.Organizaton))
            .ForMember(x => x.ObjectID, x => x.MapFrom(y => y.ID));


        CreateMap<Organization, OrganizationDetails>()
            .ReverseMap();

        CreateMap<OrganizationData, Organization>();
    }
}
