using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class IncidentResultListItemModel : LookupListItem<Guid>
    {
        public byte[] RowVersion { get; init; }
    }
}
