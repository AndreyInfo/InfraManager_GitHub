using System;

namespace InfraManager.BLL.Messages
{
    public class MessageByEmailDetails
    {
        public string From { get; init; }
        public string To { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public string HtmlContent { get; init; }
        public string MessageMimeId { get; init; }
        public DateTime? UtcDateReceived { get; init; }

        public bool IsDuplicate { get; set; }

        public Guid ID { get; set; }
        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }

    }
}
