namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Создает модель или модели из доступной сущности
/// </summary>
/// <typeparam name="TEntity">Класс сущности</typeparam>
/// <typeparam name="TModel">Класс модели</typeparam>
public interface IBuildModel<TEntity,TModel>
{
    /// <summary>
    /// Создает модель сущности по данным сущности
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Модель</returns>
    Task<TModel> BuildAsync(TEntity entity, CancellationToken token);

    /// <summary>
    /// Создает массив моделей по предоставленному массиву сущностей
    /// </summary>
    /// <param name="entities">Массив сущностей</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Массив моделей</returns>
    Task<TModel[]> BuildArrayAsync(TEntity[] entities, CancellationToken token);
}