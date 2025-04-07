using InfraManager.BLL.Calendar.Exclusions;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL;

/// <summary>
/// бизнес локига с Причинами отколонения от графика
/// </summary>
public interface IExclusionBLL
{
    /// <summary>
    /// Получения Причины  по id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ExclusionDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение таблицы причин, с поиском и скролингом
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<ExclusionDetails[]> GetByFilterAsync(ExclusionFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление с проверкой на название
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(ExclusionDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление с проверкой что нет уже причины с подобым именем
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> UpdateAsync(ExclusionDetails model, CancellationToken cancellationToken);

    /// <summary>
    /// Множественное удаление
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(DeleteModel<Guid>[] model, CancellationToken cancellationToken);
}
