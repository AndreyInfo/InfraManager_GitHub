using InfraManager;

namespace IMCore.Import.BLL.Interface.Authorization;

/// <summary>
/// Этот интерфейс описывает BLL сервис проверки прав на доступ пользователя к системе
/// </summary>
public interface IUserAccessBLL
{
    Task<bool> UserHasOperationAsync(Guid userId, OperationID operation, CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Возвращвет true если хотябы одна из операций есть у пользователя
    /// </summary>
    /// <param name="userId">ID Пользователя</param>
    /// <param name="operations">спитсок операций</param>
    /// <param name="cancellationToken"></param>
    Task<bool> UserHasAnyOperationsAsync(Guid userId, IEnumerable<OperationID> operations,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<OperationID>> GrantedOperationsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasAdminRoleAsync(Guid userId, CancellationToken cancellationToken = default);
}