using IM.Core.Import.BLL.Import;
using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Log;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.OrganizationStructure;

internal class ImportConnectorBase<TDetails,TEntity> : IImportConnectorBase<TDetails,TEntity> where TEntity:ILockableForOsi
{
    private readonly IAdditionalParametersForSelect _additional;
    private readonly ILocalLogger<ImportConnectorBase<TDetails, TEntity>> _logger;

    public ImportConnectorBase(IAdditionalParametersForSelect additional, 
        ILocalLogger<ImportConnectorBase<TDetails, TEntity>> logger)
    {
        _additional = additional;
        _logger = logger;
    }

   

    public async Task CheckForDuplicatesAsync(ICollection<TDetails> importDetails,
        ErrorStatistics<TDetails> errorDetails,
        ImportData<TDetails, TEntity> importTools, IAdditionalParametersForSelect? additional,
        Func<IGrouping<IIsSet, TDetails>, bool>? predicate = null,
        CancellationToken token = default)

    {
        
        //перевод в ошибку пользователей, соответствующих дубликатам в базе
        await CheckBaseDuplicates(importDetails,additional, errorDetails, importTools, true, token);

        //проверка на дубликаты пользователей из источника
        Func<IGrouping<object, TDetails>, bool> defaultPredicate = x => x.Count() > 1;
        //по сопоставлению
        RemoveDuplicates(importDetails, errorDetails, importTools.ModelKey, predicate ?? defaultPredicate, importTools);

        //по уникальным полям
        foreach (var insertCheckFields in importTools.DuplicateCheckFields)
        {
            RemoveDuplicates(importDetails, errorDetails, insertCheckFields, defaultPredicate, importTools);
        }

    }

    public void AfterCheckForErrors(ICollection<TDetails> importDetails, ErrorStatistics<TDetails> errorDetails, ImportData<TDetails, TEntity> importTools)
    {
        importDetails.ToList().Where(x => importTools.ValidateAfter(x)).ForEach(x =>
        {
            errorDetails.ErrorDetails.Add(x,"Недостаточно данных для импорта.");
            importDetails.Remove(x);
        });
    }

    public void CheckBeforeCreate(IDictionary<TDetails,TEntity> entities, ErrorStatistics<TDetails> errorEntities, ImportData<TDetails, TEntity> importTools)
    {
        entities.ToList().Where(x => importTools.ValidateBeforeCreate(x.Value)).ForEach(x =>
        {
            errorEntities.ErrorDetails.Add(x.Key,"Недостаточно данных для добавления");
            entities.Remove(x);
        });
    }

    public ICollection<TDetails> CheckDuplicatesFromModels(ICollection<TDetails> models, ICollection<TDetails> errors, ImportData<TDetails, TEntity> importData, Func<ICollection<TDetails>, ICollection<TDetails>> distinctFunc)
    {
        ICollection<TDetails> result = new List<TDetails>();
        var duplicates = models.ToLookup(importData.DetailsKey);
        foreach (var duplicate in duplicates)
        {
            TDetails model = default;
            var listDuplicates = duplicate.ToList();
            if (distinctFunc != null)
            { 
                var distinctResult = distinctFunc(listDuplicates);
                if (distinctResult.Any())
                    result.Add(distinctResult.First());
                foreach (var errorModel in distinctResult.Skip(1))
                {
                    errors.Add(errorModel);
                    models.Remove(errorModel);
                }
            }
            else
            {
                return models;
            }
        }

        return result.Count() > 0 ? result : models;
    }
    

    public async Task<IDictionary<TDetails,TEntity>> GetModelsForCreateAsync(ImportData<TDetails, TEntity> data,
        IEnumerable<TDetails> models,
        IEnumerable<TDetails> details,
        ErrorStatistics<TDetails> errorsModel,
        IBaseImportMapper<TDetails, TEntity> importMapper,
        Action<TEntity> initModelFunForCreate,
        Func<ICollection<TDetails>, ErrorStatistics<TDetails>, CancellationToken, Task> additionalValidationFunc = null,
        CancellationToken cancellationToken = default)
    {
        var forCreate = models.ToHashSet().Except(details).ToList();
        
        await CheckBaseDuplicates(forCreate, _additional, errorsModel, data, false,
            cancellationToken);
        
        if (additionalValidationFunc != null)
        {
            await additionalValidationFunc(forCreate, errorsModel, cancellationToken);
        }

        
        var result = forCreate.ToDictionary(x=>x,x=>importMapper.Map(data, new[]{x}).Single());

        return result;
    }

