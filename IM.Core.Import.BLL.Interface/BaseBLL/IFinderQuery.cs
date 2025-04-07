namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Набор методов для предоставления сущности по ключу
/// </summary>
/// <typeparam name="TKey">Класс ключа</typeparam>
/// <typeparam name="TEntity">Класс сущности</typeparam>
public interface IFinderQuery<TKey, TEntity>
{
    /// <summary>
    /// Возвращает сущность соответствующую предоставленнному ключу
    /// Возвращает null, если сущность не найдена
    /// </summary>
    /// <param name="key"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<TEntity> GetFindQueryAsync(TKey key, CancellationToken token);
}