using System;

namespace InfraManager.BLL.Roles;

public class RoleDetails
{
    /// <summary>
    /// Возвращает идентификатор роли
    /// </summary>
    public Guid ID { get; init; }

    /// <summary>
    /// Возвращает или задает название роли
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Возвращает или задает описание роли
    /// </summary>
    public string Note { get; init; }

    /// <summary>
    /// Возвращает идентификатор версии роли
    /// </summary>
    public byte[] RowVersion { get; init; }

    /// <summary>
    /// Флаг является ли данная роль Ролью Администратора
    /// </summary>
    public bool IsAdmin { get; init; }

    public OperationModelForRole<int>[] Operations { get; init; }

    public OperationModelForRole<Guid>[] LifeCycleStateOperations { get; init; }
}
