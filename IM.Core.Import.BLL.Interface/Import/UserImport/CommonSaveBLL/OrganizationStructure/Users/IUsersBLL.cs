using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.DAL;


namespace IM.Core.Import.BLL.Interface.OrganizationStructure.Users;

/// <summary>
/// Интерфейс для сущности Пользователь
/// </summary>
public interface IUsersBLL
{
    /// <summary>
    /// Метод создает пользователей
    /// </summary>
    /// <param name="users">список пользователей</param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task<int> CreateUsersAsync(IEnumerable<User> users, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод обновляет пользователей
    /// </summary>
    /// <param name="updateUsers">список пользователей для обновления и список пользователей, которые обновятся</param>
    /// <param name="data"></param>
    /// <param name="cancellationToken">отмена задачи</param>
    /// <returns> результат операции </returns>
    public Task<int> UpdateUsersAsync(Dictionary<IUserDetails, User> updateUsers, ImportData<IUserDetails, User> data,
        CancellationToken cancellationToken = default);
}