using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems
{
    public class ProblemDetailsModel : IUserFieldsModel
    {
        public Guid ID { get; set; }
        public int ClassID { get; set; }
        public string FullName { get; set; }
        public int Number { get; set; }
        public string UrgencyID { get; set; }
        public string UrgencyName { get; set; }
        public string InfluenceID { get; set; }
        public string InfluenceName { get; set; }
        public Guid PriorityID { get; set; }
        public string PriorityName { get; set; }
        public Guid TypeID { get; set; }
        public string TypeName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string HTMLDescription { get; set; }
        public string OwnerID { get; set; }
        public string UtcDateDetected { get; set; }
        public string UtcDatePromised { get; set; }
        public string UtcDateClosed { get; set; }
        public string UtcDateSolved { get; set; }
        public string UtcDateModified { get; set; }
        public string ProblemCauseID { get; set; }
        public string ProblemCauseName { get; set; }
        public string Solution { get; set; }
        public string HTMLSolution { get; set; }
        public string Cause { get; set; }
        public string HTMLCause { get; set; }
        public string Fix { get; set; }
        public string HTMLFix { get; set; }
        public int WorkOrderCount { get; set; }
        public string[] Calls { get; set; }
        public int CallCount { get; set; }
        public int DependencyObjectCount { get; set; }
        public int NegotiationCount { get; set; }
        public int UnreadNoteCount { get; set; }
        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public string WorkflowSchemeIdentifier { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        public string WorkflowSchemeID { get; set; }
        public int ManhoursInMinutes { get; set; }
        public int ManhoursNormInMinutes { get; set; }
        public bool OnWorkOrderExecutorControl { get; set; }
        public int NoteCount { get; set; }
        public string ManhoursString { get; set; }
        public string ManhoursNormString { get; set; }
        public string PriorityColor { get; set; }
        public FormValuesDetailsModel FormValues { get; set; }
        public string InitiatorID { get; init; }
        public string ExecutorID { get; init; }
        public string QueueID { get; init; }
        public string QueueName { get; init; }
        public string ServiceID { get; init; }
        public string ServiceCategoryName { get; init; }
        public string ServiceName { get; init; }
        public int MassIncidentCount { get;init; }
    }
}
