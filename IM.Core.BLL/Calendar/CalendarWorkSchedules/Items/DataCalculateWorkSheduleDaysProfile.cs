using AutoMapper;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;

internal sealed class DataCalculateWorkSheduleDaysProfile : Profile
{
    public DataCalculateWorkSheduleDaysProfile()
    {
        CreateMap<CalendarWorkScheduleData, DataCalculateWorkSheduleDays>();
    }
}
