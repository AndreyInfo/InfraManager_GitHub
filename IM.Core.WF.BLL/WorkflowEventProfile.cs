using AutoMapper;
using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager.DAL.WF;

namespace IM.Core.WF.BLL;

public class WorkflowEventProfile : Profile
{
    public WorkflowEventProfile()
    {
        CreateMap<WorkflowEvent, WorkflowEventModel>();
    }
}