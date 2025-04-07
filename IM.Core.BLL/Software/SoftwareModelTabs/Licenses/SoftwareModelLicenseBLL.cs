using Inframanager.BLL;
using InfraManager.DAL.Software;
using Inframanager;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.BLL.Location;
using System.Linq;
using InfraManager.DAL.Software.Licenses;
using InfraManager.BLL.Settings;
using InfraManager.BLL.ColumnMapper;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Licenses;
public class SoftwareModelLicenseBLL : ISoftwareModelLicenseBLL, ISelfRegisteredService<ISoftwareModelLicenseBLL>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<SoftwareModelLicenseBLL> _logger;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly IRepository<SoftwareLicence> _softwareLicensesRepository;
    private readonly IRepository<SoftwareLicenceReference> _softwareLicenseReferencesRepository;
    private readonly ILocationBLL _locationBLL;
    private readonly ISoftwareModelLicenseQuery _softwareModelLicenseQuery;
    private readonly IOrderedColumnQueryBuilder<SoftwareLicence, SoftwareModelLicenseForTable> _orderedColumnQueryBuilder;


    public SoftwareModelLicenseBLL(
        IMapper mapper,
        ICurrentUser currentUser,
        ILogger<SoftwareModelLicenseBLL> logger,
        IValidatePermissions<SoftwareModel> validatePermissions,
        IRepository<SoftwareLicence> softwareLicensesRepository,
        IRepository<SoftwareLicenceReference> softwareLicenceReferencesRepository,
        ILocationBLL locationBLL,
        IGuidePaggingFacade<SoftwareLicence, SoftwareModelLicenseForTable> guidePaggingFacade,
        ISoftwareModelLicenseQuery softwareModelLicenseQuery,
        IOrderedColumnQueryBuilder<SoftwareLicence, SoftwareModelLicenseForTable> orderedColumnQueryBuilder
        )
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _softwareLicensesRepository = softwareLicensesRepository;
        _softwareLicenseReferencesRepository = softwareLicenceReferencesRepository;
        _locationBLL = locationBLL;
        _softwareModelLicenseQuery = softwareModelLicenseQuery;
        _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
    }

    public async Task<SoftwareModelLicenseListItemDetails[]> GetLicensesForSoftwareModelAsync(SoftwareModelTabFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _softwareLicensesRepository.DisableTrackingForQuery().Query().Where(x => x.SoftwareModelID == filter.ID);

        var orderedQuery = await _orderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);

        var softwareModelLicenseItems = await _softwareModelLicenseQuery.ExecuteAsync(_mapper.Map<PaggingFilter>(filter), orderedQuery, cancellationToken);

        foreach(var license in softwareModelLicenseItems)
        {
            await SetLocationForLicenseAsync(license, cancellationToken);
            await SetInUseReferenceCountForLicenseAsync(license, cancellationToken);
            SetBalanceForLicense(license);
            SetSoftwareLicenceTypeModelNameForLicense(license);
        }

        return _mapper.Map<SoftwareModelLicenseListItemDetails[]>(softwareModelLicenseItems);
    }

    // Поиск и установка местоположения
    private async Task SetLocationForLicenseAsync(SoftwareModelLicenseItem license, CancellationToken cancellationToken)
    {
        if (license.RoomIntID != 0)
        {
            var locationTree = await _locationBLL
                .GetBranchTreeAsync(ObjectClass.Room, (Guid)license.RoomIMObjID, cancellationToken);

            foreach (var location in locationTree)
            {
                switch (location.ClassID)
                {
                    case ObjectClass.Organizaton:
                        license.OrganizationName = location.Name;
                        break;
                    case ObjectClass.Building:
                        license.BuildingName = location.Name;
                        break;
                    case ObjectClass.Floor:
                        license.FloorName = location.Name;
                        break;
                    case ObjectClass.Room:
                        license.RoomName = location.Name;
                        break;
                    case ObjectClass.Owner:
                        break;
                    default:
                        throw new Exception($"Несуществующий тип местоположения: {location.Name}");
                }
            }
        }
    }

    // Вычисление и установка количества выданных прав
    private async Task SetInUseReferenceCountForLicenseAsync(SoftwareModelLicenseItem license, CancellationToken cancellationToken)
    {
        var softwareLicenceReferences = await _softwareLicenseReferencesRepository
            .ToArrayAsync(x => x.SoftwareLicenceId == license.ID, cancellationToken);

        if (!IsAvailableForReference((SoftwareLicenceSchemeEnum)license.SoftwareLicenceSchemeEnum))
        {
            license.InUseReferenceCount = 0;
        }
        else
        {
            license.InUseReferenceCount = softwareLicenceReferences is null ?
                0 :
                softwareLicenceReferences.Sum(x => x.SoftwareExecutionCount ?? 0);
        }
    }

    // Вычисление и установка баланса
    private void SetBalanceForLicense(SoftwareModelLicenseItem license)
    {
        if (!IsAvailableForReference((SoftwareLicenceSchemeEnum)license.SoftwareLicenceSchemeEnum))
        {
            license.Balance = int.MaxValue;
        }
        else
        {
            license.Balance = license.Count.HasValue ? (license.Count.Value - license.InUseReferenceCount) : int.MaxValue;
        }
    }

    // Установка названия типа/модели
    private void SetSoftwareLicenceTypeModelNameForLicense(SoftwareModelLicenseItem license)
    {
        if (!string.IsNullOrWhiteSpace(license.Name) && !string.IsNullOrWhiteSpace(license.ProductCatalogTypeName))
            license.SoftwareLicenceTypeModelName = string.Format("{0} / {1}", license.ProductCatalogTypeName, license.Name);
        else if (!string.IsNullOrWhiteSpace(license.ProductCatalogTypeName))
            license.SoftwareLicenceTypeModelName = string.Format("{0}", license.ProductCatalogTypeName);
        else
            license.SoftwareLicenceTypeModelName = string.Empty;
    }
            
    private bool IsAvailableForReference(SoftwareLicenceSchemeEnum? licenceScheme)
    {
        if (licenceScheme is null)
            return false;
        switch (licenceScheme)
        {
            case SoftwareLicenceSchemeEnum.ComputerLicence:
            case SoftwareLicenceSchemeEnum.UserLicence:
            case SoftwareLicenceSchemeEnum.UserCALLicence:
            case SoftwareLicenceSchemeEnum.DeviceCALLicence:
            case SoftwareLicenceSchemeEnum.ProcessorsLicence:
                return true;
            case SoftwareLicenceSchemeEnum.ConcurentLicence:
            case SoftwareLicenceSchemeEnum.Freeware:
            case SoftwareLicenceSchemeEnum.Shareware:
            case SoftwareLicenceSchemeEnum.Demo:
            case SoftwareLicenceSchemeEnum.Site:
                return false;
            default:
                throw new Exception($"Несуществующий тип схемы лицензирования: {licenceScheme}");
        }
    }
}
