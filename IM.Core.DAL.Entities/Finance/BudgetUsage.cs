using System;

namespace InfraManager.DAL.Finance
{
    public class BudgetUsage
    {
        protected BudgetUsage()
        {
        }

        public BudgetUsage(Guid objectId, ObjectClass objectClass)
        {
            ObjectId = objectId;
            ObjectClass = objectClass;
        }

        public Guid ObjectId { get; private set; }
        public ObjectClass ObjectClass { get; private set; }
        public Guid? BudgetId { get; set; }
        public Guid? BudgetObjectId { get; set; }
        public ObjectClass? BudgetObjectClass { get; set; }

        public static string GetBudgetFullName(Guid? budgetId) => throw new NotSupportedException();
    }
}
