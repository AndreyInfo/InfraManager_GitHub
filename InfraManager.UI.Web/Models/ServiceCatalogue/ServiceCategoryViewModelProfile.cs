using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;
using InfraManager.Web.BLL.Helpers;

namespace InfraManager.UI.Web.Models.ServiceCatalogue
{
    public class ServiceCategoryViewModelProfile : Profile
    {
        public ServiceCategoryViewModelProfile()
        {
            CreateMap<ServiceCategoryDetailsModel, ServiceCategoryViewModel>()
                .ForMember(
                    viewModel => viewModel.ImageSource,
                    mapper => mapper.MapFrom(
                        model => model.HasImage
                            ? ImageHelper.GetValidPath(ImageHelper.GetServiceCategoryIconSource(model.ID))
                            : null));
        }
    }
}
