using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

/// <summary>
/// Календарь праздничных дней
/// </summary>
public interface ICalendarHolidayBLL
{
    /// <summary>
    /// Получение календаря праздничных дней, с его элементами
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarHolidayDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление, входящие элементы календаря тоже сохраняются
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(CalendarHolidayInsertDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление, элементы так же перезаписываются
    /// старые удаляются, записываются входящие
    /// </summary>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> UpdateAsync(CalendarHolidayDetails model, Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Удаление, по идентификатору, со всеми вложенными элементами
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущности</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
