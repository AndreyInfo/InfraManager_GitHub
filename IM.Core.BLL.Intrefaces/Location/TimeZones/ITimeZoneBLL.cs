using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Location.TimeZones;

/// <summary>
/// Бизнес логика с временными зонами
/// </summary>
public interface ITimeZoneBLL
{
    /// <summary>
    /// Получение временных зон с возможностью поиска
    /// </summary>
    /// <param name="filter">фильтр для поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели временных зон</returns>
    Task<IEnumerable<TimeZoneDetails>> GetDataForTableAsync(TimeZoneListFilter filter, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получение временной зоны по идентификатору
    /// </summary>
    /// <param name="id">идентификатор временной зоны</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модель временной зоны</returns>
    Task<TimeZoneDetails> GetAsync(string id, CancellationToken cancellationToken = default);
}
