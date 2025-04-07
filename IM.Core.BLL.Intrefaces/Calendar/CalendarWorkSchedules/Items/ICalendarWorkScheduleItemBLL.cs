using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;

public interface ICalendarWorkScheduleItemBLL
{
    /// <summary>
    /// Получение элементов(дней) календаря графика рабочего времени
    /// </summary>
    /// <param name="filter">фильтр для пагинации и поиска</param>
    /// <param name="calendarworkScheduleID">идентификатор календаря графика рабочего времени</param>
    /// <param name="cancellationToken"></param>
    /// <returns>элементы календаря рабочего графика</returns>
    Task<CalendarWorkScheduleItemDetails[]> GetByFilterAsync(BaseFilter filter, Guid calendarworkScheduleID, CancellationToken cancellationToken);
}
