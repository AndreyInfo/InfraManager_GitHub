using System;

namespace InfraManager.BLL.AccessManagement;

public  class AccessFilter
{
    public Guid OwnerID { get; init; }
    public ObjectClass OwnerClassID { get; init; }

}
