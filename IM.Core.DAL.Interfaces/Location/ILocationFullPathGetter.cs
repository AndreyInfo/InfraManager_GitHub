using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Location;

/// <summary>
/// Интерфейс получения полного названия местоположения для сущности, реализующей интерфейс <see cref="ILocationObject"/>.
/// </summary>
public interface ILocationFullPathGetter
{
    /// <summary>
    /// Получение полного названия местоположения.
    /// </summary>
    /// <param name="id">Идентификатор местоположения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Полное название местоположения.</returns>
    public Task<string> GetAsync(Guid id, CancellationToken cancellationToken);
}
