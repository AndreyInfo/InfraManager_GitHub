using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

public class MaterialConsumptionRateProfile : Profile
{
    public MaterialConsumptionRateProfile()
    {
        CreateMap<MaterialConsumptionRateInputDetails, MaterialConsumptionRate>();
        CreateMap<MaterialConsumptionRate, MaterialConsumptionRateOutputDetails>()
            .ForMember(x => x.MaterialModelName, x => x.MapFrom(x => x.Model.Name));
    }
}