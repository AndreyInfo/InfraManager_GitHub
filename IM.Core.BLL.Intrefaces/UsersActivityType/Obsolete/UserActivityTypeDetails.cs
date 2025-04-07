using System;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    [Obsolete("Use InfraManager.BLL.UsersActivityType.UserActivityTypeDetails instead")]
    public class UserActivityTypeDetails : UserActivityTypeBaseDetails
    {
        public Guid? ParentID { get; init; }

        public byte[] RowVersion { get; init; }
    }
}
