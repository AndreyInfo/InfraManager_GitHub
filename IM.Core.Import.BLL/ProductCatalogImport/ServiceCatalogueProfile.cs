using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.ServiceCatalogue;
using InfraManager.Core.Helpers;
using InfraManager.DAL.Import.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.ServiceBase.ImportService;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ServiceCatalogue;


namespace IM.Core.Import.BLL.Import.Importer
{
    public class ServiceCatalogueProfile : Profile
    {
        public ServiceCatalogueProfile()
        {
            CreateMap<ServiceCatalogueImportSettingData, ServiceCatalogueImportSetting>();
            CreateMap<ServiceCatalogueImportSetting, ServiceCatalogueImportSettingDetails>();
            CreateMap<ServiceCatalogueImportSettingDetails, ServiceCatalogueImportSettingData>();
            CreateMap<ServiceCatalogueImportCSVConfiguration, ServiceCatalogueImportCSVConfigurationDetails>();
            CreateMap<ServiceCatalogueImportCSVConfigurationData, ServiceCatalogueImportCSVConfiguration>();
            CreateMap<SCImportDetail, Service>()
                .ForMember(x => x.Name, x => x.MapFrom(x => x.Service_Name))
                .ForMember(x => x.State, x => x.MapFrom(x => TypeHelper.GetEnumByAnotherEnumWithName<CatalogItemState>(typeof(ServiceState), x.State)))
                .ForMember(x => x.ExternalID, x => x.MapFrom(x => x.ExternalIdentifier));
            CreateMap<ServiceData, Service>();
        }
    }
}
