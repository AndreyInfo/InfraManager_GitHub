using System.Collections.Generic;

namespace InfraManager.DAL.Users;

/// <summary>
/// Предоставляет возможность получения пользователей в
/// том числе и удаленных с фильтрацие на сервере
/// </summary>
public interface IGetUserByCriteriaQuery
{
    /// <summary>
    /// Получение последовательности пользователей по критерию
    /// </summary>
    /// <param name="criteria">Критерий выбора пользователей</param>
    /// <returns>Последовательность пользователей</returns>
    IEnumerable<User> ExecuteQuery(UserCriteria criteria);
}