using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;

namespace InfraManager.BLL.ColumnMapper;

public interface IOrderedColumnQueryBuilder<TEntity, TTable>
{
    /// <summary>
    /// Строит IOrderedQueryable на основе данных колонок для текущего пользователя для таблицы
    /// </summary> 
    /// <param name="viewName">Название таблицы</param>
    /// <param name="query">Query запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>IOrderedQueryable для определенной таблицы</returns>
    Task<IOrderedQueryable<TEntity>> BuildQueryAsync(
        string viewName,
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default);
}