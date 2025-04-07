using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.MaintenanceWork.Folders;

/// <summary>
/// Бизнес логика для Папок регламентых работ
/// </summary>
public interface IMaintenanceFolderBLL
{
    /// <summary>
    /// Получение папки регламентных работ по идентификатору
    /// </summary>
    /// <param name="id">идентификатор папки</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>модель папки регламентных работ</returns>
    Task<MaintenanceFolderDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление папки регламентых работ
    /// </summary>
    /// <param name="data">добавляемая папка</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>идентификатор добавленной папки</returns>
    Task<MaintenanceFolderDetails> AddAsync(MaintenanceFolderData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление папки регламентных работ
    /// </summary>
    /// <param name="id">ID обновляемой папки регламентных работ</param>
    /// <param name="model">модель с обновленными данными </param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>идентификатор папки регламентых работ</returns>
    Task<MaintenanceFolderDetails> UpdateAsync(Guid id, MaintenanceFolderData model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Удаление папки регламентных работ, по идентификатору
    /// </summary>
    /// <param name="id">идентификатор удаляемой папки</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
