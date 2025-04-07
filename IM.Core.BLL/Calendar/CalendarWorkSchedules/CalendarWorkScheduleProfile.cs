using System;
using System.Linq;
using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;
using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules
{
    public class CalendarWorkScheduleProfile : Profile
    {
        public CalendarWorkScheduleProfile()
        {


            CreateMap<CalendarWorkScheduleShift, CalendarWorkScheduleShiftDetails>()
                .ForMember(dst => dst.TotalTimeSpanInMinutes, m => m.MapFrom(c => c.WorkScheduleShiftExclusions.Sum(s => s.TimeSpanInMinutes)))
                .ReverseMap();

            CreateMap<CalendarWorkSchedule, CalendarWorkScheduleDetails>()
                .ReverseMap();

            CreateMap<CalendarWorkScheduleData, CalendarWorkSchedule>()
                .ConstructUsing(c => new(c.Name));

            CreateMap<CalendarWorkScheduleItem, CalendarWorkScheduleItemDetails>()
                .ForMember(dst => dst.TimeEnd, m => m.MapFrom(scr => scr.TimeStart.AddMinutes(scr.TimeSpanInMinutes)))
                .ForMember(dst => dst.DayOfYearDate, m => m.MapFrom(scr => new DateTime(scr.CalendarWorkSchedule.Year, 1, 1).AddDays(scr.DayOfYear - 1)))
                .ForMember(dst => dst.DayTypeName, m => m.MapFrom<LocalizedEnumResolver<CalendarWorkScheduleItem, CalendarWorkScheduleItemDetails, CalendarDayType>,
                        CalendarDayType>(
                        entity => entity.DayType))
                .ForMember(dst => dst.TotalTimeSpanInMinutes, m => m.MapFrom(c => c.WorkScheduleItemExclusions.Sum(s => s.TimeSpanInMinutes)))
                .ReverseMap();

            CreateMap<CalendarWorkScheduleItemExclusion, CalendarWorkScheduleItemExclusionDetails>()
                //.ForMember(dst => dst.ExclusionName, m => m.MapFrom(scr => scr.Exclusion.Name))//TODO FIX LATER
                .ReverseMap();

            CreateMap<CalendarWorkScheduleShiftExclusion, CalendarWorkScheduleShiftExclusionDetails>()
                .ForMember(dst => dst.TimeEnd, m => m.MapFrom(scr => scr.TimeStart.AddMinutes(scr.TimeSpanInMinutes)))
                .ForMember(dst => dst.ExclusionName, m => m.MapFrom(scr => scr.Exclusion.Name))
                .ReverseMap()
                .ForMember(dst => dst.Exclusion, m => m.Ignore())
                .ForMember(dst => dst.CalendarWorkScheduleShift, m => m.Ignore());



            CreateMap<CalendarWorkSchedule, CalendarWorkScheduleWithRelatedDetails>()
                .ForMember(dst => dst.WorkScheduleItems, m => m.Ignore())
                .ReverseMap();


            CreateMap<CalendarWorkScheduleShift, CalendarWorkScheduleShiftCreateData>()
                .ReverseMap();



            CreateMap<CreateCalendarWorkScheduleShiftExclusionDetails, CalendarWorkScheduleShiftExclusion>()
                .ConstructUsing(c => new(c.CalendarWorkScheduleShiftID, c.ExclusionID, c.TimeStart, c.TimeSpanInMinutes))
                .ReverseMap();

            CreateMap<CreateCalendarWorkScheduleShiftExclusionDetails, CalendarWorkScheduleShiftExclusionDetails>()
                .ReverseMap();



            CreateMap<DeleteCalendarWorkScheduleShiftExclusionDetails, CalendarWorkScheduleShiftExclusion>()
                .ReverseMap();

            CreateMap<DeleteCalendarWorkScheduleShiftExclusionDetails, CalendarWorkScheduleShiftExclusionDetails>()
                .ReverseMap();
        }
    }
}
