using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Calendar.CalendarWeekends;

internal class CalendarWeekendProfile : Profile
{
    public CalendarWeekendProfile()
    {
        CreateMap<CalendarWeekend, CalendarWeekendDetails>()
            .ReverseMap();
    }
}
