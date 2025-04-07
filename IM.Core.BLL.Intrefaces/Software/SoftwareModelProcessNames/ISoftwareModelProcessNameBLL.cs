using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelProcessNames;

/// <summary>
/// Бизнес логика для процессов модели ПО.
/// </summary>
public interface ISoftwareModelProcessNameBLL
{
    /// <summary>
    /// Получение списка процессов модели ПО.
    /// </summary>
    /// <param name="filter">Фильтр <see cref="SoftwareModelProcessNameFilter"/>.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив процессов для модели ПО типа <see cref="SoftwareModelProcessNameDetails"/>.</returns>
    Task<SoftwareModelProcessNameDetails[]> GetListAsync(SoftwareModelProcessNameFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление процессов для модели ПО.
    /// </summary>
    /// <param name="processNameData">Процесс модели ПО.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddProcessNameToSoftwareModelAsync(SoftwareModelProcessNameData processNameData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление процессов для модели ПО.
    /// </summary>
    /// <param name="processNameData">Процесс модели ПО.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteProcessNameFromSoftwareModelAsync(SoftwareModelProcessNameData processNameData, CancellationToken cancellationToken = default);
}
