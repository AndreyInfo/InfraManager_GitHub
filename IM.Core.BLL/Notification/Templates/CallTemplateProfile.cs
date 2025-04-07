using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification.Templates;

public class CallTemplateProfile : Profile
{
    public CallTemplateProfile()
    {
        CreateMap<Call, CallTemplate>()
            .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.IMObjID))
            .ForMember(dst => dst.NumberString,
                opt => opt.MapFrom(
                    src => src.ReferenceName))
            .ForMember(dst => dst.ReceiptTypeString,
                opt => opt.MapFrom<LocalizedEnumResolver<Call, CallTemplate, CallReceiptType>, CallReceiptType>(
                    src => src.ReceiptType))
            .ForMember(dst => dst.GradeString,
                opt => opt.MapFrom(
                    src => src.Grade.HasValue ? src.Grade.ToString() : string.Empty))
            .ForMember(dst => dst.EscalationCountString,
                opt => opt.MapFrom(
                    src => src.EscalationCount.ToString()))
            .ForMember(dst => dst.ServiceName,
                opt => opt.MapFrom(
                    src => src.CallService != null ? src.CallService.ServiceName : string.Empty))
            .ForMember(dst => dst.ServiceItemFullName,
                opt => opt.MapFrom(
                    src => src.CallService != null && src.CallService.ServiceItem != null ? src.CallService.ServiceItem.Name : string.Empty))
            .ForMember(dst => dst.ServiceAttendanceFullName,
                opt => opt.MapFrom(
                    src => src.CallService != null && src.CallService.ServiceAttendance != null ? src.CallService.ServiceAttendance.Name : string.Empty))
            .ForMember(dst => dst.RFSResultName,
                opt => opt.MapFrom(
                    src => src.RequestForServiceResult != null ? src.RequestForServiceResult.Name : string.Empty))
            .ForMember(dst => dst.InitiatorLastName,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.Surname : string.Empty))
            .ForMember(dst => dst.InitiatorFirstName,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.Name : string.Empty))
            .ForMember(dst => dst.InitiatorLogin,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.LoginName : string.Empty))
            .ForMember(dst => dst.ClientLastName,
                opt => opt.MapFrom(
                    src => src.Client != null ? src.Client.Surname : string.Empty))
            .ForMember(dst => dst.ClientFirstName,
                opt => opt.MapFrom(
                    src => src.Client != null ? src.Client.Name : string.Empty))
            .ForMember(dst => dst.ClientLogin,
                opt => opt.MapFrom(
                    src => src.Client != null ? src.Client.LoginName : string.Empty))
            .ForMember(dst => dst.OwnerLastName,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.Surname : string.Empty))
            .ForMember(dst => dst.OwnerFirstName,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.Name : string.Empty))
            .ForMember(dst => dst.OwnerLogin,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.LoginName : string.Empty))
            .ForMember(dst => dst.ExecutorLastName,
                opt => opt.MapFrom(
                    src => src.Executor != null ? src.Executor.Surname : string.Empty))
            .ForMember(dst => dst.ExecutorFirstName,
                opt => opt.MapFrom(
                    src => src.Executor != null ? src.Executor.Name : string.Empty))
            .ForMember(dst => dst.ExecutorLogin,
                opt => opt.MapFrom(
                    src => src.Executor != null ? src.Executor.LoginName : string.Empty))
            .ForMember(dst => dst.AccomplisherLastName,
                opt => opt.MapFrom(
                    src => src.Accomplisher != null ? src.Accomplisher.Surname : string.Empty))
            .ForMember(dst => dst.AccomplisherFirstName,
                opt => opt.MapFrom(
                    src => src.Accomplisher != null ? src.Accomplisher.Name : string.Empty))
            .ForMember(dst => dst.AccomplisherLogin,
                opt => opt.MapFrom(
                    src => src.Accomplisher != null ? src.Accomplisher.LoginName : string.Empty))
            .ForMember(dst => dst.DocumentCountString,
                opt => opt.MapFrom(
                    src => src.Aggregate != null ? src.Aggregate.DocumentCount.ToString() : string.Empty))
            .ForMember(dst => dst.ProblemCountString,
                opt => opt.MapFrom(
                    src => src.Aggregate != null ? src.Aggregate.ProblemCount.ToString() : string.Empty))
            .ForMember(dst => dst.WorkOrderCountString,
                opt => opt.MapFrom(
                    src => src.Aggregate != null ? src.Aggregate.WorkOrderCount.ToString() : string.Empty))
            .ForMember(dst => dst.ManhoursString,
                opt => opt.MapFrom<ManhoursResolver<Call, CallTemplate>, int>(
                    src => src.ManhoursInMinutes))
            .ForMember(dst => dst.ManhoursNormString,
                opt => opt.MapFrom<ManhoursResolver<Call, CallTemplate>, int>(
                    src => src.ManhoursNormInMinutes))
            .ForMember(dst => dst.BudgetString,
                opt => opt.MapFrom(
                    src => string.Empty)) // todo: Добавить мапинг когда будет реализовано в агрегате CallBudgetUsageAggregate.
            .ForMember(dst => dst.BudgetUsageCauseString,
                opt => opt.MapFrom(
                    src => string.Empty)) // todo: Добавить мапинг когда будет реализовано в агрегате BudgetUsageCauseAggregate.
            .ForMember(dst=>dst.NumberOnly, opt=>opt.MapFrom(src=>src.Number.ToString()))
            ;
    }
}