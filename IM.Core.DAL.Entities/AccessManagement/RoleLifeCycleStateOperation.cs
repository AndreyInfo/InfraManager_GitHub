using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;

namespace InfraManager.DAL.AccessManagement
{
    public class RoleLifeCycleStateOperation
    {
        protected RoleLifeCycleStateOperation()
        {
            
        }
        
        public RoleLifeCycleStateOperation(Guid roleID, Guid stateID)
        {
            RoleID = roleID;
            LifeCycleStateOperationID = stateID;
        }
        
        public Guid RoleID { get; set; }

        public Guid LifeCycleStateOperationID { get; set; }

        public virtual Role Role { get; }

        public virtual LifeCycleStateOperation LifeCycleStateOperation { get; }
    }
}
