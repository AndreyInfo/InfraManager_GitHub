using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfraManager.BLL.CrudWeb;

namespace InfraManager.BLL.Calendar.CalendarExclusions;

/// <summary>
/// бизнес логика для CalendarExclusion
/// </summary>
public interface ICalendarExclusionBLL
{
    /// <summary>
    /// Обновление сущности CalendarExclusion
    /// </summary>
    /// <param name="calendarExclusion"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarExclusionDetails> UpdateAsync(CalendarExclusionDetails calendarExclusion, Guid id,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Добавление CalendarExclusion, сразу создается новая ссылка на сервис
    /// </summary>
    /// <param name="calendarExclusion"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarExclusionDetails> AddAsync(CalendarExclusionInsertDetails calendarExclusion
        , CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка сущностей с фильтрацией
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarExclusionDetails[]> GetByFilterAsync(BaseFilterWithClassIDAndID<Guid> filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// удаление
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> DeleteAsync(DeleteModel<Guid>[] deleteModels, CancellationToken cancellationToken = default);


    /// <summary>
    /// Получение CalendarExclusion по id
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarExclusionDetails> GetByIDAsync(Guid exclusionID,
        CancellationToken cancellationToken = default);
}
