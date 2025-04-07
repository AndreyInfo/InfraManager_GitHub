using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset.History;

/// <summary>
/// Интерфейс получения полного названия объекта для записи в историю имущественных операций.
/// </summary>
public interface IObjectHistoryNameGetter
{
    /// <summary>
    /// Получение полного названия объекта.
    /// </summary>
    /// <param name="id">Идентификатор объекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Полное название объекта.</returns>
    public Task<string> GetAsync(Guid id, CancellationToken cancellationToken);
}
