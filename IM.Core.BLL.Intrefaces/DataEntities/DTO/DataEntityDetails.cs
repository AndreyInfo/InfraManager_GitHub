using System;

namespace InfraManager.BLL.DataEntities.DTO
{
    public class DataEntityDetails
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public int? OrganizationItemClassID { get; init; }
        public Guid? OrganizationItemID { get; init; }
    }
}
