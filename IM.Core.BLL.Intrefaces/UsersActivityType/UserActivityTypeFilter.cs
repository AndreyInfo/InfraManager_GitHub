using System;

namespace InfraManager.BLL.UsersActivityType;

public class UserActivityTypeFilter
{
    public Guid? ParentID { get; init; }
    public ObjectClass[] ReferencedObjectClasses { get; init; }
    public Guid? ReferencedObjectID { get; init; }
    public bool OnlyCurrentUser { get; init; } = true;
}