using System;

namespace InfraManager.DAL.AccessManagement;

public class AccessPermissionModelItem
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public Guid OwnerID { get; init; }

    public ObjectClass OwnerClassID { get; init; }

    public ObjectClass ObjectClassID { get; init; }

    public Guid ObjectID { get; init; }

    public bool? Properties { get; init; }

    public bool? Add { get; init; }

    public bool? Delete { get; init; }

    public bool? Update { get; init; }

    public bool? AccessManage { get; init; }
}
