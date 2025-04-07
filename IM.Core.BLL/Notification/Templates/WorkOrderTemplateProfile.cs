using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.Core;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification.Templates;

public class WorkOrderTemplateProfile : Profile
{
    public WorkOrderTemplateProfile()
    {
        CreateMap<WorkOrder, WorkOrderTemplate>()
            .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.IMObjID))
            .ForMember(dst => dst.NumberString,
                opt => opt.MapFrom(
                    src => src.ReferenceNumber))
            .ForMember(dst => dst.ManhoursString,
                opt => opt.MapFrom<ManhoursResolver<WorkOrder, WorkOrderTemplate>, int>(
                    src => src.ManhoursInMinutes))
            .ForMember(dst => dst.ManhoursNormString,
                opt => opt.MapFrom<ManhoursResolver<WorkOrder, WorkOrderTemplate>, int>(
                    src => src.ManhoursNormInMinutes))
            .ForMember(dst => dst.WorkOrderReferenceString,
                opt => opt.MapFrom(
                    src => src.WorkOrderReference == null ? string.Empty : src.WorkOrderReference.ReferenceName))
            .ForMember(dst => dst.WorkOrderTypeName,
                opt => opt.MapFrom(
                    src => src.Type != null ? src.Type.Name : string.Empty))
            .ForMember(dst => dst.WorkOrderPriorityName,
                opt => opt.MapFrom(
                    src => src.Priority != null ? src.Priority.Name : string.Empty))
            .ForMember(dst => dst.InitiatorLastName,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.Surname : string.Empty))
            .ForMember(dst => dst.InitiatorFirstName,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.Name : string.Empty))
            .ForMember(dst => dst.InitiatorLogin,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.LoginName : string.Empty))
            .ForMember(dst => dst.AssignorLastName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Surname : string.Empty))
            .ForMember(dst => dst.AssignorFirstName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Name : string.Empty))
            .ForMember(dst => dst.AssignorPatronymic,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Patronymic : string.Empty))
            .ForMember(dst => dst.AssignorFullName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.FullName : string.Empty))
            .ForMember(dst => dst.AssignorLogin,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.LoginName : string.Empty))
            .ForMember(dst => dst.AssignorPhone,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Phone : string.Empty))
            .ForMember(dst => dst.AssignorNumber,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Number : string.Empty))
            .ForMember(dst => dst.AssignorEmail,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Email : string.Empty))
            .ForMember(dst => dst.AssignorFax,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Fax : string.Empty))
            .ForMember(dst => dst.AssignorPager,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.Pager : string.Empty))
            .ForMember(dst => dst.AssignorPositionName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.PositionName : string.Empty))
            .ForMember(dst => dst.AssignorBuildingName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.BuildingName : string.Empty))
            .ForMember(dst => dst.AssignorRoomName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.RoomName : string.Empty))
            .ForMember(dst => dst.AssignorWorkplaceName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.WorkplaceName : string.Empty))
            .ForMember(dst => dst.AssignorOrganizationName,
                opt => opt.MapFrom(
                    src => src.Assignee != null ? src.Assignee.OrganizationName : string.Empty))
            .ForMember(dst => dst.ExecutorLastName,
                opt => opt.MapFrom(
                    src => src.Executor != null ? src.Executor.Surname : string.Empty))
            .ForMember(dst => dst.ExecutorFirstName,
                opt => opt.MapFrom(
                    src => src.Executor != null ? src.Executor.Name : string.Empty))
            .ForMember(dst => dst.ExecutorLogin,
                opt => opt.MapFrom(
                    src => src.Executor != null ? src.Executor.LoginName : string.Empty))
            .ForMember(dst => dst.DocumentCountString,
                opt => opt.MapFrom(
                    src => src.Aggregate != null ? src.Aggregate.DocumentCount.ToString() : string.Empty))
            .ForMember(dst => dst.QueueName,
                opt =>
                {
                    opt.PreCondition(src => src.Group != null);
                    opt.MapFrom(src => src.Group.Name);
                })
             .ForMember(dst => dst.NumberOnly, opt => opt.MapFrom(src => src.Number.ToString()))
           ;
    }
}