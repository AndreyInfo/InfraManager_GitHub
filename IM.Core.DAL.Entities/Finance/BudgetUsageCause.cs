using System;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.DAL.Finance
{
    public class BudgetUsageCause
    {
        protected BudgetUsageCause()
        {
        }

        public BudgetUsageCause(Guid objectId, ObjectClass objectClass) : this()
        {
            ObjectId = objectId;
            ObjectClass = objectClass;
        }

        public Guid ObjectId { get; private set; }
        public ObjectClass ObjectClass { get; private set; }
        public Guid? SLAID { get; set; }
        public virtual ServiceLevelAgreement Sla { get; set; }
        public string Text { get; set; }
    }
}
