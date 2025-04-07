using System;

namespace InfraManager.DAL.Search
{
    public class UserSearchCriteria : SearchCriteria
    {
        public Guid? UserId { get; init; }
        public OperationID[] Operations { get; init; }
        public bool HasAnyNonAdministrativeRole { get; init; }
        public Guid? QueueId { get; init; }
        public bool NoTOZ { get; init; }
        public Guid[] ExceptUserIDs { get; init; }
        public Guid? SubdivisionID { get; init; }
        public bool MOL { get; init; }
        public Guid? OrganizationID { get; init; }
        public Guid? ControlsObjectID { get; init; }
        public ObjectClass? ControlsObjectClassID { get; init; }
        public bool ControlsObjectValue { get; init; }
        public bool UseTTZ { get; init; }
    }
}
