using IM.Core.Import.BLL.Comparers;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class OrganizationsImportRepository : IOrganizationsImportRepository, ISelfRegisteredService<IOrganizationsImportRepository>
{
    private readonly IRepository<Organization> _repository;

    public OrganizationsImportRepository(IRepository<Organization> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Organization>> FromNameAsync(ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default)
    {
        var organizationsNames = organizations.Select(x => x.Name);
        return await _repository.ToArrayAsync(x => organizationsNames.Contains(x.Name), cancellationToken);
    }

    public async Task<IEnumerable<Organization>> FromExternalIDAsync(ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default)
    {
        var organizationsExternalIDs = organizations.Select(x => x.ExternalId);
        return await _repository.ToArrayAsync(x => organizationsExternalIDs.Contains(x.ExternalId),
            cancellationToken);
    }

    public async Task<IEnumerable<Organization>> FromOrganizationsByIDOrNameAsync(
        ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect additionalParametersForSelect, CancellationToken cancellationToken = default)
    {
        List<Organization> resultOrganizations = new List<Organization>();
        var organizationsByExternalID =
            await FromExternalIDAsync(organizations, additionalParametersForSelect, cancellationToken);
        var organizationsByName =
            await FromNameAsync(organizations, additionalParametersForSelect, cancellationToken);
        resultOrganizations.AddRange(organizationsByExternalID);
        resultOrganizations.AddRange(organizationsByName);

        return (resultOrganizations.Count() > 1)
            ? resultOrganizations.Distinct(new OrganizationComparer()).ToList()
            : resultOrganizations;
    }
}