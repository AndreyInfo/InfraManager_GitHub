using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.MaintenanceWork.Maintenances;

/// <summary>
/// Регламентные работы
/// </summary>
public interface IMaintenanceBLL
{
    /// <summary>
    /// Добавляет или обновляет регл. работу
    /// </summary>
    /// <param name="maintenanceData">Контракт добавления регламентной работы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<MaintenanceDetails> AddAsync(MaintenanceData maintenanceData,
       CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет регл. работу
    /// </summary>
    /// <param name="data">Контракт обновления регламентной работы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<MaintenanceDetails> UpdateAsync(Guid id, MaintenanceData data
        , CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет регл. работу
    /// </summary>
    /// <param name="id">ID удаляемой релагламентной работы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает список регл. работ по идентификатору папки
    /// </summary>
    /// <param name="filter">Фильтр для получения списка регалментных работ</param>
    /// <returns></returns>
    Task<MaintenanceDetails[]> GetByFolderIDAsync(MaintenanceFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает регл. работу
    /// </summary>
    /// <param name="id">ID регалментной работы</param>
    Task<MaintenanceDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
