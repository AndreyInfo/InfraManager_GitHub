using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ServiceBase.WorkflowService;

namespace InfraManager.BLL.Workflow
{
    public class WorkFlowShemeProfile : Profile
    {
        public WorkFlowShemeProfile()
        {
            CreateMap<WorkFlowScheme, WorkflowSchemeDetailsModel>()
                .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.Id));
            CreateMap<WorkflowSchemeDetailsModel, WorkflowSchemeNameDetails>()
                .ReverseMap();
            CreateMap<WorkflowSchemeModel, WorkflowSchemeNameDetails>()
                .ReverseMap();

        }
    }
}
