using System;

namespace InfraManager.BLL.UsersActivityType;

public class UserActivityTypeReferenceDetails
{
    public Guid ID { get; set; }
    public Guid ObjectID { get; set; }
    public ObjectClass ObjectClassID { get; set; }
}