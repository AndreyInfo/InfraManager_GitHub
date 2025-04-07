using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Settings.UserFields;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemProfile : Profile
    {
        public ProblemProfile()
        {
            CreateMap<Problem, ProblemDetails>()
                .ForMember(
                    model => model.ID,
                    mapper => mapper.MapFrom(entity => entity.IMObjID))
                .ForMember(
                    model => model.PriorityColor,
                    mapper => mapper.MapFrom(entity => entity.Priority.Color))
                .ForMember(
                    model => model.DependencyObjectCount,
                    mapper => mapper.MapFrom(entity => entity.Dependencies.Count()))
                .ForMember(
                    model => model.NegotiationCount,
                    mapper => mapper.MapFrom(entity => entity.Negotiations.Count()))
                .ForMember(
                    model => model.CallIds,
                    mapper => mapper.MapFrom(entity => entity.CallReferences.Select(x => x.CallID).ToArray()))
                .ForMember(
                    model => model.NoteCount,
                    mapper => mapper.MapFrom(entity => entity.Notes.Count()))
                .ForMember(
                    details => details.UserField1Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Problem, ProblemDetails>,
                        UserField>(_ => new UserField(UserFieldType.Problem, FieldNumber.UserField1)))
                .ForMember(
                    details => details.UserField2Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Problem, ProblemDetails>,
                        UserField>(_ => new UserField(UserFieldType.Problem, FieldNumber.UserField2)))
                .ForMember(
                    details => details.UserField3Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Problem, ProblemDetails>,
                        UserField>(_ => new UserField(UserFieldType.Problem, FieldNumber.UserField3)))
                .ForMember(
                    details => details.UserField4Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Problem, ProblemDetails>,
                        UserField>(_ => new UserField(UserFieldType.Problem, FieldNumber.UserField4)))
                .ForMember(
                    details => details.UserField5Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Problem, ProblemDetails>,
                        UserField>(_ => new UserField(UserFieldType.Problem, FieldNumber.UserField5)))
                .ForMember(
                    details => details.HTMLDescription,
                    mapper => mapper.MapFrom(entity => entity.HTMLDescription ?? entity.Description))
                .ForMember(
                    details => details.HTMLSolution,
                    mapper => mapper.MapFrom(entity => entity.HTMLSolution ?? entity.Solution))
                .ForMember(
                    details => details.HTMLCause,
                    mapper => mapper.MapFrom(entity => entity.HTMLCause ?? entity.Cause))
                .ForMember(
                    details => details.HTMLFix,
                    mapper => mapper.MapFrom(entity => entity.HTMLFix ?? entity.Fix))
                 .ForMember(
                    entity => entity.FormValues,
                    mapping => mapping.MapFrom(data => data.FormValues));

            CreateMap<ProblemData, Problem>()
                .ConstructUsing(data => new Problem(data.TypeID.Value))
                .ForMember(
                    entity => entity.Description,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLDescription != null);
                        mapping.MapFrom(data => data.HTMLDescription.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLDescription,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLDescription != null);
                        mapping.MapFrom(data => data.HTMLDescription);
                    })
                .ForMember(
                    entity => entity.Solution,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLSolution != null);
                        mapping.MapFrom(data => data.HTMLSolution.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLSolution,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLSolution != null);
                        mapping.MapFrom(data => data.HTMLSolution);
                    })
                .ForMember(
                    entity => entity.Cause,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLCause != null);
                        mapping.MapFrom(data => data.HTMLCause.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLCause,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLCause != null);
                        mapping.MapFrom(data => data.HTMLCause);
                    })
                .ForMember(
                    entity => entity.Fix,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLFix != null);
                        mapping.MapFrom(data => data.HTMLFix.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLFix,
                    mapping =>
                    {
                        mapping.Condition(data => data.HTMLFix != null);
                        mapping.MapFrom(data => data.HTMLFix);
                    })
                .ForNullableProperty(x => x.ProblemCauseID)
                .ForNullableProperty(x => x.OwnerID)
                .ForMember(
                    entity => entity.UtcDateDetected,
                    mapping => mapping
                        .MapFrom(
                            (data, entity) => string.IsNullOrEmpty(data.UtcDateDetected)
                                ? entity.UtcDateDetected
                                : data.UtcDateDetected.ConvertFromMillisecondsAfterMinimumDate()))
                .ForMember(
                    entity => entity.UtcDatePromised,
                    mapping => mapping
                        .MapFrom(
                            (data, entity) => string.IsNullOrEmpty(data.UtcDatePromised)
                                ? entity.UtcDatePromised
                                : data.UtcDatePromised.ConvertFromMillisecondsAfterMinimumDate()))
                .ForMember(
                    entity => entity.FormValues,
                    opt => {
                        opt.PreCondition(src => src.FormValuesData is not null);
                        opt.MapFrom(data => data.FormValuesData);
                    })
                .ForMember(
                    entity => entity.EntityStateID,
                    mapping => mapping.MapFrom((data, entity) => {
                        if (data.EntityStateIDIsNull)
                            return null;
                        else if (entity.EntityStateID == null)
                            return data.EntityStateID;
                        else if (entity.EntityStateID != null && data.EntityStateID != null)
                            return data.EntityStateID;
                        else
                            return entity.EntityStateID;
                    }))
                .ForMember(
                    entity => entity.WorkflowSchemeID,
                    mapping => mapping.MapFrom(data => data.WorkflowSchemeID))
                .ForNullableProperty(x => x.InitiatorID)
                .ForNullableProperty(x => x.ExecutorID)
                .ForNullableProperty(x => x.QueueID)
                .ForNullableProperty(x => x.ServiceID)
                .IgnoreOtherNulls();

            this.CreateProblemReferenceMap<ProblemReferenceListItem>();
        }
    }
}