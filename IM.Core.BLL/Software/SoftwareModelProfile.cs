using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Software.SoftwareLicenses;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.BLL.Software.SoftwareModelTabs.Components;
using InfraManager.BLL.Software.SoftwareModelTabs.Installations;
using InfraManager.BLL.Software.SoftwareModelTabs.Licenses;
using InfraManager.BLL.Software.SoftwareModelTabs.PackageContents;
using InfraManager.BLL.Software.SoftwareModelTabs.Relations;
using InfraManager.BLL.Software.SoftwareModelTabs.Updates;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.Software;
using InfraManager.DAL.Software.PackageContents;
using System;

namespace InfraManager.BLL.Software;

public class SoftwareModelProfile : Profile
{
    public SoftwareModelProfile()
    {
        CreateMap<SoftwareModel, SoftwareModelListItemDetails>()
            .ForMember(dest => dest.TemplateID, mapper => mapper.MapFrom(src => src.Template))
            .ForMember(dest => dest.TemplateText, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModel, SoftwareModelListItemDetails, SoftwareModelTemplate>, SoftwareModelTemplate>(src => src.Template));

        CreateMap<SoftwareModelDetailsItem, SoftwareModelListItemDetails>()
            .ForMember(dest => dest.TemplateText, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModelDetailsItem, SoftwareModelListItemDetails, SoftwareModelTemplate>, SoftwareModelTemplate>(src => src.TemplateID));

        CreateMap<SoftwareModel, SoftwareModelDetailsBase>()
            .ForMember(dest => dest.TemplateID, mapper => mapper.MapFrom(src => src.Template));

        CreateMap<SoftwareModel, ISoftwareModelLanguaged>()
            .Include<SoftwareModel, SoftwareCommercialModelDetails>()
            .Include<SoftwareModel, SoftwarePackageDetails>()
            .Include<SoftwareModel, SoftwareTechnicalModelDetails>()
            .Include<SoftwareModel, SoftwareUpgradeDetails>()
            .ForMember(dest => dest.Language, mapper => mapper.MapFrom(src => src.LicenseModelAdditionFields ?? new()))
            .ForMember(dest => dest.LanguageID, mapper => mapper.MapFrom(src => src.LicenseModelAdditionFields ?? new()));

        CreateMap<SoftwareModel, SoftwareModelDetailsBase>()
            .Include<SoftwareModel, SoftwareCommercialModelDetails>()
            .Include<SoftwareModel, SoftwareUpdateDetails>()
            .Include<SoftwareModel, SoftwarePackageDetails>()
            .Include<SoftwareModel, SoftwareTechnicalModelDetails>()
            .Include<SoftwareModel, SoftwareComponentDetails>()
            .Include<SoftwareModel, SoftwareUpgradeDetails>()
            .ForMember(dest => dest.TemplateText, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModel, SoftwareModelDetailsBase, SoftwareModelTemplate>, SoftwareModelTemplate>(src => src.Template))
            .ForMember(dest => dest.TemplateID, mapper => mapper.MapFrom(src => src.Template));

        CreateMap<SoftwareModel, SoftwarePackageDetails>();
        CreateMap<SoftwareModel, SoftwareUpdateDetails>();
        CreateMap<SoftwareModel, SoftwareTechnicalModelDetails>();
        CreateMap<SoftwareModel, SoftwareComponentDetails>();
        CreateMap<SoftwareModel, SoftwareUpgradeDetails>();

        CreateMap<SoftwareModelPackageContentsItem, SoftwareModelPackageContentsListItemDetails>()
            .ForMember(dest => dest.LanguageName, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModelPackageContentsItem, SoftwareModelPackageContentsListItemDetails, SoftwareModelLanguage>, SoftwareModelLanguage>(src => src.LanguageID))
            .ForMember(dest => dest.TemplateName, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModelPackageContentsItem, SoftwareModelPackageContentsListItemDetails, SoftwareModelTemplate>, SoftwareModelTemplate>(src => src.Template));


        CreateMap<SoftwareModel, SoftwareCommercialModelDetails>()
            .ForMember(dest => dest.VersionRecognitionID, mapper => mapper.MapFrom(src => src.SoftwareModelRecognition.VersionRecognitionID))
            .ForMember(dest => dest.VersionRecognitionLvl, mapper => mapper.MapFrom(src => src.SoftwareModelRecognition.VersionRecognitionLvl))
            .ForMember(dest => dest.RedactionRecognition, mapper => mapper.MapFrom(src => src.SoftwareModelRecognition.RedactionRecognition));

        CreateMap<DAL.Software.SoftwareType, SoftwareTypeDetails>();
        CreateMap<SoftwareModel, SoftwareModelParentDetails>();
        CreateMap<SoftwareModel, CommercialModelDetails>();
        CreateMap<Manufacturer, ManufacturerDetails>();
        CreateMap<SoftwareLicence, SoftwareLicenseDetails>();
        CreateMap<SoftwareModelUsingType, SoftwareModelUsingTypeDetails>();
        CreateMap<LicenseModelAdditionFields, SoftwareModelLanguage>()
            .ConvertUsing((additionalFields, language) => language = additionalFields.LanguageID ?? SoftwareModelLanguage.Undefined);
        CreateMap<LicenseModelAdditionFields, SoftwareModelLanguageDetails>()
            .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.LanguageID ?? SoftwareModelLanguage.Undefined))
            .ForMember(dest => dest.Name, mapper => mapper.MapFrom<LocalizedEnumResolver<LicenseModelAdditionFields, SoftwareModelLanguageDetails, SoftwareModelLanguage>, SoftwareModelLanguage> (src => src.LanguageID ?? SoftwareModelLanguage.Undefined));
        CreateMap<SupportLineResponsible, GroupQueueDetails>();

        CreateMap<SoftwareModel, SoftwareModelUpdateListItemDetails>()
            .ForMember(dest => dest.ParentModelName, mapper => mapper.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentModelVersion, mapper => mapper.MapFrom(src => src.Version));

        CreateMap<SoftwareInstallation, SoftwareModelInstallationListItemDetails>()
            .ForMember(dest => dest.SoftwareModelName, mapper => mapper.MapFrom(src => src.SoftwareModel.Name))
            .ForMember(dest => dest.StateName, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareInstallation, SoftwareModelInstallationListItemDetails, SoftwareInstallationState>, SoftwareInstallationState>(src => (SoftwareInstallationState)src.State));

        CreateMap<SoftwareLicence, SoftwareModelLicenseListItemDetails>()
            .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.ID))
            .ForMember(dest => dest.SoftwareModelID, mapper => mapper.MapFrom(src => src.SoftwareModelID))
            .ForMember(dest => dest.SoftwareModelName, mapper => mapper.MapFrom(src => src.SoftwareModel.Name))
            .ForMember(dest => dest.SoftwareModelVersion, mapper => mapper.MapFrom(src => src.SoftwareModel.Version))
            .ForMember(dest => dest.ManufacturerID, mapper => mapper.MapFrom(src => src.SoftwareModel.ManufacturerID))
            .ForMember(dest => dest.SoftwareLicenseSchemeID, mapper => mapper.MapFrom(src => src.SoftwareLicenceScheme))
            .ForMember(dest => dest.SoftwareTypeName, mapper => mapper.MapFrom(src => src.SoftwareModel.SoftwareType.Name))
            .ForMember(dest => dest.ProductCatalogTypeName, mapper => mapper.MapFrom(src => src.ProductCatalogType.Name));

        CreateMap<SoftwareLicenceScheme, SoftwareLicenseSchemeDetails>();
        
        CreateMap<SoftwareModel, SoftwareModelComponentListItemDetails>();

        CreateMap<SoftwareModel, SoftwareModelRelatedListItemDetails>()
            .ForMember(dest => dest.ParentName, mapper => mapper.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentVersion, mapper => mapper.MapFrom(src => src.Version));

        CreateMap<SoftwareModel, SoftwareModelPackageContentsListItemDetails>()
            .ForMember(dest => dest.SoftwareTypeName, mapper => mapper.MapFrom(src => src.SoftwareType.Name))
            .ForMember(dest => dest.TemplateName, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModel, SoftwareModelPackageContentsListItemDetails, SoftwareModelTemplate>, SoftwareModelTemplate>(src => src.Template))
            .ForMember(dest => dest.LanguageName, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModel, SoftwareModelPackageContentsListItemDetails, SoftwareModelLanguage>, SoftwareModelLanguage>(
                src => src.LicenseModelAdditionFields != null
                ? src.LicenseModelAdditionFields.LanguageID ?? SoftwareModelLanguage.Undefined
                : SoftwareModelLanguage.Undefined))

            .ForMember(dest => dest.SoftwareModelUsingTypeName, mapper => mapper.MapFrom(src => src.SoftwareModelUsingType.Name));

        CreateMap<SoftwareModelData, SoftwareModel>()
            .ForMember(dest => dest.Note, mapper => mapper.MapFrom(src => src.Note ?? string.Empty))
            .ForMember(dest => dest.Version, mapper => mapper.MapFrom(src => src.Version ?? string.Empty))
            .ForMember(dest => dest.Code, mapper => mapper.MapFrom(src => src.Code ?? string.Empty))
            .ForMember(dest => dest.ExternalID, mapper => mapper.MapFrom(src => src.ExternalID ?? string.Empty));
    }
}
