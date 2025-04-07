using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class UrgencyListItemModel : LookupListItem<Guid>
    {
        public int Sequence { get; init; }
    }
}
