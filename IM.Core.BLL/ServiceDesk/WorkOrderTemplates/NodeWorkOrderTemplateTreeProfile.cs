using System.Linq;
using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.BLL.ServiceDesk.WorkOrderTemplates;

internal class NodeWorkOrderTemplateTreeProfile : Profile
{
    public NodeWorkOrderTemplateTreeProfile()
    {
        CreateMap<WorkOrderTemplateFolder, NodeWorkOrderTemplateTree>()
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.SubFolder.Any()))
            .ForMember(dst => dst.IconName, m => m.Ignore());

        CreateMap<WorkOrderTemplate, NodeWorkOrderTemplateTree>()
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => false))
            .ForMember(dst => dst.ParentId, m => m.MapFrom(scr => scr.FolderID))
            .ForMember(dst => dst.IconName, m=> m.Ignore());
    }
}
