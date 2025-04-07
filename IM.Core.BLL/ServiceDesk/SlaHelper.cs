using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.BLL.SlaUtils;

public static class SlaHelper
{
    public static DateTime GetUtcFinishDateByCalendar(DateTime startDate, TimeSpan duration,
        CalendarWorkSchedule calendar, TimeZone calendarTimeZone, CalendarWorkScheduleDefault defaultCalendar)
    {
        DateTime dt = startDate;
        var ts = duration;
        //            
        var exclusionIntervals = new List<Interval>();
        var fixedExclusionIntervals = new List<Interval>();
        /*foreach (var e in calendarExclusionList)
            if (e.UtcPeriodStart >= utcStartDate || (e.UtcPeriodStart <= utcStartDate && e.UtcPeriodEnd >= utcStartDate))
                fixedExclusionIntervals.Add(new Interval(e.IsWorkPeriod, e.UtcPeriodStart, e.UtcPeriodEnd));*/
        //
        while (ts > TimeSpan.Zero)
        {
            exclusionIntervals.Clear();
            exclusionIntervals.AddRange(fixedExclusionIntervals);
            //
            DateTime utcBeginOfDay = dt.Date;
            DateTime utcBeginOfNextDay = GetNextDay(utcBeginOfDay).Date;
            Interval day = GetDayInfo(utcBeginOfDay, calendar, calendarTimeZone, exclusionIntervals,
                defaultCalendar); //рабочее время по дню
            Interval nextDay = GetDayInfo(utcBeginOfNextDay, calendar, calendarTimeZone, exclusionIntervals,
                defaultCalendar); //рабочее время по следующему дню, в случае перекрытия
            //
            if (calendar != null)
            {
                var item = calendar.WorkScheduleItems.First(item => item.DayOfYear == utcBeginOfDay.DayOfYear);
                //добавление исключений дня
                foreach (var itemExclusion in item.WorkScheduleItemExclusions)
                {
                    DateTime utc_start =
                        utcBeginOfDay + itemExclusion.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME);
                    DateTime utc_end = utc_start + TimeSpan.FromMinutes(itemExclusion.TimeSpanInMinutes);
                    exclusionIntervals.Add(new Interval
                    (CalendarDayType.Holiday,
                        TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                        TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
                }

                item = calendar.WorkScheduleItems.First(item => item.DayOfYear == utcBeginOfNextDay.DayOfYear);
                //добавление исключений следующего дня
                foreach (var nextItemExclusion in item.WorkScheduleItemExclusions)
                {
                    DateTime utc_start = utcBeginOfNextDay +
                                         nextItemExclusion.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME);
                    ;
                    DateTime utc_end = utc_start + TimeSpan.FromMinutes(nextItemExclusion.TimeSpanInMinutes);
                    exclusionIntervals.Add(new Interval
                    (CalendarDayType.Holiday,
                        TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                        TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
                }
            }

            //
            List<Interval> dayIntervals = new List<Interval>(exclusionIntervals); //все интервалы-исключения
            dayIntervals.Add(day); //текущий день
            dayIntervals.Add(nextDay); //следующий день, может кусочек есть
            dayIntervals =
                Interval.GetIntervalsByDay(dayIntervals, utcBeginOfDay); //обрезаем все интевалы до текущего дня
            dayIntervals = Interval.ToIntervalsWithoutIntersect(dayIntervals, true); //приводим к непересекающимся
            //
            for (int i = 0; i < dayIntervals.Count; i++)
            {
                var interval = dayIntervals[i];
                if (dt > interval.UtcEndDate && i < dayIntervals.Count - 1)
                {
                    continue;
                }

                if (!interval.IsWorkInterval)
                {
                    //не рабочий интервал - переходим к следующему
                    dt = interval.UtcEndDate;
                    continue;
                }

                //
                if (dt <= interval.UtcStartDate)
                {
                    //попали до рабочего времени
                    dt = interval.UtcStartDate;
                    if (interval.Duration >= ts)
                    {
                        //рабочий интервал больше необходимой работы
                        dt = dt.Add(ts);
                        ts = TimeSpan.Zero;
                        //
                        return dt;
                    }
                    else
                    {
                        //рабочий интервал короче необходимой работы
                        dt = interval.UtcEndDate;
                        ts -= interval.Duration;
                    }
                }
                else if (interval.UtcStartDate < dt && dt <= interval.UtcEndDate)
                {
                    //попали в рабочее время
                    if ((interval.UtcEndDate - dt) >= ts)
                    {
                        //рабочий интервал больше необходимой работы
                        dt = dt.Add(ts);
                        ts = TimeSpan.Zero;
                        //
                        return dt;
                    }
                    else
                    {
                        //рабочий интервал короче необходимой работы
                        ts -= (interval.UtcEndDate - dt);
                        dt = interval.UtcEndDate;
                    }
                }
                else if (dt > interval.UtcEndDate)
                {
                    //попали после рабочего интервала
                    dt = interval.UtcEndDate;
                }
            }

            //
            if (utcBeginOfDay == dt.Date) //GetIntervalsByDay может перевести исходный в следующий день
                dt = GetNextDay(dt);
        }

        //
        return dt;
    }

    private static DateTime GetNextDay(DateTime dt)
    {
        return (new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind)).AddDays(1);
    }

    private static Interval GetDayInfo(DateTime utcDate, CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone,
        List<Interval> defaultExclusions, CalendarWorkScheduleDefault defaultCalendar)
    {
        calendarTimeZone = calendarTimeZone ?? defaultCalendar.TimeZone;
        //
        DateTime
            date = calendarTimeZone
                .ConvertTimeFromUtc(utcDate); //момент времени, приведенный к временной зоне календаря
        if (workCalendar != null)
        {
            //задан календарь работы
            //содержится ли такой день в этом календаре?
            /*if (workCalendar.Year != date.Year)
            {
                //поиск подходящего календаря
                workCalendar = GetAndCacheCalendar(date.Year, workCalendar.Name);
            } зададут нормальный календарь, не обломятся*/
            //
            if (workCalendar != null && workCalendar.Year == date.Year)
            {
                //поиск дня
                var item = workCalendar.WorkScheduleItems.FirstOrDefault(
                    item => item.DayOfYear == (short)date.DayOfYear);

                if (item != null)
                {
                    //есть такой день календаря
                    //добавление исключений дня
                    foreach (var itemExclusion in item.WorkScheduleItemExclusions)
                    {
                        var startTimeSpan = itemExclusion.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME);
                        var utc_start = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, startTimeSpan.Hours,
                            startTimeSpan.Minutes, startTimeSpan.Seconds, startTimeSpan.Milliseconds,
                            DateTimeKind.Unspecified);
                        DateTime utc_end = utc_start + TimeSpan.FromMinutes(itemExclusion.TimeSpanInMinutes);
                        defaultExclusions.Add(new Interval
                        (CalendarDayType.Holiday,
                            TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                            TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
                    }

                    return CreateInterval((CalendarDayType)item.DayType, date,
                        item.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME),
                        TimeSpan.FromMinutes(item.TimeSpanInMinutes), calendarTimeZone);
                }
            }
        }

        //
        //не задан календарь работы или день в нем не найден - берем из календаря по умолчанию
        var def = defaultCalendar;
        //
        CalendarDayType dayType = CalendarDayType.Work;
        var weekendList = new Dictionary<DayOfWeek, bool>
        {
            { DayOfWeek.Monday, def.CalendarWeekend.Monday },
            { DayOfWeek.Tuesday, def.CalendarWeekend.Tuesday },
            { DayOfWeek.Wednesday, def.CalendarWeekend.Wednesday },
            { DayOfWeek.Thursday, def.CalendarWeekend.Thursday },
            { DayOfWeek.Friday, def.CalendarWeekend.Friday },
            { DayOfWeek.Saturday, def.CalendarWeekend.Saturday },
            { DayOfWeek.Sunday, def.CalendarWeekend.Sunday }
        };

        if (def.CalendarWeekend != null && weekendList[date.DayOfWeek])
            dayType = CalendarDayType.Weekend;
        if (CalendarDayTypeHelper.IsWorkDay(dayType) && def.CalendarHoliday != null)
            foreach (var h in def.CalendarHoliday.CalendarHolidayItems)
                if ((byte)h.Month == date.Month && h.Day == date.Day)
                {
                    dayType = CalendarDayType.Holiday;
                    break;
                }

        //
        //добавление исключений дня
        if (defaultExclusions != null && def.DinnerTimeStart.HasValue && def.ExclusionTimeSpanInMinutes.HasValue)
        {
            var startSpan = def.DinnerTimeStart.Value.Subtract(TimeZoneExtensions.MIN_UTC_TIME);
            DateTime utc_start = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, startSpan.Hours,
                startSpan.Minutes, startSpan.Seconds, startSpan.Milliseconds, DateTimeKind.Unspecified);
            DateTime utc_end = utc_start + TimeSpan.FromMinutes(def.ExclusionTimeSpanInMinutes.Value);
            defaultExclusions.Add(new Interval
            (CalendarDayType.Holiday,
                TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
        }

        //
        return CreateInterval(dayType, date, def.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME),
            TimeSpan.FromMinutes(def.TimeSpanInMinutes), calendarTimeZone);
    }

