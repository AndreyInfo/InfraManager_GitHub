using AutoMapper;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels.Models.PeripherialTypes;

internal class PeripheralModelMappingProfile : Profile
{
    public PeripheralModelMappingProfile()
    {
        CreateMap<PeripheralType, ProductModelDetails>()
            .ForMember(dst => dst.ID,
                mapper => mapper.MapFrom(
                    src => src.IMObjID));
    }
}