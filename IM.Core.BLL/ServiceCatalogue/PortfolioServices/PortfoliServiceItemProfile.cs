using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal sealed class PortfoliServiceItemProfile : Profile
{
    public PortfoliServiceItemProfile()
    {
        CreateMap<ServiceData, PortfoliServiceItemSaveModel>();

        CreateMap<ServiceItemData, PortfoliServiceItemSaveModel>();

        CreateMap<ServiceAttendanceData, PortfoliServiceItemSaveModel>();
    }
}
