using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public class DeputyUserData
    {
        public string UtcDeputySince { get; init; }

        public string UtcDeputyUntil { get; init; }

        public Guid ChildUserID { get; init; }

        public Guid ParentUserID { get; init; }
    }
}