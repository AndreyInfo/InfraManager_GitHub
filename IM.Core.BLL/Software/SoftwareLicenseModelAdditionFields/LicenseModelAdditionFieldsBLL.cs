using AutoMapper;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.DAL;
using InfraManager.DAL.Software;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareLicenseModelAdditionFields;
public class LicenseModelAdditionFieldsBLL : ILicenseModelAdditionFieldsBLL, ISelfRegisteredService<ILicenseModelAdditionFieldsBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<SoftwareLicenceScheme> _softwareLicenceSchemesRepository;
    private readonly IRepository<LicenseModelAdditionFields> _licenseModelAdditionFieldsRepository;

    public LicenseModelAdditionFieldsBLL(
        IMapper mapper,
        IRepository<SoftwareLicenceScheme> softwareLicenceSchemesRepository, 
        IRepository<LicenseModelAdditionFields> licenseModelAdditionFieldsRepository)
    {
        _mapper = mapper;
        _softwareLicenceSchemesRepository = softwareLicenceSchemesRepository;
        _licenseModelAdditionFieldsRepository = licenseModelAdditionFieldsRepository;
    }

    public async Task SaveAdditionalFieldsForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken)
    {
        var licenseScheme = await _softwareLicenceSchemesRepository
            .FirstOrDefaultAsync(x => x.ID == data.LicenseSchemeID, cancellationToken);

        var additionFields = new LicenseModelAdditionFields
        {
            SoftwareModelID = softwareModelID,
            LanguageID = data.LanguageID ?? SoftwareModelLanguage.Undefined,
            LicenseControlID = licenseScheme?.CompatibilityTypeID
        };

        _licenseModelAdditionFieldsRepository.Insert(additionFields);
    }

    public async Task UpdateAdditionalFieldsForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken)
    {
        var licenseScheme = await _softwareLicenceSchemesRepository
            .FirstOrDefaultAsync(x => x.ID == data.LicenseSchemeID, cancellationToken);

        var oldAdditionFileds = await _licenseModelAdditionFieldsRepository
            .FirstOrDefaultAsync(x => x.SoftwareModelID == softwareModelID, cancellationToken);

        if (oldAdditionFileds is null)
        {
            await SaveAdditionalFieldsForSoftwareModelAsync(softwareModelID, data, cancellationToken);
        }
        else
        {
            var additionFieldsData = new LicenseModelAdditionFieldsData
            {
                SoftwareModelID = softwareModelID,
                LanguageID = data.LanguageID ?? oldAdditionFileds.LanguageID,
                LicenseControlID = licenseScheme is null ? oldAdditionFileds.LicenseControlID : licenseScheme.CompatibilityTypeID
            };

            _mapper.Map(additionFieldsData, oldAdditionFileds);
        }
    }
}
