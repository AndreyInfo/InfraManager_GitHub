using InfraManager.BLL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.Users
{
    public class UserActivityTypeInputModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public Guid? ParentID { get; set; }

        public byte[] RowVersion { get; set; }

        public ConcludedDetails[] OrganizationItems { get; set; }
    }
}
