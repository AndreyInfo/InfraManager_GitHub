using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts;

public interface ICalendarWorkScheduleShiftsBLL
{

    /// <summary>
    /// Получение смен для Рабочего графика
    /// </summary>
    /// <param name="calendarWorkScheduleID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarWorkScheduleShiftDetails[]> GetShiftsByIDAsync(Guid calendarWorkScheduleID
        , CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление смены рабочего графика
    /// </summary>
    /// <param name="calendarWorkScheduleDetails">обновленная модель смены</param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(CalendarWorkScheduleShiftDetails calendarWorkScheduleDetails, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление смен
    /// </summary>
    /// <param name="id">идентификатор удаяемой смены</param>
    /// <param name="cancelationToken"></param>
    /// <returns>идентификаторы не удаленных смен</returns>
    Task RemoveAsync(Guid id, CancellationToken cancelationToken = default);

    /// <summary>
    /// Получение смены по идентификатору
    /// </summary>
    /// <param name="id">идентификатор получаемой смены</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модель смены</returns>
    Task<CalendarWorkScheduleShiftDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление смены рабочего графика
    /// </summary>
    /// <param name="calendarWorkScheduleShiftDetails">модель смены</param>
    /// <param name="cancellationToken"></param>
    /// <returns>идентификатор добавленной смены</returns>
    Task<Guid?> AddAsync(CalendarWorkScheduleShiftCreateData calendarWorkScheduleShiftDetails, CancellationToken cancellationToken = default);
}
