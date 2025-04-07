using InfraManager.BLL.CalendarService;
using InfraManager.DAL.CalendarWorkSchedules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL;
public interface ICalendarServiceBLL
    {
        Task<TimeSpan> GetWorkTimeByCalendarAsync(WorkTimeByCalendarData workTimeByCalendarData, CancellationToken cancellationToken = default);
        Task<TimeSpan> GetWorkTimeByUserAsync(WorkTimeByUserData workTimeByUserData, CancellationToken cancellationToken = default);
        Task<DateTime> GetWorkDateByCalendarAsync(WorkDateByCalendarData workDateByCalendarData, CancellationToken cancellationToken = default);
        Task<DateTime> GetWorkDateByUserAsync(WorkDateByUserData workDateByUserData, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Возвращает время для поля "Завершить до" для сущности
        /// </summary>
        /// <param name="timeToDo">Время исполнения сущности (проблемы, задачи)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Значение для поля "Завершить до"</returns>
        Task<DateTime> GetDatePromisedAsync(long timeToDo, CancellationToken cancellationToken = default);
        Task<DateTime> GetUtcFinishDateByCalendarAsync(DateTime startDate, TimeSpan duration, CalendarWorkSchedule calendar, DAL.ServiceDesk.TimeZone calendarTimeZone);
        /// <summary>
        /// Получить рабочее время для группы в заданный период времени асинхронно.
        /// </summary>
        /// <param name="data">Предоставляет данные для расчета.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Промежуток рабочего времени заданной группы в указанном периоде.</returns>
        Task<TimeSpan> GetWorkTimeByGroupAsync(WorkTimeByGroupData data, CancellationToken cancellationToken = default);
    }
