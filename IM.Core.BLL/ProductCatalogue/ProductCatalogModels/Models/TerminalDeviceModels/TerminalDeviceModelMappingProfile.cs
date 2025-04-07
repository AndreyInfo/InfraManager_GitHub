using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.TerminalDeviceModels;

internal class TerminalDeviceModelMappingProfile : Profile
{
    public TerminalDeviceModelMappingProfile()
    {
        CreateMap<TerminalDeviceModel, ProductModelDetails>()
            .ForMember(dst => dst.ID,
                mapper => mapper.MapFrom(
                    src => src.IMObjID));
    }
}