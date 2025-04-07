using System;
using Inframanager;

namespace InfraManager.DAL.ServiceDesk
{
    
    [ObjectClassMapping(ObjectClass.KnowledgeBaseArticleStatus)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.KBArticleStatus_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.KBArticleStatus_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.KBArticleStatus_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.KBArticleStatus_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.KBArticleStatus_Properties)]
    public class KnowledgeBaseArticleStatus
    {
        public Guid ID { get; init; }

        public string Name { get; set; }
        
        public byte[] RowVersion { get; set; }
    }
}
