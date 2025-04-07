using IM.Core.Import.BLL.Comparers;
using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Organizations;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.OrganizationStructure;

public class OrganizationImportConnector : IEntityImportConnector<OrganizationDetails, Organization>,
    ISelfRegisteredService<IEntityImportConnector<OrganizationDetails, Organization>>
{
    private readonly IOrganizationsBLL _organizationsBLL;
    private readonly IBaseImportMapper<OrganizationDetails, Organization> _organizationImportMapper;
    private readonly IImportConnectorBase<OrganizationDetails, Organization> _organizationImportConnectorBase;
    private readonly ILocalLogger<OrganizationImportConnector> _logger;

    public OrganizationImportConnector(IOrganizationsBLL organizationsBLL,
        IBaseImportMapper<OrganizationDetails, Organization> organizationImportMapper,
        IImportConnectorBase<OrganizationDetails, Organization> organizationImportConnectorBase, 
        ILocalLogger<OrganizationImportConnector> logger)
    {
        _organizationsBLL = organizationsBLL;
        _organizationImportMapper = organizationImportMapper;
        _organizationImportConnectorBase = organizationImportConnectorBase;
        _logger = logger;
    }

    public async Task SaveOrUpdateEntitiesAsync(ICollection<OrganizationDetails> organizations,
        IAdditionalParametersForSelect? additional, ImportData<OrganizationDetails, Organization> importData,
        ErrorStatistics<OrganizationDetails> errorEntries,
        CancellationToken cancellationToken = default)
    {
        _organizationImportConnectorBase.AfterCheckForErrors(organizations,errorEntries,importData);
        
        var additionalParameters = new OrganizationAdditionalParametersForSelect();
        var organizationsFromBase =
            (await importData.Func(organizations, additionalParameters, cancellationToken)).ToList();
        
        var updateOrganizations = await _organizationImportConnectorBase.GetModelsForUpdateAsync(organizationsFromBase,
            organizations,
            errorEntries,
            importData,
            null,
            null,
            cancellationToken);

        void InitOrganizationForCreate(Organization organization)
        {
            _organizationsBLL.EnrichOrganizationForCreate(organization);
        }

        var createOrganizations = await _organizationImportConnectorBase.GetModelsForCreateAsync(importData, organizations,
            updateOrganizations.FoundModels, errorEntries, _organizationImportMapper, InitOrganizationForCreate,
            null, cancellationToken);

        if (updateOrganizations.ModelsForUpdate.Count > 0)
        {
            var updateOrganizationsCount = await _organizationsBLL.UpdateOrganizationsAsync(updateOrganizations.ModelsForUpdate, importData,
                cancellationToken);
            errorEntries.UpdateCount +=
                updateOrganizationsCount;
            errorEntries.UpdatedErrors += updateOrganizations.ModelsForUpdate.Count - updateOrganizationsCount;
        }

        if (createOrganizations.Count > 0)
        {
            _organizationImportConnectorBase.CheckBeforeCreate(createOrganizations,errorEntries, importData);
            var createdOrganizationsCount = await _organizationsBLL.CreateOrganizationsAsync(createOrganizations.Values, cancellationToken);
            errorEntries.CreateCount += createdOrganizationsCount;
            errorEntries.CreatedErrors += createOrganizations.Count - createdOrganizationsCount;
        }

    }

    public void LogStatistics(ErrorStatistics<OrganizationDetails> errorEntities,
        ImportData<OrganizationDetails, Organization> importData)
    {
        _logger.Information($"Всего создано организаций: {errorEntities.CreateCount}");
        _logger.Information($"Всего обновлено организаций: {errorEntities.UpdateCount}");
        var errorEntitiesCount = errorEntities.ErrorDetails.Count;
        _logger.Information(
            $"Организаций с ошибками: {errorEntitiesCount + errorEntities.UpdatedErrors + errorEntities.CreatedErrors}. При обновлении: {errorEntities.UpdatedErrors}, при создании: {errorEntities.CreatedErrors}");
        SetDetailOrganizationErrorsToLog(errorEntities, importData);
    }

    private void SetDetailOrganizationErrorsToLog(ErrorStatistics<OrganizationDetails> errors,
        ImportData<OrganizationDetails, Organization> importData)
    {
        if (errors.ErrorDetails.Count > 0)
        {
            foreach (var error in errors.ErrorDetails.GetMessages(x=>LogHelper.ToOutputFormat(importData.DetailsKey(x)?.ToString())))
            {
                var errorName = LogHelper.ToOutputFormat(error);
                _logger.Information($"Ошибка при обновлении в организации: {errorName}");
            }
        }
    }
}