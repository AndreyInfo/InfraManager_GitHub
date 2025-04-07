using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarWeekends;

/// <summary>
/// Бизнес логика для выходных дней
/// </summary>
public interface ICalendarWeekenedBLL
{
    /// <summary>
    /// Удаление, с проверками на использование в системе и выыводом сообщений пользователю
    /// </summary>
    /// <param name="id">идентификатор удаляемого календаря</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
