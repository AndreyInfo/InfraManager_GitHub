using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class InfluenceListItemModel : LookupListItem<Guid>
    {
        public int Sequence { get; init; }
    }
}
