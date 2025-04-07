using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

public interface ICalendarWorkScheduleShiftExlusionsBLL
{
    /// <summary>
    /// СОздание причины отлонения от графика
    /// </summary>
    /// <param name="exclusionDetails">модель создаваемой причины отклонения от графика</param>
    /// <param name="cancellationToken"></param>
    /// <returns>идентификатор добавленой причины отклонения</returns>
    Task<Guid> CreateExclutionAsync(CreateCalendarWorkScheduleShiftExclusionDetails exclusionDetails, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление связи смеены и отклоения
    /// </summary>
    /// <param name="id">идентификатор связи</param>
    /// <param name="cancellationToken"></param>
    Task DeleteExclusionAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление причины отклонения от графика
    /// </summary>
    /// <param name="exclusionDetails">обновленная модель</param>
    /// <param name="cancellationToken"></param>
    Task UpdateExclutionAsync(CalendarWorkScheduleShiftExclusionDetails exclusionDetails, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение причин отклонения от графика с возможностью пагинации и поиска
    /// Для конкретной смены
    /// </summary>
    /// <param name="filter">филтр для пагинации и поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>список моделей причин отклонения от графика</returns>
    Task<CalendarWorkScheduleShiftExclusionDetails[]> GetExclutionsAsync(FilterShiftsExclusion filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение полного времени отклонения от графика
    /// </summary>
    /// <param name="exclusionId">идентификатор причины отклонения</param>
    /// <param name="cancellationToken"></param>
    /// <returns>кол-во минут отклонения</returns>
    Task<int> GetExclutionsTotalTimeAsync(Guid exclusionId, CancellationToken cancellationToken = default);
}
