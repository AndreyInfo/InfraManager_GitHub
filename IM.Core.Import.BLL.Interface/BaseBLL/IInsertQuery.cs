namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Набор методов для работы с добавлением сущностей из моделей в хранилище
/// </summary>
/// <typeparam name="TModel">Класс модели</typeparam>
/// <typeparam name="TEntity">Класс сущности</typeparam>
public interface IInsertQuery<TModel,TEntity>
{
    /// <summary>
    /// Добавляет сущность в хранилище из модели
    /// </summary>
    /// <param name="model">Модель</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Созданная сущность</returns>
    Task<TEntity> AddAsync(TModel model, CancellationToken token);
}