using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL;

public interface ITimeZoneObjects
{

    /// <summary>
    /// Обновляет у всех сущностей одного типа timeZoneID
    /// </summary>
    /// <param name="timeZoneID">новое значение идентификатора TimeZone</param>
    /// <param name="cancellationToken"></param>
    Task UpdateTimeZoneObjectsAsync(string timeZoneID, CancellationToken cancellationToken);
}