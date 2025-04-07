using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

/// <summary>
/// Реализация системных сессий
/// </summary>
public interface ISystemSessionBLL
{
    /// <summary>
    /// Создает новую сессию или продлевает существующую
    /// </summary>
    /// <param name="userID">ID пользователя сессии</param>
    /// <param name="securityStamp">Секретная информация сессии</param>
    /// <param name="userAgent">User Agent сессии</param>
    /// <param name="locationType">Откуда подключение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task CreateOrRestoreAsync(Guid userID, string securityStamp, string userAgent, SessionLocationType locationType,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Заканчивает выбранную сессию(когда пользователь выходит сам)
    /// </summary>
    /// <param name="userID">ID пользователя сессии</param>
    /// <param name="userAgent">User Agent сессии</param>
    /// <param name="securityStamp">Секретная информация сессии</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task AbortAsync(Guid userID, string userAgent, string securityStamp,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Продлевает выбранную сессию
    /// </summary>
    /// <param name="userID">ID пользователя сессии</param>
    /// <param name="userAgent">User Agent сессии</param>
    /// <param name="securityStamp">Секретная информация сессии</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task ExtendAsync(Guid userID, string userAgent, string securityStamp, CancellationToken cancellationToken);

    /// <summary>
    /// Отключает неактивные сессии пользователей по истечению времени неактивности которые
    /// указываются в настройках системы
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeactivateInactiveSessionsAsync(CancellationToken cancellationToken = default);
}