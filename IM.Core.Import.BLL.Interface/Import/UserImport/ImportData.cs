using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import.View;
using InfraManager;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Interface.Import.Models;

public class ImportData<TDetails,TEntity>
{
    public ImportData(IEnumerable<ImportModel?> importModels,
        ObjectType importFields,
        AdditionalTabDetails additionalDetails,
        bool restoreRemovedUsers,
        Func<ICollection<TDetails>, IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<TEntity>>> func,
        Func<TDetails, bool> preValidate,
        Func<TDetails, IIsSet> detailsKey,
        ImportKeyData<TDetails, TEntity> modelKey,
        Func<TDetails, TEntity, bool> checkUpdate,
        IDuplicateKeyData<TDetails, TEntity>[] duplicateCheckFields,
        Func<TDetails, bool> validateAfter,
        Func<TEntity,bool> validateBeforeCreate)
    {
        ImportModels = importModels;
        ImportFields = importFields;
        AdditionalDetails = additionalDetails;
        RestoreRemovedUsers = restoreRemovedUsers;
        Func = func;
        PreValidate = preValidate;
        DetailsKey = detailsKey;
        ModelKey = modelKey;
        IsUpdateNeeded = checkUpdate;
        DuplicateCheckFields = duplicateCheckFields;
        ValidateAfter = validateAfter;
        ValidateBeforeCreate = validateBeforeCreate;
    }

    public IEnumerable<ImportModel> ImportModels { get; init; }
    
    public ObjectType ImportFields { get; init; }
    
    public AdditionalTabDetails AdditionalDetails { get; init; }

    public bool RestoreRemovedUsers { get; init; }

    public Func<ICollection<TDetails>, IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<TEntity>>> Func { get; }
    
    public Func<TDetails,bool> PreValidate { get; }

    

    public Func<TDetails,object> DetailsKey { get; }
    
    public ImportKeyData<TDetails,TEntity> ModelKey { get; }
    
    public Func<TDetails, TEntity, bool> IsUpdateNeeded { get; }
    
    public IDuplicateKeyData<TDetails,TEntity>[] DuplicateCheckFields { get; }
    public Func<TDetails, bool> ValidateAfter { get; }

    public async Task<IEnumerable<(TDetails Detail, string Message)>> GetDuplicateKeysInBaseAsync(ICollection<TDetails> collection,
        IAdditionalParametersForSelect additional, CancellationToken token)
    {
        var result = new List<(TDetails, string)>();
        foreach (var duplicateCheck in DuplicateCheckFields)
        {
            var inBase = (await duplicateCheck.GetEntityByKey(collection, additional, token)).ToList();
            if (!inBase.Any()) continue;
            foreach (var keyFunc in duplicateCheck.DetailsToEntityKeys)
            {
                var keys = inBase.Select(x=>keyFunc.Value(x)).ToHashSet();
                var notEmptyKeys = collection.Where(x=>keyFunc.Key(x)?.IsSet()?? false);
                foreach (var userDetail in notEmptyKeys)
                {
                    if (keys.Contains(keyFunc.Key(userDetail)))
                    {
                        result.Add((userDetail,$"В базе уже есть запись с ключем {duplicateCheck.KeyName} со значением {keyFunc.Key(userDetail)}"));
                    }
                }
            }
        }

        return result;
    }

    public bool Equality(TDetails source1, TEntity source2) =>
        ModelKey.DetailsToEntityKeys.All(x => x.Key(source1) == x.Value(source2));

    public Func<TEntity, bool> ValidateBeforeCreate { get; init; }

}