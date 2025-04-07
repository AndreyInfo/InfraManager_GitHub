using System;
using Inframanager;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    [ObjectClassMapping(ObjectClass.ChangeRequestType)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.RfcType_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.RfcType_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.RfcType_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.RfcType_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.RfcType_Properties)]
    public class ChangeRequestType : IMarkableForDelete, IFormBuilder
    {
        public Guid ID { get; init; }
        
        public Guid? FormID { get; set; }

        public virtual Form Form { get; }

        public string Name { get; set; }
        
        public string WorkflowSchemeIdentifier { get; set; }

        public bool Removed { get; private set; }

        public byte[] RowVersion { get; set; }
        
        public byte[] Icon { get; set; }
        
        public string IconName { get; set; }
        
        public void MarkForDelete()
        {
            Removed = true;
        }
    }
}
