using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.Calendar;

public interface ISupportSettingsCalendarBLL
{
    /// <summary>
    /// Получение настроек календаря
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>модель настроек календаря</returns>
    Task<CalendarSettingsDetails> GetAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Обновление настроек календаря
    /// </summary>
    /// <param name="model">обновленная модель</param>
    /// <param name="cancellationToken"></param>
    /// <returns>обновленная модель настроек календаря</returns>
    Task<CalendarSettingsDetails> UpdateAsync(CalendarSettingsDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление временной зоны у объектов в системе
    /// </summary>
    /// <param name="timeZoneID">идентификатор временной зоны</param>
    /// <param name="cancellationToken"></param>
    Task UpdateObjectTimeZoneAsync(string timeZoneID, CancellationToken cancellationToken);
}
