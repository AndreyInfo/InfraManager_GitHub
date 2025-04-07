using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.Web.BLL.SD.ServiceCatalogue;

namespace InfraManager.UI.Web.Models.ServiceCatalogue
{
    public class ServiceViewModelProfile : Profile
    {
        public ServiceViewModelProfile()
        {
            CreateMap<ServiceDetailsModel, ServiceViewModel>()
                .ForMember(
                    viewModel => viewModel.Note,
                    mapper => mapper.MapFrom(
                        model => ServiceCatalogueHelper.ConvertRtfToText(model.Note)));
        }
    }
}
