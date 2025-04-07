using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings;

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