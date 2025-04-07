using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class OrganizationImportEntityData : IImportEntityData<OrganizationDetails, Organization, OrganisationComparisonEnum>, ISelfRegisteredService<IImportEntityData<OrganizationDetails, Organization, OrganisationComparisonEnum>>
{
    private readonly IOrganizationsImportRepository _organizationsImportRepository;

    private readonly Dictionary<ObjectType, ImportKeyData<OrganizationDetails,Organization>> _uniqueDetailsChecks = new()
    {
        {ObjectType.OrganizationName, new ImportKeyData<OrganizationDetails,Organization>(new(){{x => new OrganizationNameKey(x.Name),x=>new OrganizationNameKey(x.Name)}}, nameof(Organization.Name))},
        {ObjectType.OrganizationExternalID, new ImportKeyData<OrganizationDetails,Organization>(new(){{x => new ExternalIDKey(x.ExternalId),x=>new ExternalIDKey(x.ExternalId)}}, nameof(Organization.ExternalId))},
    };
    public OrganizationImportEntityData(IOrganizationsImportRepository organizationsImportRepository)
    {
        _organizationsImportRepository = organizationsImportRepository;
    }

    public Func<ICollection<OrganizationDetails>, IAdditionalParametersForSelect, CancellationToken,
        Task<IEnumerable<Organization>>> GetComparerFunction(OrganisationComparisonEnum parameter, bool withRemoved)
    {
        return parameter switch
        {
            OrganisationComparisonEnum.Name => _organizationsImportRepository.FromNameAsync,
            OrganisationComparisonEnum.ExternalID => _organizationsImportRepository.FromExternalIDAsync,
            OrganisationComparisonEnum.NameOrExternalID => _organizationsImportRepository.FromOrganizationsByIDOrNameAsync,
            _ => throw new NotSupportedException()
        };
    }
    
    public Func<ICollection<OrganizationDetails>,IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<Organization>>> GetOrganizationGetterForUniqueKeys(ObjectType type)
    {
        return type switch
        {
            ObjectType.OrganizationName => _organizationsImportRepository.FromNameAsync,
            ObjectType.OrganizationExternalID => _organizationsImportRepository.FromExternalIDAsync,
        };
    }

    public async Task<IEnumerable<IDuplicateKeyData<OrganizationDetails, Organization>>> GetUniqueKeys(ObjectType flags, bool getRemoved, CancellationToken token)
    {
        return from uniqueCheck in _uniqueDetailsChecks 
            where flags.HasFlag(uniqueCheck.Key) || uniqueCheck.Key == ObjectType.OrganizationName
            select new DuplicateKeyData<OrganizationDetails,Organization>(uniqueCheck.Value, GetOrganizationGetterForUniqueKeys(uniqueCheck.Key));
    }
}