using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class Concordance
    {
        public Guid UrgencyId { get; init; }
        public Guid InfluenceId { get; init; }
        public Guid PriorityID { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Influence Influence { get; init; }
    }
}
