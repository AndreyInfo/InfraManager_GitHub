using InfraManager.BLL.Asset;
using System;

namespace InfraManager.BLL.ServiceDesk.DTOs
{
    public class ConcordanceDetails
    {
        public Guid InfluenceId { get; init; }

        public Guid UrgencyId { get; init; }

        public Guid PriorityId { get; init; }

        public PriorityDetailsModel Priority {get; init;}
    }
}
