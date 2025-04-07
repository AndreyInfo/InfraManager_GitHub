using Inframanager.BLL;
using InfraManager.DAL.Dashboards;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Dashboards;

/// <summary>
/// Интерфейс для бизнес-логики Папки статистики
/// </summary>
public interface IDashboardFolderBLL
{
    /// <summary>
    /// Получение списка папок статистики
    /// </summary>
    /// <param name="filter">Фильтр сортировки</param>
    /// <param name="cancellationToken"> токен отмены </param>
    /// <returns> список статистики </returns>
    Task<DashboardFolderDetails[]> GetDetailsPageAsync(DashboardFolderListFilter filterBy, ClientPageFilter<DashboardFolder> pageBy, CancellationToken cancellationToken);
    /// <summary>
    /// Получение выборки данных папок статистики
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных статистики, удовлетворяющих условиям выборки</returns>
    Task<DashboardFolderDetails[]> GetDetailsArrayAsync(DashboardFolderListFilter filterBy, CancellationToken cancellationToken);
    /// <summary>
    /// Получение папок статистики по идентификатору
    /// </summary>
    /// <param name="id"> идентификатор папки статистики</param>
    /// <param name="cancellationToken"> токен отмены </param>
    /// <returns> статистика </returns>
    Task<DashboardFolderDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Удаление папки статистики из базы данных
    /// </summary>
    /// <param name="id"> идентификатор папки статистики</param>
    /// <param name="cancellationToken"> токен отмены </param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Добавление папки статистики в базу данных
    /// </summary>
    /// <param name="report"> статистика</param>
    /// <param name="cancellationToken"> токен отмены </param>
    Task<DashboardFolderDetails> AddAsync(DashboardFolderData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление папки статистики в базе данных
    /// </summary>
    /// <param name="id">ID статистики</param>
    /// <param name="data"> статистика</param>
    /// <param name="cancellationToken"> токен отмены </param>
    Task<DashboardFolderDetails> UpdateAsync(Guid id, DashboardFolderData data, CancellationToken cancellationToken);
}
