using System;

namespace InfraManager.BLL.Roles;

public class UserRolesWithSelectedData
{
    /// <summary>
    ///     Идентификатор роли
    /// </summary>
    public Guid ID { get; init; }

    /// <summary>
    ///     Выбрана ли роль
    /// </summary>
    public bool IsSelected { get; init; }
}