using AutoMapper;
using InfraManager.DAL;
using InfraManager.ServiceBase.ScheduleService;
using InfraManager.Services.ScheduleService;

namespace IM.Core.Schedule.BLL
{
    internal class ScheduleTaskProfile : Profile
    {
        public ScheduleTaskProfile()
        {
            CreateMap<ScheduleTaskEntity,ScheduleTask>()
               .ForMember(p => p.Schedules, m => m.MapFrom(src => src.Schedules))
               .ReverseMap();

            CreateMap<ScheduleEntity, TaskSchedule>()
               .ForMember(p => p.Months, m => m.MapFrom(src => !String.IsNullOrEmpty(src.Months)  ? src.Months.Split(',', StringSplitOptions.None).Select(int.Parse).ToArray():null))
               .ForMember(p => p.DaysOfWeek, m => m.MapFrom(src => !String.IsNullOrEmpty(src.DaysOfWeek) ? src.DaysOfWeek.Split(',', StringSplitOptions.None).Select(int.Parse).ToArray():null))
               .ForMember(p => p.TaskScheduleID, m => m.MapFrom(src => src.ScheduleTaskEntityID));

            CreateMap<TaskSchedule, ScheduleEntity>()
               .ForMember(p => p.Months, m => m.MapFrom(src => string.Join(",", src.Months)))
               .ForMember(p => p.DaysOfWeek, m => m.MapFrom(src => string.Join(",", src.DaysOfWeek)))
               .ForMember(p => p.ScheduleTaskEntityID, m => m.MapFrom(src => src.TaskScheduleID));

        }
    }
}