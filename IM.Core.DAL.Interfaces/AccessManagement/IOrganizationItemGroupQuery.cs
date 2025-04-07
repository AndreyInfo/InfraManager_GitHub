using System;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.DAL.AccessManagement;

public interface IOrganizationItemGroupQuery
{
    /// <summary>
    /// Получене запроса кто имеет права к объекту
    /// Включена возможность поиска
    /// </summary>
    /// <param name="objectID">идентификатор объекта</param>
    /// <param name="objectClass">его тип</param>
    /// <param name="searchString">строка поиска</param>
    /// <param name="sortColumn">колонка для сортировки</param>
    /// <param name="take">параметр для пагинации</param>
    /// <param name="skip">параметр для пагинации</param>
    /// <param name="cancellationToken"></param>
    /// <returns>элементы которые имеют доступ к объекту</returns>
    Task<AccessPermissionModelItem[]> ExecuteAsync(Guid objectID
        , ObjectClass objectClass
        , string searchString
        , Sort sortColumn
        , string[] mappedValues
        , int take
        , int skip
        , CancellationToken cancellationToken);
}