using System;

namespace InfraManager.BLL.Messages
{
    public class MessageByEmailData
    {
        public string From { get; init; }
        public string To { get; init; }
        public string Title { get; init; }
        
        public string Content { get; init; }
        
        public string HtmlContent { get; init; }
        public string MessageMimeId { get; init; }
        public DateTime? UtcDateReceived { get; init; }
        public byte SeverityType { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public Guid[] AttachmentIDs { get; init; }

        public DateTime? UtcDateModified { get; set; }
        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        
        public string TargetEntityStateID { get; set; }


    }
}
