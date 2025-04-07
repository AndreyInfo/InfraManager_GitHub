using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Settings.UserFields;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallProfile : Profile
    {
        public CallProfile()
        {
            CreateMap<CallData, Call>()
                .ConstructUsing(model => new Call(model.ReceiptType.Value))
                .ForMember(
                    entity => entity.ReceiptType,
                    mapping =>
                    {
                        mapping.MapFrom((data, source) => data.ReceiptType.HasValue ? data.ReceiptType.Value : source.ReceiptType);
                    })
                .ForMember(
                    entity => entity.Description,
                    mapping =>
                    {
                        mapping.Condition(data => data.Description != null);
                        mapping.MapFrom(data => data.Description.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLDescription,
                    mapping =>
                    {
                        mapping.Condition(data => data.Description != null);
                        mapping.MapFrom(data => data.Description);
                    })
                .ForMember(
                    entity => entity.Solution,
                    mapping =>
                    {
                        mapping.Condition(data => data.Solution != null);
                        mapping.MapFrom(data => data.Solution.RemoveHtmlTags());
                    })
                .ForMember(
                    entity => entity.HTMLSolution,
                    mapping =>
                    {
                        mapping.Condition(data => data.Solution != null);
                        mapping.MapFrom(data => data.Solution);
                    })
                .ForMember(
                    entity => entity.UtcDatePromised,
                    mapping => mapping.MapFrom(data => data.UtcDatePromisedMilliseconds))
                .ForMember(
                    entity => entity.EntityStateID,
                    mapping => mapping.MapFrom((data, entity) => {
                        if (data.EntityStateIDIsNull)
                            return null;
                        else if (entity.EntityStateID == null)
                           return data.EntityState;
                        else if (entity.EntityStateID != null && data.EntityState != null)
                            return data.EntityState;
                        else
                            return entity.EntityStateID;
                    }))
                .ForNullableProperty(data => data.ExecutorID)
                .ForMember(
                    entity => entity.OwnerID,
                    mapper => mapper.MapFrom(data => data.OwnerID)) // Добавлено чтобы не игнорировались NULL
                .ForMember(
                    entity => entity.AccomplisherID,
                    mapper => mapper.MapFrom(data => data.AccomplisherID)) // Добавлено чтобы не игнорировались NULL
                .ForMember(entity => entity.QueueID,
                    mapper => mapper.MapFrom(data => data.QueueID)) // Добавлено чтобы не игнорировались NULL
                .ForMember(
                    entity => entity.FormValues,
                    opt => {
                        opt.PreCondition(src => src.FormValuesData is not null);
                        opt.MapFrom(data => data.FormValuesData);
                    })
                .ForMember(
                    entity => entity.WorkflowSchemeID,
                    mapping => mapping.MapFrom(data => data.WorkflowSchemeID))
                .IgnoreOtherNulls();

            CreateMap<Call, CallDetails>()
                .ForMember(model => model.ID, mapper => mapper.MapFrom(call => call.IMObjID))
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom(call => call.Priority.Color))
                .ForMember(
                    model => model.WorkflowSchemeID,
                    mapper => mapper.MapFrom(entity =>
                        entity.WorkflowSchemeID.HasValue ? entity.WorkflowSchemeID.ToString() : string.Empty))
                .ForMember(
                    model => model.UtcDateAccomplished,
                    mapper => mapper.MapFrom(entity =>
                        entity.UtcDateAccomplished.ConvertToMillisecondsAfterMinimumDate()))
                .ForMember(
                    model => model.UtcDateClosed,
                    mapper => mapper.MapFrom(entity => entity.UtcDateClosed.ConvertToMillisecondsAfterMinimumDate()))
                .ForMember(
                    model => model.UtcDateModified,
                    mapper => mapper.MapFrom(entity => entity.UtcDateModified.ConvertToMillisecondsAfterMinimumDate()))
                .ForMember(
                    model => model.UtcDateOpened,
                    mapper => mapper.MapFrom(entity => entity.UtcDateOpened.ConvertToMillisecondsAfterMinimumDate()))
                .ForMember(
                    model => model.UtcDatePromised,
                    mapper => mapper.MapFrom(entity => entity.UtcDatePromised.ConvertToMillisecondsAfterMinimumDate()))
                .ForMember(
                    model => model.UtcDateRegistered,
                    mapper => mapper.MapFrom(entity =>
                        entity.UtcDateRegistered.ConvertToMillisecondsAfterMinimumDate()))
                .ForMember(
                    model => model.Grade,
                    mapper => mapper.MapFrom(entity => entity.Grade.ToString()))
                .ForMember(
                    model => model.ServiceID,
                    mapper => mapper.MapFrom(entity => entity.CallService.Service.ID))
                .ForMember(
                    model => model.ServiceCategoryName,
                    mapper => mapper.MapFrom(entity => entity.CallService.Service.Category.Name))
                .ForMember(
                    model => model.ServiceName,
                    mapper => mapper.MapFrom(entity => entity.CallService.Service.Name))
                .ForMember(
                    model => model.ServiceItemID,
                    mapper => mapper.MapFrom(
                        entity => entity.CallService.ServiceItem == null
                            ? string.Empty
                            : entity.CallService.ServiceItem.ID.ToString()))
                .ForMember(
                    model => model.ServiceItemName,
                    mapper => mapper.MapFrom(
                        entity => entity.CallService.ServiceItem == null
                            ? string.Empty
                            : entity.CallService.ServiceItem.Name))
                .ForMember(
                    model => model.ServiceAttendanceID,
                    mapper => mapper.MapFrom(
                        entity => entity.CallService.ServiceAttendance == null
                            ? string.Empty
                            : entity.CallService.ServiceAttendance.ID.ToString()))
                .ForMember(
                    model => model.ServiceAttendanceName,
                    mapper => mapper.MapFrom(
                        entity => entity.CallService.ServiceAttendance == null
                            ? string.Empty
                            : entity.CallService.ServiceAttendance.Name))
                .ForMember(
                    model => model.CallType,
                    mapper => mapper.MapFrom(entity => entity.CallType.FullName))
                .ForMember(
                    model => model.RFCResultID,
                    mapper => mapper.MapFrom(entity => entity.RequestForServiceResultID))
                .ForMember(model => model.RFCResultName,
                    mapper =>
                    {
                        mapper.Condition(entity => entity.RequestForServiceResultID.HasValue);
                        mapper.MapFrom(entity => entity.RequestForServiceResult.Name);
                    })
                .ForMember(model => model.IncidentResultName,
                    mapper =>
                    {
                        mapper.Condition(entity => entity.IncidentResultID.HasValue);
                        mapper.MapFrom(entity => entity.IncidentResult.Name);
                    })
                .ForMember(model => model.IsRFCCallType,
                    mapper => mapper.MapFrom(entity => entity.CallType.IsChangeRequest))
                .ForMember(model => model.IsIncidentResultCallType,
                    mapper => mapper.MapFrom(entity => entity.CallType.IsIncident))
                .ForMember(
                    model => model.OwnerID,
                    mapper => mapper.MapFrom(entity =>
                        entity.OwnerID.HasValue ? entity.OwnerID.ToString() : string.Empty))
                .ForMember(
                    model => model.AccomplisherID,
                    mapper => mapper.MapFrom(entity =>
                        entity.AccomplisherID.HasValue ? entity.AccomplisherID.ToString() : string.Empty))
                .ForMember(
                    model => model.ExecutorID,
                    mapper => mapper.MapFrom(entity =>
                        entity.ExecutorID.HasValue ? entity.ExecutorID.ToString() : string.Empty))
                .ForMember(
                    details => details.UserField1Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Call, CallDetails>,
                        UserField>(_ => new UserField(UserFieldType.Call, FieldNumber.UserField1)))
                .ForMember(
                    details => details.UserField2Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Call, CallDetails>,
                        UserField>(_ => new UserField(UserFieldType.Call, FieldNumber.UserField2)))
                .ForMember(
                    details => details.UserField3Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Call, CallDetails>,
                        UserField>(_ => new UserField(UserFieldType.Call, FieldNumber.UserField3)))
                .ForMember(
                    details => details.UserField4Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Call, CallDetails>,
                        UserField>(_ => new UserField(UserFieldType.Call, FieldNumber.UserField4)))
                .ForMember(
                    details => details.UserField5Name,
                    mapper => mapper.MapFrom<
                        UserFieldNameResolver<Call, CallDetails>,
                        UserField>(_ => new UserField(UserFieldType.Call, FieldNumber.UserField5)))
                .ForMember(
                    details => details.ReceiptTypeName,
                    mapper => mapper.MapFrom<
                        LocalizedEnumResolver<Call, CallDetails, CallReceiptType>,
                        CallReceiptType>(call => call.ReceiptType))
                .ForMember(
                    entity => entity.FormValues,
                    mapping => mapping.MapFrom(data => data.FormValues))
                .ForMember(
                    entity => entity.RFCResultID,
                    mapping => mapping.MapFrom(data => data.RequestForServiceResultID))
                .ForMember(
                    entity=>entity.WorkOrdersRefCount,
                    mapper=>mapper.MapFrom(entity=>entity.WorkOrderReferences.Count())
                    )
                ;
        }
    }
}
