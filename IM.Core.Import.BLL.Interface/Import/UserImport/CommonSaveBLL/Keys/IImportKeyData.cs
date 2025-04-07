using IM.Core.Import.BLL.Import.Array;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Interface.Import.Models;

/// <summary>
/// Предоставляет доступ для поиска информации по выбранному ключу
/// </summary>
public interface IImportKeyData<TDetails, TEntity>
{
    /// <summary>
    /// Название ключа
    /// </summary>
    string KeyName { get; }
    
    /// <summary>
    /// Получение ключа из UserDetails
    /// </summary>
    IEnumerable<Func<TDetails, IIsSet>> DetailsKey { get; }
    
    /// <summary>
    /// Получение ключа из Users
    /// </summary>
    IEnumerable<Func<TEntity, IIsSet>> EntityKey { get; }

    IReadOnlyDictionary<Func<TDetails, IIsSet>, Func<TEntity, IIsSet>> DetailsToEntityKeys { get; }
}