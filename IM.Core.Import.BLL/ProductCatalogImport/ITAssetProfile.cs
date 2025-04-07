using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.ITAsset;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Import.ITAsset;
using InfraManager.DAL.ITAsset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.ServiceBase.ImportService.WebAPIModels.ITAsset;
using AdapterEntity = InfraManager.DAL.Asset.Adapter;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace IM.Core.Import.BLL.Import.Importer;
public class ITAssetProfile : Profile
{
    public ITAssetProfile()
    {
        CreateMap<ITAssetImportSettingData, ITAssetImportSetting>();
        CreateMap<ITAssetImportSetting, ITAssetImportSettingDetails>();
        CreateMap<ITAssetImportSettingDetails, ITAssetImportSettingData>();
        CreateMap<ITAssetImportCSVConfiguration, ITAssetImportCSVConfigurationDetails>();
        CreateMap<ITAssetImportCSVConfigurationData, ITAssetImportCSVConfiguration>();
        CreateMap<ITAssetImportDetails, AssetEntity>();
        CreateMap<ITAssetImportDetails, ITAssetImportParsedDetails>();
        CreateMap<ITAssetImportParsedDetails, ITAssetUndisposed>();

        CreateMap<ITAssetImportParsedDetails, AssetEntity>()
           .ForMember(dst => dst.ID, m => m.Ignore());

        CreateMap<ITAssetImportParsedDetails, AdapterEntity>()
           .ForMember(dst => dst.ID, m => m.Ignore())
           .ForMember(dst => dst.ExternalID, m => m.Ignore())
           .ForMember(dst => dst.SerialNumber, m => m.Ignore())
           .ForMember(dst => dst.Code, m => m.Ignore())
           .ForMember(dst => dst.Identifier, m => m.MapFrom(src => "0"));

        CreateMap<ITAssetImportParsedDetails, Peripheral>()
           .ForMember(dst => dst.ID, m => m.Ignore());

        CreateMap<ITAssetImportParsedDetails, NetworkDevice>()
           .ForMember(dst => dst.ID, m => m.Ignore());

        CreateMap<ITAssetImportParsedDetails, TerminalDevice>()
           .ForMember(dst => dst.ID, m => m.Ignore());

        CreateMap<ProductCatalogType, ProductCatalogTypeDetails>()
           .ForMember(dst => dst.ClassName, m => m.MapFrom(src => src.ProductCatalogTemplate.Name));

        CreateMap<string?, decimal?>().ConstructUsing(src => string.IsNullOrEmpty(src) ? null : Convert.ToDecimal(src));
        CreateMap<string?, DateTime?>().ConstructUsing(src => string.IsNullOrEmpty(src) ? null : Convert.ToDateTime(src));
    }
}
