using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk;

/// <summary>
/// Этот интерфейс позволяет привести дату-время в время в часовом поясе Службы поддержки.
/// </summary>
public interface IServiceDeskDateTimeConverter
{
    /// <summary>
    /// Преобразовать указанное время в время в часовом поясе Службы поддержки. 
    /// </summary>
    /// <param name="dateTime">Время.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="DateTimeOffset"/>, соответствующий заданному времени в часовом поясе Службы поддержки.</returns>
    Task<DateTimeOffset> ConvertAsync(DateTime dateTime, CancellationToken cancellationToken = default);
}