using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Sessions;

/// <summary>
/// Класс реализует работу с пользовательскими сессиями
/// </summary>
public interface ISessionBLL
{
    /// <summary>
    /// Получение списка активных сессий
    /// </summary>
    /// <param name="filter">Фильтр выборки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список активных сессий</returns>
    Task<SessionDetails[]> ListAsync(SessionFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Отключает выбранную сессию.
    /// (userID и userAgent) используются как ключ для поиска сессии
    /// </summary>
    /// <param name="userID">ID пользователя сессии</param>
    /// <param name="userAgent">User Agent сессии</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeactivateSessionAsync(Guid userID, string userAgent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает актуальную статистику по сессиям на данный момент
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Актуальная статистика по сессиям</returns>
    Task<SessionsStatisticDetails> SessionStatisticAsync(CancellationToken cancellationToken = default);
}