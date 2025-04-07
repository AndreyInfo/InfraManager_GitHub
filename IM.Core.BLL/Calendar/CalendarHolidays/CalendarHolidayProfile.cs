using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

internal class CalendarHolidayProfile : Profile
{
    public CalendarHolidayProfile()
    {
        CreateMap<CalendarHoliday, CalendarHolidayTableDetails>()
            .ReverseMap();

        CreateMap<CalendarHoliday, CalendarHolidayDetails>()
            .ReverseMap();

        CreateMap<CalendarHoliday, CalendarHolidayInsertDetails>()
            .ReverseMap();
    }
}
