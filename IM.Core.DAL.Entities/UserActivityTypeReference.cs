using System;

namespace InfraManager.DAL;

public class UserActivityTypeReference
{
    public UserActivityTypeReference()
    {
        ID = Guid.NewGuid();
    }
    public Guid ID { get; init; }
    public Guid UserActivityTypeID { get; init; }
    public Guid ObjectID { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public Guid? ReferenceObjectID { get; init; }
    public ObjectClass? ReferenceClassID { get; init; }
    public virtual UserActivityType Type { get; init; }
}
