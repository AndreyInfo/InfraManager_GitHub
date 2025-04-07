using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class PriorityMatrixCell
    {
        public Guid UrgencyID { get; set; }
        public Guid InfluenceID { get; set; }
        public Guid PriorityID { get; set; }

        public virtual Priority Priority { get; set; }
        public virtual Influence Influence { get; set; }
        public virtual Urgency Urgency { get; set; }
    }
}
