using Inframanager;
using System;

namespace InfraManager.DAL.Message
{
    [ObjectClassMapping(ObjectClass.MessageByEmail)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.MessageByEmail_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.MessageByEmail_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.MessageByEmail_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.MessageByEmail_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.MessageByEmail_Properties)]
    public class MessageByEmail : IWorkflowEntity
    {
        protected MessageByEmail() { }
        public MessageByEmail(Message message)
        {
            Message = message;
            IMObjID = message.IMObjID;
        }

        public Guid IMObjID { get; init; }
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HtmlContent { get; set; }
        public string MessageMimeId { get; set; }
        public DateTime UtcDateReceived { get; set; }
        public virtual Message Message { get; set; }
        public DateTime UtcDateModified { get => Message.UtcDateModified; set => Message.UtcDateModified = value; }
        public string EntityStateID { get => Message.EntityStateID; set => Message.EntityStateID = value; }
        public string EntityStateName { get => Message.EntityStateName; set => Message.EntityStateName = value; }
        public Guid? WorkflowSchemeID { get => Message.WorkflowSchemeID; set => Message.WorkflowSchemeID = value; }
        public string WorkflowSchemeVersion { get => Message.WorkflowSchemeVersion; set => Message.WorkflowSchemeVersion = value; }
        public string WorkflowSchemeIdentifier { get => Message.WorkflowSchemeIdentifier; set => Message.WorkflowSchemeIdentifier = value; }
        public string TargetEntityStateID { get => Message.TargetEntityStateID; set => Message.TargetEntityStateID = value; }
    }
}
