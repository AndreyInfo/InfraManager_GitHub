using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.OrganizationStructure.Subdivisions;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.OrganizationStructure;

public class SubdivisionImportConnector : IEntityImportConnector<ISubdivisionDetails, Subdivision>,
    ISelfRegisteredService<IEntityImportConnector<ISubdivisionDetails, Subdivision>>
{
    private readonly ISubdivisionsBLL _subdivisionsBLL;
    private readonly IImportConnectorBase<ISubdivisionDetails, Subdivision> _subdivisionImportConnectorBase;
    private readonly IBaseImportMapper<ISubdivisionDetails, Subdivision> _importMapper;
    private readonly ILocalLogger<IEntityImportConnector<ISubdivisionDetails, Subdivision>> _logger;
    private readonly IAdditionalParametersForSelect _additional;

    public SubdivisionImportConnector(ISubdivisionsBLL subdivisionsBLL,
        IImportConnectorBase<ISubdivisionDetails, Subdivision> subdivisionImportConnectorBase,
        IBaseImportMapper<ISubdivisionDetails, Subdivision> importMapper, IAdditionalParametersForSelect additional, 
        ILocalLogger<IEntityImportConnector<ISubdivisionDetails, Subdivision>> logger)
    {
        _subdivisionsBLL = subdivisionsBLL;
        _subdivisionImportConnectorBase = subdivisionImportConnectorBase;
        _importMapper = importMapper;
        _additional = additional;
        _logger = logger;
    }

    public async Task SaveOrUpdateEntitiesAsync(ICollection<ISubdivisionDetails> subdivisions,
        IAdditionalParametersForSelect? additional, ImportData<ISubdivisionDetails, Subdivision> importData,
        ErrorStatistics<ISubdivisionDetails> errorEntities,
        CancellationToken cancellationToken = default)
    {
        _subdivisionImportConnectorBase.AfterCheckForErrors(subdivisions,errorEntities,importData);
        var additionalParameters = new SubdivisionAdditionalParametersForSelect();
        var subdivisionsFromBase =
            (await importData.Func(subdivisions, additionalParameters, cancellationToken)).ToList();

        CheckForOrganizationsAndParentSubdivisions(subdivisions, errorEntities);
        //todo:subdivisionsForUpdate
        var updateSubdivisions = await _subdivisionImportConnectorBase.GetModelsForUpdateAsync(subdivisionsFromBase,
            subdivisions, errorEntities, importData, null, null, cancellationToken);

        Action<Subdivision> initSubdivisionForCreate = (subdivision) =>
        {
            _subdivisionsBLL.EnrichSubdivisionForCreate(subdivision);
        };
        //todo:subdivisionsForCreate
        var createSubdivisions = await _subdivisionImportConnectorBase.GetModelsForCreateAsync(importData, subdivisions,
            updateSubdivisions.FoundModels, errorEntities, _importMapper, initSubdivisionForCreate);

        if (updateSubdivisions.ModelsForUpdate.Count > 0)
        {
            var updateSubdivisionsCount = await _subdivisionsBLL.UpdateSubdivisionsAsync(updateSubdivisions.ModelsForUpdate, importData, cancellationToken);
            errorEntities.UpdateCount +=
                updateSubdivisionsCount;
            errorEntities.UpdatedErrors += updateSubdivisions.ModelsForUpdate.Count - updateSubdivisionsCount;
        }

        if (createSubdivisions.Count > 0)
        {
           _subdivisionImportConnectorBase.CheckBeforeCreate(createSubdivisions, errorEntities, importData);
           var createdSubdivisionsCount = await _subdivisionsBLL.CreateSubdivisionsAsync(createSubdivisions.Values, cancellationToken);
           errorEntities.CreateCount += createdSubdivisionsCount;
           errorEntities.CreatedErrors += createSubdivisions.Count - createdSubdivisionsCount;
        }

        
    }

    public void LogStatistics(ErrorStatistics<ISubdivisionDetails> errorEntities,
        ImportData<ISubdivisionDetails, Subdivision> importData)
    {
        _logger.Information($"Всего создано подразделений: {errorEntities.CreateCount}");
        _logger.Information($"Всего обновлено подразделений: {errorEntities.UpdateCount}");
        var errorCount = errorEntities.ErrorDetails.Count;
        _logger.Information(
            $"Подразделений с ошибками: {errorCount + errorEntities.UpdatedErrors + errorEntities.CreatedErrors}. При проверке: {errorCount}, при создании: {errorEntities.UpdatedErrors}, при обновлении: {errorEntities.CreatedErrors}.");
        SetDetailSubdivisionErrorsToLog(errorEntities, importData);
    }

    private void CheckForOrganizationsAndParentSubdivisions(ICollection<ISubdivisionDetails> subdivisions,
        ErrorStatistics<ISubdivisionDetails> errorEntities)
    {
        foreach (var subdivision in subdivisions.ToList())
        {
            if ((subdivision.OrganizationID.HasValue && subdivision.OrganizationID != Guid.Empty) ||
                (subdivision.IsNoSubdivision() && subdivision.SubdivisionID != Guid.Empty))
                continue;
            subdivisions.Remove(subdivision);
            errorEntities.ErrorDetails.Add(subdivision,"Не установлены радительская организация и/или родительское подразделение.");
            _logger.Information(
                $"Подразделение {subdivision.Name} не привязано к организации или родительскому подразделению. Не импортируется.");
        }
    }

    private void SetDetailSubdivisionErrorsToLog(ErrorStatistics<ISubdivisionDetails> errors,
        ImportData<ISubdivisionDetails, Subdivision> importData)
    {
        if (errors.ErrorDetails.Count > 0)
        {
            foreach (var error in errors.ErrorDetails.GetMessages(x=>LogHelper.ToOutputFormat(importData.DetailsKey(x)?.ToString())))
            {
                var errorName = LogHelper.ToOutputFormat(error);
                _logger.Information($"Ошибка при обновлении в подразделении с именем: {errorName}");
            }
        }
    }
}