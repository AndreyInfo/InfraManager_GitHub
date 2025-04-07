using System;

namespace InfraManager.BLL.Roles;

public class RoleInsertDetails
{
    /// <summary>
    /// Задает название роли
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Задает описание роли
    /// </summary>
    public string Note { get; init; }

    public OperationModelForRole<int>[] Operations { get; init; }

    public OperationModelForRole<Guid>[] LifeCycleStateOperations { get; init; }
}
