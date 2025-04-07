using InfraManager.DAL;
using System;

namespace InfraManager.BLL.Search
{
    public class UserSearchClientCriteria : SearchCriteria
    {
        public Guid? UserId { get; init; }
        public Guid? QueueId { get; init; }
        public bool? NoTOZ { get; init; }
        public Guid[] ExceptUserIDs { get; init; }
        public Guid? SubdivisionID { get; init; }
        public bool? MOL { get; init; }
        public Guid? OrganizationID { get; init; }
        public Guid? ControlsObjectID { get; init; }
        public ObjectClass? ControlsObjectClassID { get; init; }
        public bool ControlsObjectValue { get; init; }
        public OperationID[] OperationIds { get; init; }
    }
}
