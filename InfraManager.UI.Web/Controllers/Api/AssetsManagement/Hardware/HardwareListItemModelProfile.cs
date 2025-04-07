using AutoMapper;
using InfraManager.BLL.AssetsManagement.Hardware;

namespace InfraManager.UI.Web.Controllers.Api.AssetsManagement.Hardware;

public class HardwareListItemModelProfile : Profile
{
    public HardwareListItemModelProfile()
    {
        CreateMap<HardwareListItem, HardwareListItemModel>();
    }
}