using System;

namespace InfraManager.WebApi.Contracts.OrganizationStructure
{
    public class DeputyUserDetailsModel
    {
        public Guid ID { get; set; }

        public Guid ParentUserID { get; set; }

        public string ParentUserFullName { get; set; }

        public Guid ChildUserID { get; set; }

        public string ChildUserFullName { get; set; }

        // TODO: должно стать UtcDeputySince в будущем
        public DateTime UtcDataDeputyWith { get; set; }

        // TODO: должно стать UtcDeputyUntil в будущем
        public DateTime UtcDataDeputyBy { get; set; }

        public string UtcDataDeputyBySt { get; set; }

        public string UtcDataDeputyWithSt { get; set; }
    }
}