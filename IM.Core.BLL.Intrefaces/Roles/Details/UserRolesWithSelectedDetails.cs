using System;

namespace InfraManager.BLL.Roles;

public class UserRolesWithSelectedDetails
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
    
    public bool Selected { get; set; }
}