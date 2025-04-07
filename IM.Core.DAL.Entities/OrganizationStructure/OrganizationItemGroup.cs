using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.DAL.OrganizationStructure
{
    public class OrganizationItemGroup
    {
        protected OrganizationItemGroup()
        {
            
        }

        public OrganizationItemGroup(Guid id, Guid itemID, ObjectClass itemClassID)
        {
            ID = id;
            ItemID = itemID;
            ItemClassID = itemClassID;
        }
        
        public OrganizationItemGroup(Guid id, ObjectClass itemClassID)
        {
            ID = id;
            ItemClassID = itemClassID;
        }
        
        public Guid ID { get; init; }
        public Guid ItemID { get; init; }
        public ObjectClass ItemClassID { get; init; }
        
        public virtual ServiceUnit ServiceUnit { get; set; }
        public virtual AccessPermission AccessPermission { get; init; }
    }
}
