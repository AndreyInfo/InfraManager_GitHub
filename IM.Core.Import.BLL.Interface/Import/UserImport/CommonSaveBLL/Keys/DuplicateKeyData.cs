using IM.Core.Import.BLL.Import.Array;
using InfraManager.DAL;
using Newtonsoft.Json.Serialization;

namespace IM.Core.Import.BLL.Interface.Import.Models;

public class DuplicateKeyData<TDetails, TEntity> : IDuplicateKeyData<TDetails,TEntity>
{
    private readonly IImportKeyData<TDetails,TEntity> _keyData;

    public DuplicateKeyData(IImportKeyData<TDetails,TEntity> keyData, 
        Func<ICollection<TDetails>,IAdditionalParametersForSelect,CancellationToken, Task<IEnumerable<TEntity>>> getUsersByKey)
    {
        _keyData = keyData;
        GetEntityByKey = getUsersByKey;
    }

    public string KeyName => _keyData.KeyName;

    public IEnumerable<Func<TDetails, IIsSet>> DetailsKey => _keyData.DetailsKey;

    public IEnumerable<Func<TEntity, IIsSet>> EntityKey => _keyData.EntityKey;

    public IReadOnlyDictionary<Func<TDetails, IIsSet>, Func<TEntity, IIsSet>> DetailsToEntityKeys => _keyData.DetailsToEntityKeys;

    public Func<ICollection<TDetails>,IAdditionalParametersForSelect, CancellationToken, Task<IEnumerable<TEntity>>> GetEntityByKey { get; }
}