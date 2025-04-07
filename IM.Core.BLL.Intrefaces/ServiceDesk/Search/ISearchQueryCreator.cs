using System;
using System.Linq;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Search;

public interface ISearchQueryCreator
{
    /// <summary>
    /// Создает запрос для <see cref="ILightSearcher"/>
    /// </summary>
    /// <param name="id">ID текущего пользователя</param>
    /// <returns></returns>
    IQueryable<MyTasksListQueryResultItem> CreateQuery(Guid id);
}