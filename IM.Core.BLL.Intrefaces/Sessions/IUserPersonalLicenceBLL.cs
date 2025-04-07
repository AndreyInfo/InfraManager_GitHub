using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Sessions;

/// <summary>
/// Класс реализует системеную работу с пользовательскими сессиями
/// </summary>
public interface IUserPersonalLicenceBLL
{
    /// <summary>
    /// Показывает, есть ли у пользователя персональная лицензия
    /// </summary>
    /// <param name="userID">ID Пользователя</param>
    /// <param name="maxCount">Максимальное кол-во персональных лицензий</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns><c>True</c> если у пользователя есть персональная лицензия, иначе <c>false</c></returns>
    Task<bool> HasPersonalLicenceAsync(Guid userID, int maxCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает список персональных лицензий
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список персональных лицензий</returns>
    Task<UserPersonalLicenceDetails[]> ListAsync(BaseFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет новую персональную лицензию
    /// </summary>
    /// <param name="userID">ID пользователя, для которого добавляется персональная лицензия</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task InsertAsync(Guid userID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет персональную лицензию
    /// </summary>
    /// <param name="userID">ID пользователя, для которого удаляется персональная лицензия</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid userID, CancellationToken cancellationToken = default);
}