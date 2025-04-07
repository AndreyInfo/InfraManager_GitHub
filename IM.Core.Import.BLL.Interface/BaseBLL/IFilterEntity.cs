namespace IM.Core.Import.BLL.Interface.Import;

/// <summary>
/// Строит запрос для сущностей в хранилище по заданному фильтру
/// </summary>
/// <typeparam name="TEntity">Класс сущностей</typeparam>
/// <typeparam name="TFilter">Класс фильра</typeparam>
public interface IFilterEntity<TEntity,TFilter>
{
    /// <summary>
    /// Строит и возвращает запрос в хранилище для указанного фильтра
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <returns>Запрос к данным в хранилище</returns>
    IQueryable<TEntity> Query(TFilter filter);
}