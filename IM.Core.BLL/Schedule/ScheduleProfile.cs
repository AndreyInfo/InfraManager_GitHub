using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Scheduler;
using InfraManager.Services.ScheduleService;
using System.Linq;
using InfraManager.ServiceBase.ScheduleService;

namespace InfraManager.BLL.Schedule
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<ScheduleTask, SchedulerListDetail>()
                .ForMember(p => p.Description, m => m.MapFrom(src => src.Note))
                .ForMember(p => p.Schedule, m => m.MapFrom(src => src.Schedules.Any()))
                .ForMember(dst => dst.TaskStateName, m => m.MapFrom<LocalizedEnumResolver<ScheduleTask, SchedulerListDetail, TaskState>, TaskState>(
                    queryItem => queryItem.TaskState))
                .ReverseMap();

        }
    }
}
