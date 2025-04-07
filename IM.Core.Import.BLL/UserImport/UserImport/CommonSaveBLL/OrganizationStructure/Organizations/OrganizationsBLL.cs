using AutoMapper;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Organizations;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Logging;
using IronPython.Modules;
using IM.Core.Import.BLL.Comparers;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;

namespace IM.Core.Import.BLL.OrganizationStructure.Organizations;

public class OrganizationsBLL : IOrganizationsBLL, ISelfRegisteredService<IOrganizationsBLL>
{
    private readonly IRepository<Organization> _repository;
    private readonly IUnitOfWork _saveChanges;
    private readonly ILocalLogger<OrganizationsBLL> _logger;
    private readonly IOrganizationsAddRangeQuery _organizationsAddRangeQuery;

    private readonly IBaseImportMapper<OrganizationDetails, Organization> _mapper;
    public OrganizationsBLL(
        IRepository<Organization> repository,
        IOrganizationsAddRangeQuery organizationsAddRangeQuery,
        IUnitOfWork saveChanges,
        ILocalLogger<OrganizationsBLL> logger,
        IBaseImportMapper<OrganizationDetails, Organization> mapper)
    {
        _repository = repository;
        _organizationsAddRangeQuery = organizationsAddRangeQuery;
        _saveChanges = saveChanges;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<int> CreateOrganizationsAsync(IEnumerable<Organization> organizations,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _organizationsAddRangeQuery.ExecuteAsync(organizations, cancellationToken);

            await _saveChanges.SaveAsync(cancellationToken);
            return organizations.Count();
        }
        catch (Exception e)
        {
            _logger.Information($"ERR Ошибка создания организаций");
            _logger.Error(e, $"Error Create Organizations");
            throw;
        }
    }

    public void EnrichOrganizationForCreate(Organization organization)
    {
        organization.ID = Guid.NewGuid();
    }
    public async Task<int> UpdateOrganizationsAsync(Dictionary<OrganizationDetails, Organization> updateOrganizations,
        ImportData<OrganizationDetails, Organization> importData,
        CancellationToken cancellationToken = default)
    {
        var count = 0;
        foreach (var organization in updateOrganizations)
        {
            try
            {
                _mapper.Map(importData, new[]{(organization.Key, organization.Value)});
                await _saveChanges.SaveAsync(cancellationToken);
                count++;
            }
            catch (Exception e)
            {
                _logger.Information($"Ошибка обновления организации с ид {importData.DetailsKey(organization.Key)}");
                _logger.Error("Ошибка обновления организаций", string.Empty, e);
            }
        }
        return count;
    }

    public async Task<Organization> GetOrganizationByIDAsync(Guid id, CancellationToken cancellationToken = default)
                => await _repository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

    public async Task<Organization?> GetOrganizationByIDOrNameAsync(OrganizationDetails organization,
        CancellationToken cancellationToken = default)
        => await _repository.FirstOrDefaultAsync(x => x.ExternalId == organization.ExternalId || x.Name == organization.Name, cancellationToken);
    public async Task<IEnumerable<Organization>> GetOrganizationsByExternalIDAsync(List<OrganizationDetails> organizations, CancellationToken cancellationToken = default)
    {
        var organizationsExternalIDs = organizations.Select(x => x.ExternalId);
        return await _repository.ToArrayAsync(x => organizationsExternalIDs.Contains(x.ExternalId), cancellationToken);
    }
    public async Task<IEnumerable<Organization>> GetOrganizationsByNameAsync(List<OrganizationDetails> organizations, CancellationToken cancellationToken = default)
    {
        var organizationsNames = organizations.Select(x => x.Name);
        return await _repository.ToArrayAsync(x => organizationsNames.Contains(x.Name), cancellationToken);
    }
    public async Task<IEnumerable<Organization>> GetOrganizationsByIDOrNameAsync(List<OrganizationDetails> organizations, CancellationToken cancellationToken = default)
    {
        List<Organization> resultOrganizations = new List<Organization>();
        var organizationsByExternalID = await GetOrganizationsByExternalIDAsync(organizations, cancellationToken);
        var organizationsByName = await GetOrganizationsByNameAsync(organizations, cancellationToken);
        resultOrganizations.AddRange(organizationsByExternalID);
        resultOrganizations.AddRange(organizationsByName);

        return (resultOrganizations.Count()>1)
            ?resultOrganizations.Distinct(new OrganizationComparer()).ToList()
            :resultOrganizations;
    }

}