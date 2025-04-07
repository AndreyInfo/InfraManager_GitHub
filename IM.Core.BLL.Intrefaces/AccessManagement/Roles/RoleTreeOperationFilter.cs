using System;

namespace InfraManager.BLL.AccessManagement;

public class RoleTreeOperationFilter
{
    public ObjectClass? ClassID { get; init; }
    public Guid RoleID { get; init; }
}
