using AutoMapper;
using InfraManager.BLL.ServiceCatalogue;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.Mapping
{
    public class OrganizationItemGroupProfile : Profile
    {
        public OrganizationItemGroupProfile()
        {
            CreateMap<OrganizationItemGroup, ConcludedDetails>()
                .ForMember(x => x.ClassID, x => x.MapFrom(x => x.ItemClassID))
                .ForMember(x => x.ObjectID, x => x.MapFrom(x => x.ItemID));
            

            CreateMap<ConcludedDetails, OrganizationItemGroup>()
                .ForMember(x => x.ItemClassID, x => x.MapFrom(x => x.ClassID))
                .ForMember(x => x.ItemID, x => x.MapFrom(x => x.ObjectID));
        }
    }
}
