using Inframanager;
using System;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class ChangeRequestCategory
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public bool Removed { get; set; }
    }
}
