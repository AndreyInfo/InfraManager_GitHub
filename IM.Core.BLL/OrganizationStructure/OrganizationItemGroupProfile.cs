using AutoMapper;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    public class OrganizationItemGroupProfile : Profile
    {
        public OrganizationItemGroupProfile()
        {
            CreateMap<OrganizationItemGroup, OrganizationItemGroupData>();
        }
    }
}
