using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class InfluenceDetailsModel : LookupDetails<Guid>
    {
        public int Sequence { get; init; }
    }
}
