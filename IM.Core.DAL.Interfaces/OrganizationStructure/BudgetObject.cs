using System;

namespace InfraManager.DAL.OrganizationStructure
{
    public class BudgetObject
    {
        public Guid ID { get; init; }
        public ObjectClass ClassID { get; init; }
        public string Name { get; init; }
    }
}
