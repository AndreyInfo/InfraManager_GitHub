using AutoMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue
{
    internal class ServiceCategoryProfile : Profile
    {
        public ServiceCategoryProfile()
        {
            CreateMap<ServiceCategory, ServiceCategoryDetails>();
        }
    }
}
