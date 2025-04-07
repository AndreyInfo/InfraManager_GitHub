using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.OrganizationStructure;
/// <summary>
/// Запрос на получение названия владельца.
/// </summary>
public interface IOwnerNameQuery
{
    /// <summary>
    /// Получение названия владельца.
    /// </summary>
    /// <param name="id">Идентификатор владельца.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Название владельца.</returns>
    public Task<string> ExecuteAsync(Guid id, CancellationToken cancellationToken);
}
