using InfraManager.BLL.Dashboards.ForTable;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Dashboards;

/// <summary>
/// Работа с панелью статистики
/// </summary>
public interface IDashboardBLL
{
	/// <summary>
	/// Список папок и панелей, которые доступны пользователю
	/// </summary>
	/// <param name="parentFolderID">id папки</param>
	/// <param name="userID">id пользователя</param>
	Task<IEnumerable<DashboardTreeItemDetails>> GetTreeListAsync(Guid? parentFolderID, Guid userID, CancellationToken cancellationToken = default);
	
    /// <summary>
    /// Получение выборки данных статистики
    /// </summary>
    /// <param name="filterBy">Условия выборки</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных статистики, удовлетворяющих условиям выборки</returns>
    Task<DashboardsForTableDetails[]> GetDetailsArrayAsync(DashboardListFilter filterBy, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение статистики по идентификатору
    /// </summary>
    /// <param name="id"> идентификатор статистики</param>
    /// <param name="cancellationToken"> токен отмены </param>
    /// <returns> статистика </returns>
    Task<DashboardFullDetails> AllDetailsAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление статистики из базы данных
    /// </summary>
    /// <param name="id"> идентификатор статистики</param>
    /// <param name="cancellationToken"> токен отмены </param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Добавление статистики в базу данных
    /// </summary>
    /// <param name="report"> статистика</param>
    /// <param name="cancellationToken"> токен отмены </param>
    Task<DashboardFullDetails> InsertAsync(DashboardFullData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление статистики в базе данных
    /// </summary>
    /// <param name="id">ID статистики</param>
    /// <param name="data"> статистика</param>
    /// <param name="cancellationToken"> токен отмены </param>
    Task<DashboardFullDetails> UpdateDashboardAsync(Guid id, DashboardFullData data, CancellationToken cancellationToken);


    /// <summary>
    /// Заменяет xml значения у статистики
    /// </summary>
    /// <param name="id">ID статистики</param>
    /// <param name="data">Новое xml значение</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task UpdateDashboardData(Guid id, string data, CancellationToken cancellationToken = default);
}