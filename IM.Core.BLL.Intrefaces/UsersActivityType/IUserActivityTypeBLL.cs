using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;

namespace InfraManager.BLL.UsersActivityType;

/// <summary>
/// Логика работы с сущностями "Вид деятельности" <see cref="UserActivityType"/>
/// </summary>
public interface IUserActivityTypeBLL
{
    /// <summary>
    /// Получить список типов деятельности, удовлетворяющих фильтру, асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список видов деятельности удовлетворяющих фильтру.</returns>
    Task<UserActivityTypeDetails[]> GetDetailsArrayAsync(UserActivityTypeFilter filterBy, CancellationToken cancellationToken = default);
}