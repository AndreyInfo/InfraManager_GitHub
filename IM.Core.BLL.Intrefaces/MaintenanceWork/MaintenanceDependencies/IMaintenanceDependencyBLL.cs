using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

/// <summary>
/// Зависимости регламентных работ
/// </summary>
public interface IMaintenanceDependencyBLL
{
    /// <summary>
    /// Получение зависимостей с регламентной работой
    /// С возможностью поиска и пагинации
    /// </summary>
    /// <param name="filter">фильтр для поиска и пагинации</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>модели зависимосотей</returns>
    Task<MaintenanceDependencyDetails[]> GetByMaintenanceIDAsync(MaintenanceDependencyFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление зависимости
    /// </summary>
    /// <param name="data">модель добавляемой зависимости</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>идентификатор регламентной работы к которой добавили зависимость</returns>
    Task<Guid> AddAsync(MaintenanceDependencyData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление зависимости
    /// </summary>
    /// <param name="model">модель обновляемой зависимости</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>идентификатор регламентной работы к которой добавили зависимость</returns>
    Task<Guid> UpdateAsync(MaintenanceDependencyData model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление зависмости между объектом и регламентной работой
    /// </summary>
    /// <param name="key">модель для удаления</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task DeleteAsync(MaintenanceDependencyDeleteKey key, CancellationToken cancellationToken = default);
}
