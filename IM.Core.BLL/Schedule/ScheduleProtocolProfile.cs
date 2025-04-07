using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Extensions;
using InfraManager.BLL.Scheduler;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services.ScheduleService;

namespace InfraManager.BLL.Schedule
{
    public class ScheduleProtocolProfile : Profile
    {
        public ScheduleProtocolProfile()
        {
            CreateMap<ScheduleTask, SchedulerProtocolsDetail>()
                .ForMember(dst => dst.TaskTypeName, m => m.MapFrom<LocalizedEnumResolver<ScheduleTask, SchedulerProtocolsDetail, TaskType>, TaskType>(
                    queryItem => queryItem.TaskType))
                .ForMember(dst => dst.TaskStateName, m => m.MapFrom<LocalizedEnumResolver<ScheduleTask, SchedulerProtocolsDetail, TaskState>, TaskState>(
                    queryItem => queryItem.TaskState));
        }
    }
}
