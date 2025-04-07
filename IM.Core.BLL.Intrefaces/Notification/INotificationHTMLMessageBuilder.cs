using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification;

/// <summary>
/// Интерфейс позволяет создавать текст из списка сообщений
/// </summary>
public interface INotificationHTMLMessageBuilder
{
    /// <summary>
    /// Создает текст с учетом HTML разметки
    /// </summary>
    /// <param name="messages">Список сообщений</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task<string> BuildMessageHTMLTextAsync(IEnumerable<Note> messages, CancellationToken cancellationToken);
    
    /// <summary>
    /// Создает текст без учета HTML разметки
    /// </summary>
    /// <param name="messages">Список сообщений</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task<string> BuildMessageTextAsync(IEnumerable<Note> messages, CancellationToken cancellationToken);
}