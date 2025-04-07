using System;

namespace InfraManager.BLL.Roles;

public class OperationModelForRole<TID> where TID : struct
{
    public Guid RoleID { get; init; }

    public TID OperationID { get; init; }

    public string Name { get; set; }
}
