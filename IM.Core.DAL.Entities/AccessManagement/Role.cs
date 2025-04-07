using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.AccessManagement;

/// <summary>
/// Этот класс представляет сущность Роль
/// </summary>
[ObjectClassMapping(ObjectClass.Role)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Role_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Role_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Role_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Role_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Role_Properties)]
public class Role : Catalog<Guid>
{
    public static Guid AdminRoleId = new("00000000-0000-0000-0000-000000000001");

    public Role(string name)
    {
        ID = Guid.NewGuid();
        Name = name;
    }

    public Role()
    {
    }


    /// <summary>
    /// Возвращает или задает описание роли
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Возвращает идентификатор версии роли
    /// </summary>
    public byte[] RowVersion { get; init; }

    public bool IsAdmin => ID == AdminRoleId;

    public virtual ICollection<RoleOperation> Operations { get; set; }
    public virtual ICollection<RoleLifeCycleStateOperation> LifeCycleStateOperations { get; set; }
}
