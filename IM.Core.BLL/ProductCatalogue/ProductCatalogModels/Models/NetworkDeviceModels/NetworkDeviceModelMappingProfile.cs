using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.NetworkDeviceModels;

internal class NetworkDeviceModelMappingProfile : Profile
{
    public NetworkDeviceModelMappingProfile()
    {
        CreateMap<NetworkDeviceModel, ProductModelDetails>()
            .ForMember(dst => dst.ID,
                mapper => mapper.MapFrom(
                    src => src.IMObjID));
    }
}