using AutoMapper;
using InfraManager.BLL.Extensions;
using InfraManager.CrossPlatform.WebApi.Contracts.Assets;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Software.Installation;
using System;
using DTO = InfraManager.DAL.Software;
using InfraManager.ResourcesArea;
using InfraManager.DAL.Asset.Subclasses;

namespace InfraManager.BLL.Mapping
{
    public class SoftwareProfile : Profile
    {
        public SoftwareProfile()
        {
            CreateMap<DTO.SoftwareLicenceScheme, SoftwareLicenceScheme>()
                .ForMember(dst => dst.AllowInstallOnVM, m => m.MapFrom(
                      src => src.IsAllowInstallOnVm
                          ? (src.InstallationLimitPerVm == null ? SoftwareLicenceSchemeForVM.AllowedUnrestricted
                          : SoftwareLicenceSchemeForVM.AllowedWithRestriction)
                          : SoftwareLicenceSchemeForVM.NotAllowed
                      ))
                .ForMember(dst => dst.LicensingObjectTypeLabel, m => m.MapFrom(src => ((SoftwareLicenceSchemeObjectTypes)src.LicensingObjectType).GetDisplayLabel()))
                .ForMember(dst => dst.SchemeTypeLabel, m => m.MapFrom(src => ((SoftwareLicenceSchemeType)src.SchemeType).GetDisplayLabel()))
                ;

            CreateMap<SoftwareLicenceScheme, DTO.SoftwareLicenceScheme>()
                .ForMember(dst => dst.IsAllowInstallOnVm, m => m.MapFrom(src => src.AllowInstallOnVM != SoftwareLicenceSchemeForVM.NotAllowed))
                .ForMember(dst => dst.InstallationLimitPerVm, m => m.MapFrom(
                      src => src.AllowInstallOnVM == SoftwareLicenceSchemeForVM.AllowedWithRestriction ? src.InstallationLimitPerVM:(int?)null))
                .ForMember(dst=>dst.CreatedDate, m=>m.Ignore())
                .ForMember(dst=>dst.UpdatedDate, m=>m.Ignore())
                ;

            CreateMap<DTO.SoftwareLicenceScheme, SoftwareLicenceSchemeListItem>()
                .ForMember(dst => dst.LicensingObjectTypeLabel, m => m.MapFrom(src => ((SoftwareLicenceSchemeObjectTypes)src.LicensingObjectType).GetDisplayLabel()))
                .ForMember(dst => dst.SchemeTypeLabel, m => m.MapFrom(src => ((SoftwareLicenceSchemeType)src.SchemeType).GetDisplayLabel()))
                .ForMember(dst => dst.IsLocationRestrictionApplicable, m => m.MapFrom(src => src.CompatibilityTypeID == (byte)ECompatibilityLicenceSchema.Site))
                ;

            CreateMap<ViewSoftwareInstallation, SoftwareInstallationListItem>()
                .ForMember(dst => dst.CreateDate, m => m.MapFrom(src => src.UtcDateCreated.ToLocalTime()))
                .ForMember(dst => dst.DateLastSurvey, m => m.MapFrom(src => src.UtcDateLastDetected == null ? "": src.UtcDateLastDetected.Value.ToLocalTime().ToString()))                             
                .ForMember(dst => dst.Status, m => m.MapFrom(src => src.State))
                .ForMember(dst => dst.StatusName, m => m.MapFrom(p => p.State > 0 ? Resources.software_installation_status_archive : Resources.software_installation_status_active));
            
            CreateMap<DTO.SoftwareInstallation, SoftwareInstallationItem>()
                .ForMember(dst => dst.CreateDate, m => m.MapFrom(src => src.UtcDateCreated))
                .ForMember(dst => dst.DateLastSurvey, m => m.MapFrom(src => src.UtcDateLastDetected.ToString()))  // TODO:  откуда ???                 
                .ForMember(dst => dst.SoftwareModelName, m => m.MapFrom(src => src.SoftwareModel.Name))
                .ForMember(dst => dst.CommercialModelName, m => m.MapFrom(src => src.SoftwareModel.CommercialModel != null ? src.SoftwareModel.CommercialModel.Name : string.Empty))
                .ForMember(dst => dst.SoftwareInstallationName, m => m.MapFrom(src => string.IsNullOrEmpty(src.UniqueNumber) ? src.ID.ToString() : src.UniqueNumber));

            CreateMap<SoftwareInstallationItem, DTO.SoftwareInstallation>()
                .ForMember(dst => dst.UniqueNumber, m => m.MapFrom(src => string.IsNullOrEmpty(src.SoftwareInstallationName) ? src.ID.ToString() : src.SoftwareInstallationName))
                .ForMember(dst => dst.InstallDate, m => m.MapFrom(src => ConvertFromStringOrTicks(src.InstallDate)));

            CreateMap<DTO.SoftwareLicenceSchemeProcessorCoeff, SoftwareLicenceSchemeCoefficient>();

            CreateMap<SoftwareLicenceSchemeCoefficient, DTO.SoftwareLicenceSchemeProcessorCoeff>()
                .ForMember(dst => dst.LicenceSchemeId, m => m.UseDestinationValue());

            CreateMap<Tuple<AdapterType, Processor>, ProcessorModelModel>();

            CreateMap<DTO.SoftwareType, DTO.SoftwareTypeResponse> ();

            AllowNullCollections = true;
        }

        private DateTime? ConvertFromStringOrTicks(string installDate)
        {
            if (string.IsNullOrWhiteSpace(installDate))
                return null;
            if (DateTime.TryParse(installDate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out var dateTime))
                return dateTime;
            if (long.TryParse(installDate, out var ticks))
                return new DateTime(1970, 1, 1).AddMilliseconds(ticks);
            return null;
        }
    }
}
