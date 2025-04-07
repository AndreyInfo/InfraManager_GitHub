using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemDetails : IUserFieldsModel, ISDEntityWithPriorityColorInt
    {
        public Guid ID { get; init; }
        public ObjectClass ClassID => ObjectClass.Problem;
        public string FullName => $"IM-PL-{Number}";
        public int Number { get; init; }
        public string UrgencyID { get; init; }
        public string UrgencyName { get; init; }
        public string InfluenceID { get; init; }
        public string InfluenceName { get; init; }
        public Guid PriorityID { get; init; }
        public string PriorityName { get; init; }
        public int PriorityColor { get; init; }
        public Guid TypeID { get; init; }
        public string TypeName { get; init; }
        public string Summary { get; init; }
        public string Description { get; init; }
        public string OwnerID { get; init; }
        public string UtcDateDetected { get; init; }
        public string UtcDatePromised { get; set; }
        public string UtcDateClosed { get; init; }
        public string UtcDateSolved { get; init; }
        public string UtcDateModified { get; init; }
        public string ProblemCauseID { get; init; }
        public string ProblemCauseName { get; init; }
        public string Solution { get; init; }
        public string Cause { get; init; }
        public string Fix { get; init; }
        public int WorkOrderCount { get; set; }
        public Guid[] CallIds { get; init; }
        public int CallCount => CallIds.Count();
        public int DependencyObjectCount { get; init; }
        public int NegotiationCount { get; init; }
        public int UnreadNoteCount { get; set; }
        public string EntityStateID { get; init; }
        public string EntityStateName { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public string WorkflowSchemeVersion { get; init; }
        public string WorkflowSchemeID { get; init; }
        public int ManhoursInMinutes { get; init; }
        public int ManhoursNormInMinutes { get; init; }
        public bool OnWorkOrderExecutorControl { get; init; }
        public int NoteCount { get; init; }
        public FormValuesDetailsModel FormValues { get; init; }
        public string HTMLFix { get; init; }
        public string HTMLDescription {get; init;}
        public string HTMLCause { get; init;}
        public string HTMLSolution { get; init; }
        public long? FormValuesID { get; init; }
        public string InitiatorID { get; init; }
        public string ExecutorID { get; init; }
        public string QueueID { get; init; }
        public string QueueName { get; set; }
        public string ServiceID { get; init; }
        public string ServiceCategoryName { get; set; }
        public string ServiceName { get; set; }
        public async Task SetUserFieldNames(IUserFieldNameBLL nameProvider)
        {
            UserField1Name = await nameProvider.GetNameAsync(FieldNumber.UserField1);
            UserField2Name = await nameProvider.GetNameAsync(FieldNumber.UserField2);
            UserField3Name = await nameProvider.GetNameAsync(FieldNumber.UserField3);
            UserField4Name = await nameProvider.GetNameAsync(FieldNumber.UserField4);
            UserField5Name = await nameProvider.GetNameAsync(FieldNumber.UserField5);
        }

        public int MassIncidentCount { get; set; }
    }
}
