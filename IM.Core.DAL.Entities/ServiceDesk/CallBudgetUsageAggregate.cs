using System;

namespace InfraManager.DAL
{
    public class CallBudgetUsageAggregate
    {
        public Guid ID { get; set; }
        public string FullName { get; set; }
        public Guid? BudgetID { get; set; }
        public Guid? BudgetObjectID { get; set; }
        public ObjectClass? BudgetObjectClassID { get; set; }

    }
}
