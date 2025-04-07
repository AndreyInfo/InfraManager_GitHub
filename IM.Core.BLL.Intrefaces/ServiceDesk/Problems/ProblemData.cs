using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.FormDataValue;
using System;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemData : IServiceDeskEntityData
    {
        public Guid? UrgencyID { get; init; }
        public Guid? InfluenceID { get; init; }
        public Guid? PriorityID { get; init; }
        public Guid? TypeID { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public NullablePropertyWrapper<Guid> OwnerID { get; init; }
        public string UtcDatePromised { get; init; }
        public string UtcDateClosed { get; init; }
        public string UtcDateSolved { get; init; }
        public string UtcDateDetected { get; init; }
        public NullablePropertyWrapper<Guid> ProblemCauseID { get; init; }
        public string Solution { get; init; }
        public string Cause { get; init; }
        public string Fix { get; init; }
        public string HTMLSolution { get; init; }
        public string HTMLCause { get; init; }
        public string HTMLFix { get; init; }
        public string HTMLDescription { get; init; }
        public string EntityStateID { get; init; }
        public bool EntityStateIDIsNull { get; init; }
        public string EntityStateName { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public NullablePropertyWrapper<Guid> WorkflowSchemeID { get; init; }
        public int? ManhoursNormInMinutes { get; init; }
        public int? ManhoursInMinutes { get; init; }
        public bool? OnWorkOrderExecutorControl { get; init; }
        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }
        public FormValuesData FormValuesData { get; init; }
        public NullablePropertyWrapper<Guid> InitiatorID { get; init; }
        public NullablePropertyWrapper<Guid> ExecutorID { get; init; }
        public NullablePropertyWrapper<Guid> QueueID { get; init; }
        public NullablePropertyWrapper<Guid> ServiceID { get; init; }
    }
}
