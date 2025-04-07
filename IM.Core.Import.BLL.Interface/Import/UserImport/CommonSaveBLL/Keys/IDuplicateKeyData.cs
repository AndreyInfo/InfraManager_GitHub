using InfraManager.DAL;

namespace IM.Core.Import.BLL.Interface.Import.Models;

/// <summary>
/// Предоставляет доступ к расширенным данным для поиска дубликатов по выбранному клюу
/// </summary>
public interface IDuplicateKeyData<TDetails,TEntity> : IImportKeyData<TDetails,TEntity>
{
    /// <summary>
    /// Функция получения пользователей из базы по ключу, получаемому из входных данных
    /// </summary>
    Func<ICollection<TDetails>,IAdditionalParametersForSelect,CancellationToken, Task<IEnumerable<TEntity>>> GetEntityByKey { get; }
}