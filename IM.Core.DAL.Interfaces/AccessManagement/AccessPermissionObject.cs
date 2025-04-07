using System;

namespace InfraManager.DAL.AccessManagement;
public class AccessPermissionObject
{
    public Guid OwnerID { get; init; }
    public ObjectClass OwnerClassID { get; init; }
    public Guid ObjectID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
}