using System;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    public class NullWorkOrderReference : WorkOrderReference
    {
        protected NullWorkOrderReference() : base(Guid.Empty, ObjectClass.Unknown, string.Empty)
        {
        }
    }
}
