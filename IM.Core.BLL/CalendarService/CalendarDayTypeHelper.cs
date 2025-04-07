using InfraManager.DAL.CalendarWorkSchedules;

namespace InfraManager.BLL.CalendarService
{
    internal class CalendarDayTypeHelper
    {
        public static bool IsWorkDay(CalendarDayType type)
        {
            switch (type)
            {
                case CalendarDayType.Work:
                case CalendarDayType.WorkShort:
                case CalendarDayType.WorkLong:
                case CalendarDayType.WorkPreHoliday:
                    return true;
                default:
                    return false;
            }
        }
    }


}
