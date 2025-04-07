using Inframanager;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;

namespace InfraManager.DAL.ServiceDesk
{
    [ObjectClassMapping(ObjectClass.WorkOrderType)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.WorkOrderType_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.WorkOrderType_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.WorkOrderType_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.WorkOrderType_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.WorkOrderType_Properties)]
    public class WorkOrderType : Lookup, IMarkableForDelete
    {
        public int Color { get; set; }

        public bool Default { get; set; }        

        public string WorkflowSchemeIdentifier { get; set; }

        public WorkOrderTypeClass TypeClass { get; set; }

        public Guid? FormID { get; set; }

        public string IconName { get; set; }

        public virtual WorkFlowScheme WorkFlowScheme { get; }

        public bool Removed { get; private set; }
        public void MarkForDelete()
        {
            Removed = true;
        }
    }
}
