namespace IM.Core.Import.BLL.Interface.Import;

public interface IBaseBLL<TKey,TEntity, TInput, TOutput, TFilter>: IBaseBLL<TKey,TFilter,TInput,TOutput>
{
    
}
public interface IBaseBLL<TKey,TFilter,TInput,TOutput>
{
     /// <summary>
    /// Получение таблицы сущностей
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица сущностей</returns>
     public Task<TOutput[]> GetDetailsArrayAsync(
        TFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение сущности по ключу 
    /// </summary>
    /// <param name="id">Ключ сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Детализация сущности</returns>
    public Task<TOutput> DetailsAsync(TKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет сущность 
    /// </summary>
    /// <param name="data">Данные сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленная сущность</returns>
    public Task<TOutput> AddAsync(TInput data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление сущности ссответствующей ключу
    /// </summary>
    /// <param name="id">Ключ сущности</param>
    /// <param name="data">Новые данные сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Обновленная сущность</returns>
    public Task<TOutput> UpdateAsync(TKey id,
        TInput data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление сущности
    /// </summary>
    /// <param name="id">Ключ удаляемой сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}