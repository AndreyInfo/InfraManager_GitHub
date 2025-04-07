using AutoMapper;
using Inframanager.DAL.ProductCatalogue.Import;

namespace InfraManager.BLL.ProductCatalogue.Import;

internal class ProductCatalogImportCSVConfigurationConcordanceProfile : Profile
{
    public ProductCatalogImportCSVConfigurationConcordanceProfile()
    {
        CreateMap<ProductCatalogImportCSVConfigurationConcordance,
                ProductCatalogImportCSVConfigurationConcordanceOutputDetails>()
            ;

        CreateMap<ProductCatalogImportOutputCSVConfigurationConcordanceDetails,
                ProductCatalogImportCSVConfigurationConcordance>()
            .ForMember(x => x.Field, x => x.MapFrom(y => y.Field))
            .ForMember(x=>x.ID,x=>x.MapFrom(y=>y.ID));

        CreateMap<ProductCatalogImportCSVConfigurationConcordanceDetails,
                ProductCatalogImportCSVConfigurationConcordance>()
            .ForMember(x => x.Field, x => x.MapFrom(y => y.Field));
    }
}