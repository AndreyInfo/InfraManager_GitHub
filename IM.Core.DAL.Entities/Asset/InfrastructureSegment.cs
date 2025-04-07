using System;
using Inframanager;

namespace InfraManager.DAL.Asset
{
    [OperationIdMapping(ObjectAction.Insert, OperationID.InfrastructureSegment_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.InfrastructureSegment_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.InfrastructureSegment_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.InfrastructureSegment_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.InfrastructureSegment_Properties)]
    public class InfrastructureSegment : Catalog<Guid>
    {
        public byte[] RowVersion { get; set; }
    }
}