    public async Task<UpdateImportModels<TDetails, TEntity>> GetModelsForUpdateAsync(
        IEnumerable<TEntity> modelsFromBase,
        ICollection<TDetails> modelSourceData,
        ErrorStatistics<TDetails> errorModels,
        ImportData<TDetails, TEntity> data,
        Action<KeyValuePair<TDetails, TEntity>> initModelFunc,
        Func<ImportData<TDetails, TEntity>, ICollection<ModelAndDetailsModel<TDetails, TEntity>>, ICollection<TDetails>,
            ICollection<TDetails>, ErrorStatistics<TDetails>, CancellationToken, Task> additionalValidateFunc,
        CancellationToken token)
    {

        HashSet<ModelAndDetailsModel<TDetails, TEntity>> update = new();
        //поддержка сложных ключей
        foreach (var detailAndEntityKey in data.ModelKey.DetailsToEntityKeys)
        {
            var fromBase = modelsFromBase.Where(x=>detailAndEntityKey.Value(x).IsSet());
            var fromSource = modelSourceData.Where(x=>detailAndEntityKey.Key(x).IsSet());
            var current = fromBase
                .Join(fromSource, detailAndEntityKey.Value, detailAndEntityKey.Key, (x, y) => new ModelAndDetailsModel<TDetails,TEntity>(x, y))
                .Where(x=>!update.Contains(x)).ToList();
            update.UnionWith(current);
        }
        
        //найденные в базе
        var foundModels = update.Select(x => x.Details).ToList();
        
        var checkedForDuplicates = CheckForDuplicates(update, errorModels);

       
        //не обновлять заблокированные
        foreach (var pair in checkedForDuplicates.ToList())
        {
            if (pair.Model.IsLockedForOsi ?? false)
            {
                _logger.Information($"Сущность {data.DetailsKey(pair.Details)} заблокирована для обновления");
                checkedForDuplicates.Remove(pair);
            }
        }

        var filteredUpdate = checkedForDuplicates.AsEnumerable()
            .Where(x => data.IsUpdateNeeded(x.Details, x.Model) && foundModels.Contains(x.Details))
            .ToList();
        

        var detailsCollection = filteredUpdate.Select(x => x.Details).ToList();
        foreach (var insertCheckFields in data.DuplicateCheckFields)
        {
            var entity =
                await insertCheckFields.GetEntityByKey(detailsCollection, _additional, token);
            foreach (var detailsToEntityKey in insertCheckFields.DetailsToEntityKeys)
            {
                var entityKeys = entity.Select(detailsToEntityKey.Value).ToHashSet();
                var detailsKeys = filteredUpdate
                    .ToLookup(x => detailsToEntityKey.Key(x.Details));
                
                foreach (var detailsKey in detailsKeys)
                {
                    if ( entityKeys.Contains(detailsKey.Key))
                    {
                        foreach (var detail in detailsKey)
                        {
                            var updatingField = detailsToEntityKey.Value(detail.Model);
                            if (!detailsKey.Key.Equals(updatingField))
                            {
                                checkedForDuplicates.Remove(detail);
                                errorModels.ErrorDetails.Add(detail.Details, "");
                            }
                        }
                    }
                }
            }
        }
    
        
        if (additionalValidateFunc != null)
        {
            await additionalValidateFunc(data ,checkedForDuplicates, foundModels, modelSourceData, errorModels, token);
        }
        
        
        var result = checkedForDuplicates
            .ToDictionary(x => x.Details, x => x.Model);


        if (initModelFunc is not null)
        {
            foreach (var modelFromBase in result)
            {
                initModelFunc(modelFromBase);
            }
        }

        return new UpdateImportModels<TDetails, TEntity>(result, foundModels);
    }

    private static List<ModelAndDetailsModel<TDetails, TEntity>> CheckForDuplicates(
        IEnumerable<ModelAndDetailsModel<TDetails, TEntity>> filteredUpdate, ErrorStatistics<TDetails> errorModels)
    {
        var checkDuplicates = filteredUpdate.ToLookup(x => x.Details)
            .ToList();
        foreach (var duplicate in checkDuplicates.Where(x => x.Count() > 1).ToList())
        {
            errorModels.ErrorDetails.Add(duplicate.Key,"Найдено более одного объекта в базе данных, подходящего по правилу сопоставления.");
            checkDuplicates.Remove(duplicate);
        }


        var checkedForDuplicates = checkDuplicates.SelectMany(x => x).ToList();
        return checkedForDuplicates;
    }

    public void CheckUpdateModelsForError(ICollection<TDetails> modelsForUpdate,
        ErrorStatistics<TDetails> errorModels,
        ImportData<TDetails, TEntity> data, Func<TDetails, ImportData<TDetails, TEntity>, bool> validateFunc,
        string message)
    {
        foreach (var current in modelsForUpdate.ToList())
        {
            if (validateFunc(current, data))
            {
                errorModels.ErrorDetails.Add(current,message);
                modelsForUpdate.Remove(current);
            }
        }

        //фильтр дубликатов
        var duplicates = modelsForUpdate.ToLookup(data.DetailsKey);
        var keys = duplicates.Where(x => x.Count() > 1).Select(x => x.Key)
            .ToArray();
        foreach (var key in keys)
        {
            foreach (var userDetails in duplicates[key].Skip(1))
            {
                errorModels.ErrorDetails.Add(userDetails,"Дубликат в источнике");
                modelsForUpdate.Remove(userDetails);
            }
        }
    }

