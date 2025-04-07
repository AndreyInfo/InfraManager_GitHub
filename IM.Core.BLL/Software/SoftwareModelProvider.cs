using AutoMapper;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Software;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software;

public class SoftwareModelProvider<T> : ISoftwareModelProvider where T : SoftwareModelDetailsBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<SupportLineResponsible> _supportLineRepository;
    private readonly IRepository<SoftwareLicenceScheme> _softwareLicenceSchemeRepository;

    public SoftwareModelProvider(
        IMapper mapper,
        IRepository<SupportLineResponsible> supportLineRepository,
        IRepository<SoftwareLicenceScheme> softwareLicenceSchemeRepository
        )
    {
        _mapper = mapper;
        _supportLineRepository = supportLineRepository;
        _softwareLicenceSchemeRepository = softwareLicenceSchemeRepository;
    }

    public async Task<SoftwareModelDetailsBase> GetAsync(SoftwareModel model, SoftwareModelTemplate modelTemplate)
    {
        var details = _mapper.Map<T>(model);

        var groupQueue = await _supportLineRepository
            .FirstOrDefaultAsync(x => x.ObjectID == model.ID);

        if (groupQueue is not null)
        {
            ((ISoftwareModelGrouped)details).GroupQueueID = groupQueue.OrganizationItemID;
        }

        var licenseScheme = model.LicenseModelAdditionFields is null || !model.LicenseModelAdditionFields.LicenseControlID.HasValue
            ? await _softwareLicenceSchemeRepository
                .FirstOrDefaultAsync(x => x.ID == SoftwareLicenceScheme.EmptyID)
            : await _softwareLicenceSchemeRepository
                .FirstOrDefaultAsync(x => x.CompatibilityTypeID == model.LicenseModelAdditionFields.LicenseControlID);

        if (details is SoftwareModelLicensed licensed)
        {
            licensed.LicenseSchemeID = licenseScheme.ID;
            licensed.LicenseScheme = _mapper.Map<SoftwareLicenseSchemeDetails>(licenseScheme);
        }

        return details;
    }
}
