using InfraManager.DAL.Software;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelDependencies;

/// <summary>
/// Бизнес логика для работы с сущностью SoftwareModelDependency
/// </summary>
public interface ISoftwareModelDependencyBLL
{
    /// <summary>
    /// Получение списка зависимостей моделей ПО
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SoftwareModelDependencyDetails[]> GetListAsync(SoftwareModelDependencyFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление новой зависимости модели ПО
    /// </summary>
    /// <param name="model">Модель</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SoftwareModelDependencyDetails> AddAsync(SoftwareModelDependencyData model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление зависимости модели ПО
    /// </summary>
    /// <param name="dependencyKey">Модель для удаления.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(SoftwareModelDependencyData dependencyKey, CancellationToken cancellationToken = default);

}
