using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Sessions;

public interface ISessionDeactivator
{
     /// <summary>
     /// Принудительно отключает сессию пользователя
     /// </summary>
     /// <param name="userID">ID пользователя сессии</param>
     /// <param name="userAgent">User Agent сессии</param>
     /// <param name="cancellationToken">Токен отмены</param>
     Task DeactivateAsync(Guid userID, string userAgent, CancellationToken cancellationToken = default);
}