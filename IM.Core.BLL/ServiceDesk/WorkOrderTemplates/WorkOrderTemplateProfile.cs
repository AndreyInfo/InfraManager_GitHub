using AutoMapper;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders.Templates;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

internal sealed class WorkOrderTemplateProfile : Profile
{
    public WorkOrderTemplateProfile()
    {
        CreateMap<WorkOrderTemplate, WorkOrderTemplateDetails>()
            .ForMember(dst => dst.WorkOrderPriorityName, m => m.MapFrom(scr => scr.Priority.Name))
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.FolderID))
            .ForMember(dst => dst.WorkOrderTypeName, m => m.MapFrom(scr => scr.Type.Name))
            .ForMember(dst => dst.FolderName, m => m.MapFrom(scr => scr.Folder.Name))
            .ForMember(dst => dst.InitiatorName, m => m.MapFrom(scr => scr.Initiator.FullName))
            .ReverseMap()
            .ForMember(dst => dst.FolderID, m => m.MapFrom(scr => scr.WorkOrderTemplateFolderID))
            .ForMember(dst => dst.ExecutorAssignmentType, m => m.MapFrom(scr => (ExecutorAssignmentType)((byte)scr.ExecutorAssignmentType
                                                                                + (scr.FlagTTZ ? (byte)ExecutorAssignmentType.FlagTTZ : 0)
                                                                                + (scr.FlagTOZ ? (byte)ExecutorAssignmentType.FlagTOZ : 0)
                                                                                + (scr.FlagServiceResponsible ? (byte)ExecutorAssignmentType.FlagServiceResponsible : 0)
                                                                                + (scr.FlagCalendarWorkSchedule ? (byte)ExecutorAssignmentType.FlagCalendarWorkSchedule : 0)
                                                                                )))
            .ForMember(dst => dst.Folder, m => m.Ignore())
            .ForMember(dst => dst.Type, m => m.Ignore())
            .ForMember(dst => dst.Priority, m => m.Ignore())
            .ForMember(dst => dst.Initiator, m => m.Ignore());


        CreateMap<WorkOrderTemplateFolder, WorkOrderTemplateFolderDetails>()
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.ParentID))
            .ForMember(dst => dst.ParentName, m => m.MapFrom(scr => scr.Parent.Name))
            .ReverseMap()
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.ParentID))
            .ForMember(dst => dst.Parent, m => m.Ignore());
    }
}
