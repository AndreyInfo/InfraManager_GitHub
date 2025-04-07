using AutoMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue
{
    internal class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, ServiceDetails>();
        }
    }
}
