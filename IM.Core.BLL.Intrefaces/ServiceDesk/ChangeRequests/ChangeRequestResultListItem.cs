using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests;

public class ChangeRequestResultListItem : LookupListItem<Guid>
{
    public byte[] RowVersion { get; init; }
}