    public async Task CheckBaseDuplicates(ICollection<TDetails> importUsers, IAdditionalParametersForSelect additional,
        ErrorStatistics<TDetails> errorUsers, ImportData<TDetails, TEntity> data,
        bool checkDuplicate,
        CancellationToken token)
    {
        var checkDuplicateCount = checkDuplicate ? 1 : 0;
        //проверка базы на дубликаты по требуемым ключам
        foreach (var duplicateCheck in data.DuplicateCheckFields)
        {
            //здесь могут быть дубликаты

            var entity = (await duplicateCheck.GetEntityByKey(importUsers,additional,token)).ToList();
            //группировка дубликатов по непустому ключу 
            foreach (var detailToEntity in duplicateCheck.DetailsToEntityKeys)
            {
                var keyToImportEntity = importUsers.ToLookup(x => detailToEntity.Key(x));

                var duplicateData = entity.Where(x => detailToEntity.Value(x)?.IsSet() ?? false)
                    .GroupBy(x => detailToEntity.Value(x))
                    .Where(x => x.Count() > checkDuplicateCount)
                    .ToList();
                 
                if (duplicateData.Any())
                {
                  
                    var duplicateGroups = duplicateData.Where(x=>keyToImportEntity.Contains(x.Key)).ToList();
                    if (!duplicateGroups.Any()) continue;
                    var duplicateCheckKeyName = LogHelper.ToOutputFormat(duplicateCheck.KeyName);
                    _logger.Information($"Ошибки при импорте в базе по {duplicateCheckKeyName}");

                    foreach (var duplicateGroup in duplicateGroups)
                    {
                        var value = duplicateGroup.Key?.ToString();
                        LogHelper.ToOutputFormat(value);
                        var count = duplicateGroup.Count();
                        PrepareRemoveBaseDuplicateGroup(importUsers, errorUsers, duplicateGroup, keyToImportEntity,
                            count);
                    }
                }
            }
        }
        
    }
    

    private void PrepareRemoveBaseDuplicateGroup(ICollection<TDetails> importUsers,
        ErrorStatistics<TDetails> errorUsers,
        IGrouping<IIsSet, TEntity> duplicateGroup,
        ILookup<IIsSet, TDetails> keyToImportUser, int count)
    {
        var duplicateGroupKey = duplicateGroup.Key;

        {
            var existsMessage = $"{duplicateGroupKey} уже есть в базе.";
            var duplicateMessage = $"Дублируется в базе {duplicateGroupKey} число дубликатов {count}";
            var duplicateImportUsers = keyToImportUser[duplicateGroupKey];
            string message;
            message = count == 1 ? existsMessage : duplicateMessage;
            _logger.Information(message);

            MoveUsersToError(importUsers, errorUsers, duplicateImportUsers, message);
        }
    }

    public void MoveUsersToError(ICollection<TDetails> importUsers, ErrorStatistics<TDetails> errorUsers,
        IEnumerable<TDetails> duplicateImportUsers, string message)
    {
        foreach (var duplicateImportUser in duplicateImportUsers)
        {
            importUsers.Remove(duplicateImportUser);
            errorUsers.ErrorDetails.Add(duplicateImportUser, message);
        }
    }

    public void RemoveDuplicates(ICollection<TDetails> fromData,
        ErrorStatistics<TDetails> errorUsers,
        IImportKeyData<TDetails, TEntity> dataUserDetailsKey,
        Func<IGrouping<IIsSet, TDetails>, bool> predicate, ImportData<TDetails, TEntity> importTools)
    {
        foreach (var detailsToEntityKey in dataUserDetailsKey.DetailsToEntityKeys)
        {
           
            //группировка по ключу
            var detailsLookup = fromData.Where(x=>detailsToEntityKey.Key(x)?.IsSet() ?? false)
                .ToLookup(x=>detailsToEntityKey.Key(x));

            //ключи дубликатов из данных и имеющие соответствие в базе
            var keys = detailsLookup.Where(predicate).Select(x=>x.Key).ToList();
        
            foreach (var key in keys)
            {
                foreach (var userDetails in detailsLookup[key].Skip(1))
                {
                    var message = $"Дубликат {importTools.DetailsKey(userDetails)} по ключу {key}";
                    _logger.Information(message);
                    errorUsers.ErrorDetails.Add(userDetails, message);
                    fromData.Remove(userDetails);
                }
            }
        }
        
    }
}