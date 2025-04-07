using System;

namespace InfraManager.BLL.ServiceDesk
{
    public class ConcordanceModel
    {
        public Guid UrgencyId { get; init; }
        public Guid InfluenceId { get; init; }
        public Guid PriorityId { get; init; }
    }
}
