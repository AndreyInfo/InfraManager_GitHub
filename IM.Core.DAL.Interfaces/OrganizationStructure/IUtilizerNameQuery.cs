using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.OrganizationStructure;
/// <summary>
/// Запрос на получение названия использующего.
/// </summary>
public interface IUtilizerNameQuery
{
    /// <summary>
    /// Получение названия использующего.
    /// </summary>
    /// <param name="id">Идентификатор использующего.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Название использующего.</returns>
    public Task<string> ExecuteAsync(Guid id, CancellationToken cancellationToken);
}
