namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Набор методов для обновления сущности данными из модели
/// </summary>
/// <typeparam name="TModel">Класс модели</typeparam>
/// <typeparam name="TEntity">Класс сущности</typeparam>
public interface IUpdateQuery<TModel,TEntity>
{
    /// <summary>
    /// Обновляет сущность в хранилище данными из модели
    /// Выбрасывает IObjectNotFoundException, если сущность не найдена
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="model">Модель</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Обновленная сущность</returns>
    Task<TEntity> UpdateAsync(TEntity entity, TModel model, CancellationToken token);
}