using AutoMapper;
using IM.Core.Import.BLL.Interface.Configurations.View;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager.DAL.Import;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Debug;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;
using InfraManager.DAL.ProductCatalogue.Import;
using InfraManager;

namespace IM.Core.Import.BLL.Import
{
    internal class ImportTasksProfile : Profile
    {
        public ImportTasksProfile()
        {
            CreateMap<User, UserDebugData>();
            CreateMap<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfiguration>();
            CreateMap<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationOutputDetails>();
            CreateMap<ProductCatalogImportSetting, ProductCatalogImportSettingOutputDetails>();

            CreateMap<UISetting, ImportTasksDetails>();
            CreateMap<UISetting, ImportMainTabDetails>();
            CreateMap<ImportMainTabDetails, UISetting>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ForMember(x => x.ID, opt => opt.Ignore());
            CreateMap<ImportMainTabModel, UISetting>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ForMember(x => x.ID, opt => opt.Ignore());
            CreateMap<UISetting, AdditionalTabDetails>();
            CreateMap<AdditionalTabData, UISetting>();
            
            CreateMap<ProductCatalogImportCSVConfigurationConcordance, ScriptDataDetails<ConcordanceObjectType>>()
                .ForMember(x => x.FieldName, x => x.MapFrom(x => x.Field))
                .ForMember(x => x.Script, x => x.MapFrom(y => y.Expression));
            CreateMap<ProductCatalogImportSetting, CsvOptions>()
                .ForMember(x => x.Delimeter, x => x.MapFrom(x => x.ProductCatalogImportCSVConfiguration.Delimeter))
                .ForMember(x => x.Path, x => x.MapFrom(y => y.Path));

        }
    }
}
