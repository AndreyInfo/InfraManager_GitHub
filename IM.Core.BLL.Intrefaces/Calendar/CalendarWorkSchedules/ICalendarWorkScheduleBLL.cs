using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules;

/// <summary>
/// Бизнес логика для Рабочиx Календарей
/// </summary>
public interface ICalendarWorkScheduleBLL
{
    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="id">идентификатор обновляемой сущности</param>
    /// <param name="data">модель для обновления</param>
    /// <param name="cancelationToken"></param>
    /// <returns>идентификатор обновленной сущности</returns>
    Task<Guid> UpdateAsync(Guid id, CalendarWorkScheduleData data, CancellationToken cancelationToken = default);

    /// <summary>
    /// Удаление, удаляет все связанные сущности тоже
    /// Если используется в системе, то выбрасывается ошибка клиенту
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущности</param>
    /// <param name="cancelationToken"></param>
    Task RemoveAsync(Guid id, CancellationToken cancelationToken = default);

    /// <summary>
    /// Получение рабочих дней
    /// Есть возможность для пересчета
    /// </summary>
    /// <param name="filter">фильтр для поиска и пагинации</param>
    /// <param name="calendarWorkScheduleID">идентифи</param>
    /// <param name="model">параметры для пересчета</param>
    /// <param name="cancellationToken"></param>
    /// <returns>рабочие дни</returns>
    Task<CalendarWorkScheduleItemDetails[]> GetDaysByIDAsync(BaseFilter filter, Guid calendarWorkScheduleID, DataCalculateWorkSheduleDays model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение рабочиx календарей
    /// С пагинацией и поисков
    /// </summary>
    /// <param name="filter">фильтр для пагинации и поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>рабочие календари</returns>
    Task<CalendarWorkScheduleDetails[]> GetAsync(BaseFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение рабочего календаря
    /// </summary>
    /// <param name="id">идентификатор получаемого календаря</param>
    /// <param name="cancellationToken"></param>
    /// <returns>рабочий календарь</returns>
    Task<CalendarWorkScheduleWithRelatedDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Создание рабочего календаря
    /// </summary>
    /// <param name="calendarWorkScheduleDetails">модель рабочего календаря</param>
    /// <param name="cancelationToken"></param>
    /// <returns>идентификатор добавленого календаря</returns>
    Task<Guid> CreateAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, CancellationToken cancelationToken);

    /// <summary>
    /// Создание по аналогии
    /// </summary>
    /// <param name="calendarWorkScheduleDetails">модель для создания</param>
    /// <param name="cancelationToken"></param>
    /// <returns>идентификатор добавленого календаря</returns>
    Task<Guid> CreateByAnalogyAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, CancellationToken cancelationToken);

    /// <summary>
    /// Получить исключения заданного графика работы удовлетворяющие заданному фильтру.
    /// </summary>
    /// <param name="workScheduleID">Уникальный идентификатор графика работы.</param>
    /// <param name="dayOfYear">Номер дня в году (<c>null</c> - по всем дням в графике).</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Массив <see cref="CalendarWorkScheduleItemExclusionDetails"/>.</returns>
    Task<CalendarWorkScheduleItemExclusionDetails[]> GetExclusionsAsync(Guid workScheduleID, int? dayOfYear, CancellationToken cancellationToken = default);
}
