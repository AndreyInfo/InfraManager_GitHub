using System;
using System.Text;
using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.Core.Extensions;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.Services.SearchService;
using Document = InfraManager.Services.SearchService.Document;

namespace InfraManager.UI.Web.AutoMapper
{
    public class LuceneDocumentProfile : Profile // TODO: Этого класса тут быть не должно
    {
        public LuceneDocumentProfile()
        {
            CreateMap<WorkOrder, Document>().ConvertUsing(Map);
            CreateMap<Problem, Document>().ConvertUsing(Map);
            CreateMap<Call, Document>().ConvertUsing(Map);
            CreateMap<NoteDetails[], Document>().ConvertUsing(Map);
            CreateMap<MassIncident, Document>().ConvertUsing(Map);
        }

        private Document Map(MassIncident massiveIncident, Document document)
        {
            document ??= new Document();
            document.Fields.Add(new Field(SearchHelper.LuceneID, massiveIncident.IMObjID.ToString(), false));
            document.Fields.Add(new Field(SearchHelper.LuceneClassID, IMSystem.Global.OBJ_MassIncident.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneName, massiveIncident.Name, true));
            document.Fields.Add(new Field(SearchHelper.LuceneNumber, massiveIncident.ID.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, massiveIncident.Description.Plain, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, massiveIncident.Name, true));
            document.Fields.Add(new Field(SearchHelper.LuceneSolution, massiveIncident.Solution.Plain, true));
            document.Fields.Add(new Field(SearchHelper.LuceneOwnerID, massiveIncident.OwnedBy.IMObjID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneEntityStateName, massiveIncident.EntityStateName ?? string.Empty,
                true));

            document.Fields.Add(new Field(SearchHelper.LuceneProcessed, massiveIncident.EntityStateID.IsNullOrEmpty()
                                                                        && !massiveIncident.WorkflowSchemeID.HasValue
                                                                        && !massiveIncident.WorkflowSchemeVersion.IsNullOrEmpty()
                ? Boolean.TrueString
                : Boolean.FalseString, false));
            document.Fields.Add(new Field(SearchHelper.LuceneInitiatorID, massiveIncident.CreatedBy.IMObjID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneDateModified, massiveIncident.UtcDateModified.Ticks.ToString(),
                true));

            return document;
        }

        private Document Map(Call call, Document document)
        {
            document ??= new Document();
            document.Fields.Add(new Field(SearchHelper.LuceneID, call.IMObjID.ToString(), false));
            document.Fields.Add(new Field(SearchHelper.LuceneClassID, IMSystem.Global.OBJ_CALL.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneNumber, call.Number.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneCallSummaryName, call.CallSummaryName, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, call.Description, true));
            document.Fields.Add(new Field(SearchHelper.LuceneSolution, call.Solution, true));
            document.Fields.Add(new Field(SearchHelper.LuceneClientID, call.ClientID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneOwnerID, call.OwnerID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneExecutorID, call.ExecutorID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneEntityStateName, call.EntityStateName ?? string.Empty,
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField1, call.UserField1, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField2, call.UserField2, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField3, call.UserField3, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField4, call.UserField4, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField5, call.UserField5, true));
            document.Fields.Add(new Field(SearchHelper.LuceneProcessed, call.EntityStateID.IsNullOrEmpty()
                                                                        && !call.WorkflowSchemeID.HasValue
                                                                        && !call.WorkflowSchemeVersion.IsNullOrEmpty()
                ? Boolean.TrueString
                : Boolean.FalseString, false));
            document.Fields.Add(new Field(SearchHelper.LuceneInitiatorID, call.InitiatorID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneDateModified, call.UtcDateModified.Ticks.ToString(),
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneDatePromised, call.UtcDatePromised.Ticks.ToString(),
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneAccomplisherID, call.AccomplisherID.ToStringOrEmpty(),
                true));
            return document;
        }

        private Document Map(Problem problem, Document document)
        {
            document ??= new Document();
            document.Fields.Add(new Field(SearchHelper.LuceneID, problem.IMObjID.ToString(), false));
            document.Fields.Add(new Field(SearchHelper.LuceneClassID, IMSystem.Global.OBJ_PROBLEM.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneNumber, problem.Number.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneName, problem.Summary, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, problem.Description, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, problem.Solution, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, problem.Cause, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, problem.Fix, true));
            document.Fields.Add(new Field(SearchHelper.LuceneOwnerID, problem.OwnerID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneEntityStateName, problem.EntityStateName ?? string.Empty,
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField1, problem.UserField1, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField2, problem.UserField2, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField3, problem.UserField3, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField4, problem.UserField4, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField5, problem.UserField5, true));
            document.Fields.Add(new Field(SearchHelper.LuceneProblemCauseName, problem.ProblemCause?.Name ?? "", true));
            document.Fields.Add(new Field(SearchHelper.LuceneDateModified, problem.UtcDateModified.Ticks.ToString(),
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneDatePromised, problem.UtcDatePromised.Ticks.ToString(),
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneProcessed, problem.EntityStateID.IsNullOrEmpty()
                                                                        && !problem.WorkflowSchemeID.HasValue
                                                                        && !problem.WorkflowSchemeVersion
                                                                            .IsNullOrEmpty()
                ? Boolean.TrueString
                : Boolean.FalseString, false));
            return document;
        }

        private Document Map(WorkOrder workOrder, Document document)
        {
            document ??= new Document();
            document.Fields.Add(new Field(SearchHelper.LuceneID, workOrder.IMObjID.ToString(), false));
            document.Fields.Add(new Field(SearchHelper.LuceneClassID, IMSystem.Global.OBJ_WORKORDER.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneNumber, workOrder.Number.ToString(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneName, workOrder.Name, true));
            document.Fields.Add(new Field(SearchHelper.LuceneDescription, workOrder.Description, true));
            document.Fields.Add(new Field(SearchHelper.LuceneExecutorID, workOrder.ExecutorID.ToStringOrEmpty(), true));
            document.Fields.Add(new Field(SearchHelper.LuceneEntityStateName, workOrder.EntityStateName ?? string.Empty,
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneDatePromised, workOrder.UtcDatePromised.Ticks.ToString(),
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneDateModified, workOrder.UtcDateModified.Ticks.ToString(),
                true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField1, workOrder.UserField1, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField2, workOrder.UserField2, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField3, workOrder.UserField3, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField4, workOrder.UserField4, true));
            document.Fields.Add(new Field(SearchHelper.LuceneUserField5, workOrder.UserField5, true));
            document.Fields.Add(new Field(SearchHelper.LuceneProcessed,
                workOrder.EntityStateID.IsNullOrEmpty()
                && !workOrder.WorkflowSchemeID.HasValue
                && !workOrder.WorkflowSchemeVersion.IsNullOrEmpty()
                    ? Boolean.TrueString
                    : Boolean.FalseString, true));
            document.Fields.Add(
                new Field(SearchHelper.LuceneInitiatorID, workOrder.InitiatorID.ToStringOrEmpty(), false));
            document.Fields.Add(new Field(SearchHelper.LuceneAssignerID, workOrder.AssigneeID.ToStringOrEmpty(), true));
            return document;
        }

        private Document Map(NoteDetails[] details, Document document)
        {
            document ??= new Document();
            if (details.Length != 0)
            {
                var notes = new StringBuilder();
                foreach (var note in details)
                    notes.AppendFormat("{0};", note.Message);
                document.Fields.Add(new Field(SearchHelper.LuceneSDNote, notes.ToString(), true));
            }

            return document;
        }
    }
}