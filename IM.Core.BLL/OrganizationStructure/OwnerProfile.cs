using AutoMapper;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<OwnerData, Owner>();
            CreateMap<Owner, OwnerDetails>();
        }
    }
}
