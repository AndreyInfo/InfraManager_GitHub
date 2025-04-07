using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Sessions;

namespace InfraManager.BLL.Sessions;

/// <summary>
/// Реализация работы системных сессий
/// </summary>
public interface ISessionHistoryBLL
{
    
    /// <summary>
    /// Получение списка историй сессий
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список историй сессий</returns>
    Task<UserSessionHistoryDetails[]> ListAsync(BaseFilter filter, CancellationToken cancellationToken = default);
}