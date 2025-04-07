using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.AdapterTypes;

internal class AdapterModelMappingProfile : Profile
{
    public AdapterModelMappingProfile()
    {
        CreateMap<AdapterType, ProductModelDetails>()
            .ForMember(dst => dst.ID,
                mapper => mapper.MapFrom(
                    src => src.IMObjID));
    }
}