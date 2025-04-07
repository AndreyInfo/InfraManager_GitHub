using System;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    public class UserActivityTypePathDetails
    {
        public Guid ID { get; init; }
        public string Path { get; init;}
        public virtual UserActivityTypeBaseDetails[] Pathes { get; init;}
    }
}
