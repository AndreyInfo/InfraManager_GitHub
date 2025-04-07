using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager.Web.BLL.SD.ServiceCatalogue;

namespace InfraManager.UI.Web.Models.ServiceCatalogue
{
    public class ServiceItemViewModelProfile : Profile
    {
        public ServiceItemViewModelProfile()
        {
            CreateMap<ServiceItemDetailsModel, ServiceItemViewModel>()
                .ForMember(
                    viewModel => viewModel.Note,
                    mapper => mapper.MapFrom(
                        model => ServiceCatalogueHelper.ConvertRtfToText(model.Note)));
        }
    }
}
