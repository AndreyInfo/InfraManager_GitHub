using AutoMapper;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.BLL.Owners;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.Mapping
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OrganizationStructureNodeModelDetails>()
                .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.Owner))
                .ForMember(x => x.ObjectID, x => x.MapFrom(y => y.IMObjID));
            CreateMap<Owners.OwnerDetails, OrganizationStructureNodeModelDetails>()
                .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.Owner))
                .ForMember(x => x.ObjectID, x => x.MapFrom(y => y.IMObjID));
            CreateMap<Owner, Owners.OwnerDetails>().ReverseMap();
            
        }
    }
}
