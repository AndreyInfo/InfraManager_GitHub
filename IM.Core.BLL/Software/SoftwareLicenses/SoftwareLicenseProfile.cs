using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Software.SoftwareModelTabs.Licenses;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Software;
using InfraManager.DAL.Software.Licenses;

namespace InfraManager.BLL.Software.SoftwareLicenses;

public class SoftwareLicenseProfile : Profile
{
    public SoftwareLicenseProfile()
    {
        CreateMap<SoftwareLicence, SoftwareLicenseDetails>();
        CreateMap<SoftwareModelLicenseItem, SoftwareModelLicenseListItemDetails>()
            .ForMember(dest => dest.LicenceTypeName, mapper => mapper.MapFrom<LocalizedEnumResolver<SoftwareModelLicenseItem, SoftwareModelLicenseListItemDetails, LicenceType>, LicenceType>(src => src.LicenceType));
    }
}
