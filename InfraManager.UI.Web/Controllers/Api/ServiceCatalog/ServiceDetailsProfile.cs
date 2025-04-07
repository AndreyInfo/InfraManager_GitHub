using AutoMapper;
using InfraManager.BLL.ServiceCatalogue;
using InfraManager.WebApi.Contracts.Models.ServiceCatalogue;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog
{
    internal class ServiceDetailsProfile : Profile
    {
        public ServiceDetailsProfile()
        {
            CreateMap<ServiceDetails, ServiceDetailsModel>()
                .ForMember(
                    model => model.CategoryUri,
                    mapper => mapper.MapFrom(
                        details => details.CategoryID.HasValue
                            ? $"/api/servicecategories/{details.CategoryID.Value}"
                            : null));
        }
    }
}
