using System;

namespace InfraManager.DAL.OrganizationStructure
{
    public class GroupUser
    {
        public Guid UserID { get; init; }

        public virtual User User { get; }

        public Guid GroupID { get; init; }
    }
}
