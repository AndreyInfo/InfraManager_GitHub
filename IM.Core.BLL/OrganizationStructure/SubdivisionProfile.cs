using AutoMapper;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    public class SubdivisionProfile : Profile
    {
        public SubdivisionProfile()
        {
            CreateMap<Subdivision, OrganizationStructureNodeModelDetails>()
                .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.Division))
                .ForMember(x => x.ObjectID, x => x.MapFrom(y => y.ID));
            
            CreateMap<SubdivisionDetails, OrganizationStructureNodeModelDetails>()
                .ForMember(x => x.ClassID, x => x.MapFrom(y => ObjectClass.Division))
                .ForMember(x => x.ObjectID, x => x.MapFrom(y => y.ID));


            CreateMap<Subdivision, SubdivisionDetails>()
                .ForMember(dst => dst.Path, m => m.MapFrom(scr => scr.FullName))
                .ReverseMap();
            
            
            CreateMap<SubdivisionData, Subdivision>();
        }
    }
}