    private static Interval CreateInterval(CalendarDayType dayType, DateTime date, TimeSpan timeStart,
        TimeSpan timeSpan, TimeZone calendarTimeZone)
    {
        if (!CalendarDayTypeHelper.IsWorkDay(dayType))
        {
            timeStart = TimeSpan.Zero;
            timeSpan = TimeSpan.Zero;
        }

        //
        DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes,
            timeStart.Seconds, timeStart.Milliseconds, DateTimeKind.Unspecified);
        DateTime utcDateTime = TimeZoneExtensions.ConvertTimeToUtc(dateTime, calendarTimeZone);
        //
        return new Interval(
            dayType,
            utcDateTime,
            timeSpan);
    }

    private static bool RegistrationMatch(FilterOperation operation, long t, long ticks)
    {
        switch (operation)
        {
            case FilterOperation.Equal:
                if (ticks != t) return false;
                break;
            case FilterOperation.GT:
                if (ticks <= t) return false;
                break;
            case FilterOperation.GTE:
                if (ticks < t) return false;
                break;
            case FilterOperation.LT:
                if (ticks >= t) return false;
                break;
            case FilterOperation.LTE:
                if (ticks > t) return false;
                break;
            default: return true; 
        }

        return true;
    }

    internal static RuleValue Parse(Rule rule)
    {
        if (rule.Value == null)
        {
            return null;
        }

        var retval = new RuleValue();
        var stream = new MemoryStream(rule.Value);
        using (var reader = new BinaryReader(stream))
        {
            string version = reader.ReadString();
            //
            RuleValueType type;
            int count;
            FilterOperation operation;
            long time;
            Guid id;
            //
            while (stream.Position < stream.Length)
            {
                type = (RuleValueType)reader.ReadByte();
                //
                switch (type)
                {
                    case RuleValueType.Price:
                        retval.Price = reader.ReadString();
                        break;
                    case RuleValueType.PromiseTime:
                        retval.PromiseTime = reader.ReadInt64();
                        break;
                    case RuleValueType.ServiceAttendance:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            retval.ServiceAttendances.Add(id);
                        }

                        break;
                    case RuleValueType.DayOfWeek:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            DayOfWeek day = (DayOfWeek)reader.ReadByte();
                            retval.DaysOfWeek.Add(day);
                        }

                        break;
                    case RuleValueType.Priority:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            retval.Priorities.Add(id);
                        }

                        break;
                    case RuleValueType.Urgency:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            retval.Urgencies.Add(id);
                        }

                        break;
                    case RuleValueType.CallType:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            retval.CallTypes.Add(id);
                        }

                        break;
                    case RuleValueType.RegistrationTime:
                        operation = (FilterOperation)reader.ReadInt32();
                        time = reader.ReadInt64();
                        retval.TimeRegistrations.Add(operation, time);
                        break;
                    case RuleValueType.ServiceItem:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            retval.ServiceItems.Add(id);
                        }

                        break;
                    case RuleValueType.Service:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            retval.Services.Add(id);
                        }

                        break;
                    case RuleValueType.ClientPosition:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            int positionID = reader.ReadInt32();
                            retval.Positions.Add(positionID);
                        }

                        break;
                    case RuleValueType.ClientOrgStructure:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            int classID = reader.ReadInt32();
                            id = new Guid(reader.ReadString());
                            retval.OrganisationItems.Add(id);
                        }

                        break;
                    case RuleValueType.ParameterTemplate:
                        count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            id = new Guid(reader.ReadString());
                            //
                            int countData = reader.ReadInt32();
                            if (countData > 0)
                            {
                                byte[] valuesData = reader.ReadBytes(countData);
                            }
                            //                                
                            //TODO: Поддержка Form Values
                        }

                        break;
                }
            }
        }

        stream.Close();

        return retval;
    }

    internal class RuleValue
    {
        public string Price { get; set; }
        public long? PromiseTime { get; set; }
        public List<Guid> Services { get; } = new List<Guid>();
        public List<Guid> ServiceItems { get; } = new List<Guid>();
        public List<Guid> ServiceAttendances { get; } = new List<Guid>();
        public List<Guid> OrganisationItems { get; } = new List<Guid>();
        public List<DayOfWeek> DaysOfWeek { get; } = new List<DayOfWeek>();
        public List<Guid> Priorities { get; } = new List<Guid>();
        public List<Guid> Urgencies { get; } = new List<Guid>();
        public List<Guid> CallTypes { get; } = new List<Guid>();
        public List<int> Positions { get; } = new List<int>();

        public Dictionary<FilterOperation, long> TimeRegistrations { get; } =
            new Dictionary<FilterOperation, long>();
        
        public bool MatchCondition(Call call, User client, Guid[] orgItemIDs, DateTime userRegistrationDate)
        {
            long userRegistrationTicks = new TimeSpan(userRegistrationDate.Hour, userRegistrationDate.Minute, 0).Ticks;

            return (!Services.Any() || Services.Contains(call.CallService.Service.ID))
                   && (!ServiceItems.Any() || (call.CallService.ServiceItemID.HasValue && ServiceItems.Contains(call.CallService.ServiceItemID.Value)))
                   && (!ServiceAttendances.Any() || (call.CallService.ServiceAttendanceID.HasValue && ServiceAttendances.Contains(call.CallService.ServiceAttendanceID.Value)))
                   && (!OrganisationItems.Any() || OrganisationItems.Intersect(orgItemIDs).Any())
                   && (!Priorities.Any() || Priorities.Contains(call.PriorityID))
                   && (!Urgencies.Any() || (call.UrgencyID.HasValue && Urgencies.Contains(call.UrgencyID.Value)))
                   && (!CallTypes.Any() || CallTypes.Contains(call.CallTypeID))
                   && (!Positions.Any() || (client.PositionID.HasValue && Positions.Contains(client.PositionID.Value)))
                   && (!DaysOfWeek.Any() || DaysOfWeek.Contains(userRegistrationDate.DayOfWeek))
                   && (!TimeRegistrations.Any() || TimeRegistrations.All(x => RegistrationMatch(x.Key, x.Value, userRegistrationTicks)));
        }
    }

    internal sealed class Interval
    {
        #region constuctors

        private Interval(bool isWorkInterval, DateTime utcStartDate, DateTime utcEndDate)
            : this(isWorkInterval ? CalendarDayType.Work : CalendarDayType.Weekend,
                utcStartDate, utcEndDate)
        {
        }

        internal Interval(CalendarDayType dayType, DateTime utcStartDate, DateTime utcEndDate)
        {
            if (utcEndDate < utcStartDate)
                throw new NotSupportedException("startDate > endDate");
            //
            this.DayType = dayType;
            this.UtcStartDate = utcStartDate;
            this.UtcEndDate = utcEndDate;
        }

        internal Interval(CalendarDayType dayType, DateTime utcStartDate, TimeSpan duration)
        {
            if (duration.TotalMilliseconds < 0)
                throw new NotSupportedException("duration < 0");
            //
            this.DayType = dayType;
            this.UtcStartDate = utcStartDate;
            this.UtcEndDate = utcStartDate + duration;
        }

        #endregion

        #region properties

        private CalendarDayType DayType { get; set; }

        public bool IsWorkInterval
        {
            get { return CalendarDayTypeHelper.IsWorkDay(this.DayType); }
        }

        public DateTime UtcStartDate { get; private set; }
        public DateTime UtcEndDate { get; private set; }

        public TimeSpan Duration
        {
            get { return this.UtcEndDate - this.UtcStartDate; }
        }

        #endregion


        #region static method ShrinkIntervals

        internal static List<Interval> ShrinkIntervals(List<Interval> intervalsWithoutIntersect)
        {
            if (intervalsWithoutIntersect.Count <= 1)
                return intervalsWithoutIntersect;
            intervalsWithoutIntersect.Sort((x, y) => x.UtcStartDate.CompareTo(y.UtcStartDate));
            //
            List<Interval> retval = new List<Interval>();
            //
            Interval previousInterval = null;
            foreach (var interval in intervalsWithoutIntersect)
            {
                if (previousInterval == null)
                {
                    //определяем предыдущий элемент
                    previousInterval = interval;
                    continue;
                }

                //
                //стык границ
                if (interval.IsWorkInterval == previousInterval.IsWorkInterval &&
                    interval.UtcStartDate == previousInterval.UtcEndDate)
                {
                    //можно поглотить
                    previousInterval = new Interval(previousInterval.DayType, previousInterval.UtcStartDate,
                        interval.UtcEndDate);
                }
                else
                {
                    retval.Add(previousInterval);
                    previousInterval = interval;
                }
            }

            //
            if (previousInterval != null)
            {
                //последний элемент
                retval.Add(previousInterval);
            }

            //
            return retval;
        }

        #endregion

        #region static method ToIntervalsWithoutIntersect

        internal static List<Interval> ToIntervalsWithoutIntersect(List<Interval> intervalList,
            bool influenceNotWorkInterval)
        {
            if (intervalList.Count <= 1)
                return intervalList;
            //
            List<Interval> retval = new List<Interval>();
            //
            List<DateTime> pointList = new List<DateTime>();
            foreach (var interval in intervalList)
            {
                pointList.Add(interval.UtcStartDate);
                pointList.Add(interval.UtcEndDate);
            }

            pointList.Sort((x, y) => x.CompareTo(y));
            //
            DateTime? r1 = null;
            for (int i = 0; i < pointList.Count; i++)
            {
                DateTime r2 = pointList[i];
                if (r1.HasValue)
                {
                    List<Interval> intersectList = intervalList.FindAll(x =>
                        x.UtcStartDate <= r1.Value &&
                        x.UtcEndDate >= r2);
                    //
                    if (intersectList.Count == 0)
                    {
                        r1 = r2;
                        continue;
                    }

                    //
                    bool existsWork = false;
                    bool existsNotWork = false;
                    foreach (var interval in intersectList)
                        if (interval.IsWorkInterval)
                            existsWork = true;
                        else
                            existsNotWork = true;
                    //
                    bool isWorkRegion = existsWork;
                    if (influenceNotWorkInterval)
                        isWorkRegion &= !existsNotWork;
                    retval.Add(new Interval(isWorkRegion, r1.Value, r2));
                }

                //
                r1 = r2;
            }

            //
            return retval;
        }

        #endregion

        #region static method GetIntervalsByDay

        internal static List<Interval> GetIntervalsByDay(List<Interval> intervalList, DateTime utcDate)
        {
            if (intervalList.Count == 0)
                return intervalList;
            //
            DateTime utcStartDay = utcDate.Date;
            DateTime utcEndDay = utcStartDay.AddDays(1);
            //
            List<Interval> retval = new List<Interval>();
            foreach (var x in intervalList)
            {
                if (x.UtcStartDate <= utcStartDay && x.UtcEndDate >= utcStartDay && x.UtcEndDate <= utcEndDay)
                    retval.Add(new Interval(x.DayType, utcStartDay, x.UtcEndDate));
                else if (x.UtcStartDate >= utcStartDay && x.UtcEndDate <= utcEndDay)
                    retval.Add(x);
                else if (x.UtcStartDate >= utcStartDay && x.UtcStartDate <= utcEndDay && x.UtcEndDate >= utcEndDay)
                    retval.Add(new Interval(x.DayType, x.UtcStartDate, utcEndDay));
                else if (x.UtcStartDate <= utcStartDay && x.UtcEndDate >= utcEndDay)
                    retval.Add(new Interval(x.DayType, utcStartDay, utcEndDay));
                //остальные интервалы не пересекаются
            }

            //
            return retval;
        }

        #endregion

        #region static method GetWorkInterval

        internal static TimeSpan GetWorkInterval(List<Interval> intervalList)
        {
            TimeSpan retval = TimeSpan.Zero;
            //
            foreach (var interval in intervalList)
                if (interval.IsWorkInterval)
                    retval = retval.Add(interval.Duration);
            //
            return retval;
        }

        #endregion
    }

    private sealed class CalendarDayTypeHelper
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
    
    public enum CalendarDayType : byte
    {
        Unknown = 0,
        Work = 1,
        WorkShort = 2,
        WorkLong = 3,
        WorkPreHoliday = 4,
        Holiday = 5,
        Weekend = 6,
        WeekendByTemplate = 7
    }
    
    public enum CountPlannedDateFrom : byte
    {
        SinceDateCreation = 0,
        SinceDateRegistration = 1,
        SinceNow = 2
    }
}