using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.CalendarService
{
    public static class CalendarServiceHelper
    {
        public static bool IsWorkDay(this CalendarDayType calendarDay)
        {
            switch (calendarDay)
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
        public static bool IsWorkDay(this CalendarInterval calendarInterval)
        {
            return IsWorkDay(calendarInterval.CalendarDayType);
        }

        public static TimeSpan DateTime2Time(this DateTime dateTime)
        {
            return dateTime - new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }
        public static TimeSpan DaylightDelta(this DAL.ServiceDesk.TimeZoneAdjustmentRule rule)
        {
            return TimeSpan.FromMinutes(rule.DaylightDeltaInMinutes);
        }
        public static TimeSpan BaseUtcOffset(this DAL.ServiceDesk.TimeZone timeZone)
        {
            return TimeSpan.FromMinutes(timeZone.BaseUtcOffsetInMinutes);
        }
        public static DAL.ServiceDesk.TimeZoneAdjustmentRule GetAdjustmentRuleForTime(this DAL.ServiceDesk.TimeZone timeZone, DateTime forTime)
        {
            return timeZone.TimeZoneAdjustmentRules.FirstOrDefault(x=>x.DateStart<=forTime && x.DateEnd>= forTime);
        }
        public static TimeSpan ToTimeSpan(this short value)
        {
            return TimeSpan.FromMinutes(value);
        }
        public static Dictionary<DayOfWeek, bool> WeekendList(this CalendarWeekend value)
        {
            return new Dictionary<DayOfWeek, bool>()
            {
                { DayOfWeek.Sunday, value.Sunday },
                { DayOfWeek.Monday, value.Monday},
                { DayOfWeek.Tuesday, value.Tuesday },
                { DayOfWeek.Wednesday, value.Wednesday},
                { DayOfWeek.Thursday, value.Thursday },
                { DayOfWeek.Friday, value.Friday},
                { DayOfWeek.Saturday, value.Saturday },
            };

        }

    }
}
