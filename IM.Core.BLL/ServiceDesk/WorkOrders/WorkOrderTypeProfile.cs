using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using System.Drawing;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderTypeProfile : Profile
    {
        public WorkOrderTypeProfile()
        {
            CreateMap<WorkOrderType, WorkOrderTypeDetails>()
                .ForMember(x => x.Color, x => x.MapFrom(x => ColorTranslator.ToHtml(Color.FromArgb(x.Color)))); //TODO move to PL????

            CreateMap<WorkOrderTypeData, WorkOrderType>()
                .ForMember(x => x.Color, x => x.MapFrom(x => ColorTranslator.FromHtml(x.Color).ToArgb())); //TODO move to PL????
        }

    }
}
