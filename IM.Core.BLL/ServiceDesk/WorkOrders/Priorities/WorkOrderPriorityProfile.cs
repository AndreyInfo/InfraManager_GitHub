using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using System.Drawing;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Priorities;

internal class WorkOrderPriorityProfile : Profile
{
    public WorkOrderPriorityProfile()
    {
        CreateMap<WorkOrderPriority, WorkOrderPriorityDetails>()
            .ForMember(model => model.Color, mapper => mapper.MapFrom<PriorityColorResolver>());

        CreateMap<WorkOrderPriorityData, WorkOrderPriority>()
            .ConstructUsing(x => new WorkOrderPriority(x.Name, x.Sequence))
            .ForMember(x => x.Color, x => x.MapFrom(x => ColorTranslator.FromHtml(x.Color).ToArgb())); //TODO move to PL????
    }
}
