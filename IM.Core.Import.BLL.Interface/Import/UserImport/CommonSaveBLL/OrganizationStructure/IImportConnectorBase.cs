using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.OrganizationStructure;

public interface IImportConnectorBase<TDetails,TEntity> where TEntity:ILockableForOsi
{
    Task<IDictionary<TDetails,TEntity>> GetModelsForCreateAsync(ImportData<TDetails, TEntity> data,
        IEnumerable<TDetails> models,
        IEnumerable<TDetails> details,
        ErrorStatistics<TDetails> errorsModel,
        IBaseImportMapper<TDetails, TEntity> importMapper, Action<TEntity> initModelFunForCreate,
        Func<ICollection<TDetails>, ErrorStatistics<TDetails>, CancellationToken, Task> additionalValidationFunc = null,
        CancellationToken cancellationToken = default);

    Task<UpdateImportModels<TDetails, TEntity>> GetModelsForUpdateAsync(IEnumerable<TEntity> modelsFromBase,
        ICollection<TDetails> modelSourceData,
        ErrorStatistics<TDetails> errorModels,
        ImportData<TDetails, TEntity> data,
        Action<KeyValuePair<TDetails, TEntity>> initModelFunc,
        Func<ImportData<TDetails, TEntity>, ICollection<ModelAndDetailsModel<TDetails, TEntity>>, ICollection<TDetails>,
            ICollection<TDetails>, ErrorStatistics<TDetails>, CancellationToken, Task> additionalValidateFunc,
        CancellationToken token);

    void CheckUpdateModelsForError(ICollection<TDetails> modelsForUpdate,
        ErrorStatistics<TDetails> errorModels,
        ImportData<TDetails, TEntity> data, Func<TDetails, ImportData<TDetails, TEntity>, bool> validateFunc,
        string message);

    void MoveUsersToError(ICollection<TDetails> importUsers, ErrorStatistics<TDetails> errorUsers,
        IEnumerable<TDetails> duplicateImportUsers, string message);

    Task CheckForDuplicatesAsync(ICollection<TDetails> importDetails,
        ErrorStatistics<TDetails> errorDetails,
        ImportData<TDetails, TEntity> importTools, IAdditionalParametersForSelect? additional,
        Func<IGrouping<IIsSet, TDetails>, bool>? predicate = null,
        CancellationToken token = default);

    void AfterCheckForErrors(ICollection<TDetails> importDetails, ErrorStatistics<TDetails> errorDetails, ImportData<TDetails, TEntity> importTools);
    void CheckBeforeCreate(IDictionary<TDetails,TEntity> entities, ErrorStatistics<TDetails> errorEntities,
        ImportData<TDetails, TEntity> importTools);
}