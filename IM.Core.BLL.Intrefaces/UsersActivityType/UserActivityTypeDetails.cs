using System;

namespace InfraManager.BLL.UsersActivityType;

public class UserActivityTypeDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? ParentID { get; init; }
    public UserActivityTypeReferenceDetails[] References { get; init; }
}