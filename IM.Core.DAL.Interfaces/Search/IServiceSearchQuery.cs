using System;
using System.Linq;

namespace InfraManager.DAL.Search;

/// <summary>
/// Определяет интерфейс запроса для поиска сервиса в каталоге.
/// </summary>
public interface IServiceSearchQuery
{
    /// <summary>
    /// Возвращает запрос получения сервисов, удовлетворяющих заданным критериям.
    /// </summary>
    /// <param name="criteria">Критерий поиска сервисов.</param>
    /// <param name="userID">Уникальный идентификатор пользователя.</param>
    /// <returns><see cref="IQueryable{ObjectSearchResult}"/>.</returns>
    IQueryable<ObjectSearchResult> Query(ServiceSearchCriteria criteria, Guid userID);
}