using System;

namespace InfraManager.BLL.AccessManagement.AccessPermissions
{
    public class AccessPermissionDetails
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public ObjectClass OwnerClassId { get; set; }

        public ObjectClass ObjectClassId { get; set; }

        public Guid ObjectId { get; set; }

        public AccessPermissionRightsDetails Rights { get; init; }

    }
}
