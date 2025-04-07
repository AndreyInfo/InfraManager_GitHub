using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Calendar.CalendarHolidays;
/// <summary>
/// бизнес логика с элементами(днями) календаря
/// </summary>
public interface ICalendarHolidayItemBLL
{
    /// <summary>
    /// Получение по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarHolidayItemDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Получение списка
    /// Проверяется наличие CalendarHoliday по id элементы которого ищут, если его не существует, то удаляется
    /// </summary>
    /// <param name="search">строка поиска</param>
    /// <param name="calendarHolidayID">id CalendarHoliday</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CalendarHolidayItemDetails[]> GetListAsync(Guid calendarHolidayID, string search = "", CancellationToken cancellationToken = default);


    /// <summary>
    /// Добавление нового элемента
    /// Еслм существует уже в такую дату в этом календаре, то exception
    /// Если не сущесвует календаря по calendarHolidayID, то exception
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(CalendarHolidayItemDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление нового элемента
    /// Еслм существует уже в такую дату в этом календаре, то exception
    /// Если не сущесвует календаря по calendarHolidayID, то exception
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> UpdateAsync(CalendarHolidayItemDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="models"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(DeleteModel<Guid>[] models, CancellationToken cancellationToken);
}
