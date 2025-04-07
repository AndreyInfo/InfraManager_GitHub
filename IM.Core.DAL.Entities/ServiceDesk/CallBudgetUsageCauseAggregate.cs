using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class CallBudgetUsageCauseAggregate
    {
        public Guid ID { get; set; }
        public Guid? SlaID { get; set; }
        public string Name { get; set; }
    }
}
