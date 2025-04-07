using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public class OrganizationItemGroupData
    {
        public Guid ID { get; init; }
        public Guid ItemID { get; init; }
        public ObjectClass ItemClassID { get; init; }
    }
}
