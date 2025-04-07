using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public class DeputyUserDetails
    {
        public Guid ID { get; init; }

        public Guid ParentUserID { get; init; }

        public string ParentFullName { get; init; }

        public Guid ChildUserID { get; init; }
        public string ChildFullName { get; init; }

        // TODO: должно стать UtcDeputySince в будущем
        public DateTime UtcDataDeputyWith { get; init; }

        // TODO: должно стать UtcDeputyUntil в будущем
        public DateTime UtcDataDeputyBy { get; init; }
    }
}