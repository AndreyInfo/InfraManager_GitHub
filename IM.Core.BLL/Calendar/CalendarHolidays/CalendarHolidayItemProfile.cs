using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Calendar.CalendarHolidays;

internal class CalendarHolidayItemProfile : Profile
{
    public CalendarHolidayItemProfile()
    {
        CreateMap<CalendarHolidayItem, CalendarHolidayItemDetails>()
            .ReverseMap();
    }
}
