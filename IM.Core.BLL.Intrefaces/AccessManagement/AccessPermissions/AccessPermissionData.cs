using System;

namespace InfraManager.BLL.AccessManagement.AccessPermissions;

public class AccessPermissionData
{
    public Guid ObjectID { get; init; }

    public ObjectClass ClassID { get; init; }

    public Guid OwnerID { get; init; }

    public AccessPermissionRightsDetails Rights { get; init; } 
}
