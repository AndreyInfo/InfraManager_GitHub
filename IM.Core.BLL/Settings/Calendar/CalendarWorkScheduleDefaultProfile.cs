using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Settings.Calendar;

public sealed class CalendarWorkScheduleDefaultProfile : Profile
{
    public CalendarWorkScheduleDefaultProfile()
    {
        CreateMap<CalendarWorkScheduleDefault, CalendarSettingsDetails>()
            .ForMember(dst => dst.CalendarWeekendName, m => m.MapFrom(scr => scr.CalendarWeekend.Name))
            .ForMember(dst => dst.CalendarHolidayName, m => m.MapFrom(scr => scr.CalendarHoliday.Name))
            .ForMember(dst => dst.TimeZoneName, m => m.MapFrom(scr => scr.TimeZone.Name))
            .ForMember(dst => dst.TimeStartEnd, m => m.MapFrom(scr => scr.TimeEnd))
            .ForMember(dst => dst.DinnerTimeSpanInMinutes, m=> m.MapFrom(scr => scr.ExclusionTimeSpanInMinutes))
            .ReverseMap()
            .ForMember(dst => dst.TimeEnd, m=> m.MapFrom(scr => scr.TimeStartEnd))
            .ForMember(dst => dst.CalendarWeekend, m=> m.Ignore())
            .ForMember(dst => dst.CalendarHoliday, m=> m.Ignore());
    }
}
