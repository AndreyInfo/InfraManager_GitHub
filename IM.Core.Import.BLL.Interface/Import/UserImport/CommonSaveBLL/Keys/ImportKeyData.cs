using IM.Core.Import.BLL.Import.Array;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Interface.Import.Models;

public class ImportKeyData<TDetails,TEntity> : IImportKeyData<TDetails,TEntity>
{
    private readonly IReadOnlyDictionary<Func<TDetails, IIsSet>, Func<TEntity, IIsSet>> _keys;
    public ImportKeyData(Dictionary<Func<TDetails, IIsSet>, Func<TEntity, IIsSet>> userKey, string keyName)
    {
        _keys = userKey;
        KeyName = keyName;
    }

    public string KeyName { get;}

    public IReadOnlyDictionary<Func<TDetails, IIsSet>, Func<TEntity, IIsSet>> DetailsToEntityKeys => _keys;

    public IEnumerable<Func<TDetails, IIsSet>> DetailsKey => _keys.Keys;

    public IEnumerable<Func<TEntity, IIsSet>> EntityKey => _keys.Values;
}