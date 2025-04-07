using AutoMapper;
using InfraManager.BLL.Users;
using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using InfraManager.BLL.Settings.Calendar;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.CalendarService;
//  TODO: скопировнный из легнаси код нужно рефактрить
    public class CalendarServiceBLL : ICalendarServiceBLL, ISelfRegisteredService<ICalendarServiceBLL>
    {
        private readonly ISupportSettingsCalendarBLL _supportSettingsCalendar;
        private readonly IReadonlyRepository<CalendarWorkSchedule> _calendarWorkSchedulesRepository;
        private readonly IReadonlyRepository<CalendarWorkScheduleItem> _calendarWorkScheduleItemsRepository;
        private readonly IReadonlyRepository<CalendarWorkScheduleItemExclusion> _calendarWorkScheduleItemExclusionsRepository;
        private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _calendarWorkScheduleDefaultsRepository;
        private readonly IReadonlyRepository<CalendarWeekend> _calendarWeekendsRepository;
        private readonly IReadonlyRepository<CalendarHoliday> _calendarHolidaysRepository;
        private readonly IReadonlyRepository<CalendarHolidayItem> _calendarHolidayItemsRepository;
        private readonly IReadonlyRepository<GroupUser> _groupUserRepository;
        private readonly IFinder<TimeZone> _finderTimeZone;
        private readonly IUserBLL _userBLL;
        private readonly ISubdivisionBLL _subdivisionBLL;
        private readonly IReadonlyRepository<Workplace> _workplaceRepository;
        private readonly IMapper _mapper;
        private readonly IFinder<Organization> _organizationFinder;
        private readonly IReadonlyRepository<CalendarWorkScheduleShift> _calendarWorkScheduleShifts;

        public CalendarServiceBLL(
            ISupportSettingsCalendarBLL supportSettingsCalendar,
            IReadonlyRepository<CalendarWorkSchedule> calendarWorkSchedulesRepository,
            IReadonlyRepository<CalendarWorkScheduleItem> calendarWorkScheduleItemsRepository,
            IReadonlyRepository<CalendarWorkScheduleItemExclusion> calendarWorkScheduleItemExclusionsRepository,
            IReadonlyRepository<CalendarWorkScheduleDefault> calendarWorkScheduleDefaultsRepository,
            IReadonlyRepository<CalendarWeekend> calendarWeekendsRepository,
            IReadonlyRepository<CalendarHoliday> calendarHolidaysRepository,
            IReadonlyRepository<CalendarHolidayItem> calendarHolidayItemsRepository,
            IFinder<TimeZone> finderTimeZone,
            IUserBLL userBLL,
            ISubdivisionBLL subdivisionBLL,
            IReadonlyRepository<Workplace> workplaceRepository,
            IMapper mapper,
            IReadonlyRepository<GroupUser> groupUserRepository,
            IFinder<Organization> organizationFinder,
            IReadonlyRepository<CalendarWorkScheduleShift> calendarWorkScheduleShifts)
        {
            _supportSettingsCalendar = supportSettingsCalendar;
            _calendarWorkSchedulesRepository = calendarWorkSchedulesRepository;
            _calendarWorkScheduleItemsRepository = calendarWorkScheduleItemsRepository;
            _calendarWorkScheduleItemExclusionsRepository = calendarWorkScheduleItemExclusionsRepository;
            _calendarWorkScheduleDefaultsRepository = calendarWorkScheduleDefaultsRepository;
            _calendarWeekendsRepository = calendarWeekendsRepository;
            _calendarHolidaysRepository = calendarHolidaysRepository;
            _calendarHolidayItemsRepository = calendarHolidayItemsRepository;
            _finderTimeZone = finderTimeZone;
            _userBLL = userBLL;
            _subdivisionBLL = subdivisionBLL;
            _workplaceRepository = workplaceRepository;
            _mapper = mapper;
            _groupUserRepository = groupUserRepository;
            _organizationFinder = organizationFinder;
            _calendarWorkScheduleShifts = calendarWorkScheduleShifts;
        }

        public async Task<TimeSpan> GetWorkTimeByCalendarAsync(WorkTimeByCalendarData data, CancellationToken cancellationToken = default)
        {
            if (data.utcFinishDate <= data.utcStartDate)
                return TimeSpan.Zero;

            var workCalendar = data.calendarWorkScheduleID == null ? null : (await _calendarWorkSchedulesRepository.FirstOrDefaultAsync(x => x.ID == data.calendarWorkScheduleID.Value, cancellationToken));
            var calendarTimeZone = string.IsNullOrEmpty(data.calendarTimeZoneID) ? null : (await _finderTimeZone.FindAsync(data.calendarTimeZoneID, cancellationToken));

            return await GetWorkTimeInternal(data.utcStartDate, data.utcFinishDate, workCalendar, calendarTimeZone, cancellationToken);
        }
        public async Task<TimeSpan> GetWorkTimeByUserAsync(WorkTimeByUserData data, CancellationToken cancellationToken = default)
        {
            (CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone) = await GetUserCalendar(data.UserID, cancellationToken);

            return await GetWorkTimeInternal(data.utcStartDate, data.utcFinishDate, workCalendar, calendarTimeZone, cancellationToken);
        }

        public async Task<DateTime> GetWorkDateByCalendarAsync(WorkDateByCalendarData data, CancellationToken cancellationToken = default)
        {
            var workCalendar = data.calendarWorkScheduleID == null ? null : (await _calendarWorkSchedulesRepository.FirstOrDefaultAsync(x => x.ID == data.calendarWorkScheduleID.Value, cancellationToken));
            var calendarTimeZone = await GetDefaultTimeZoneIfNullAsync(await _finderTimeZone.FindAsync(data.calendarTimeZoneID, cancellationToken), cancellationToken);

            return await GetWorkDateInternal(data.utcStartDate, data.Duration, workCalendar, calendarTimeZone, cancellationToken);
        }

        public async Task<DateTime> GetWorkDateByUserAsync(WorkDateByUserData data, CancellationToken cancellationToken = default)
        {
            (CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone) = await GetUserCalendar(data.UserID, cancellationToken);
            return await GetWorkDateInternal(data.utcStartDate, data.Duration, workCalendar, calendarTimeZone, cancellationToken);
        }
        
        public async Task<DateTime> GetUtcFinishDateByCalendarAsync(DateTime startDate, TimeSpan duration, CalendarWorkSchedule calendar, TimeZone calendarTimeZone)
        {
            DateTime dt = startDate;
            var ts = duration;
            
            var exclusionIntervals = new List<CalendarInterval>();
            var fixedExclusionIntervals = new List<CalendarInterval>();
            
            while (ts > TimeSpan.Zero)
            {
                exclusionIntervals.Clear();
                exclusionIntervals.AddRange(fixedExclusionIntervals);
                
                DateTime utcBeginOfDay = dt.Date;
                DateTime utcBeginOfNextDay = GetNextDay(utcBeginOfDay).Date;
                CalendarInterval day = await GetDayInfoAsync(utcBeginOfDay, calendar, calendarTimeZone, exclusionIntervals);//рабочее время по дню
                CalendarInterval nextDay = await GetDayInfoAsync(utcBeginOfNextDay, calendar, calendarTimeZone, exclusionIntervals);//рабочее время по следующему дню, в случае перекрытия
                
                if (calendar != null)
                {
                    var item = calendar.WorkScheduleItems.First(item => item.DayOfYear == utcBeginOfDay.DayOfYear);
                    //добавление исключений дня
                    foreach (var itemExclusion in item.WorkScheduleItemExclusions)
                    {
                        DateTime utc_start = utcBeginOfDay + itemExclusion.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME);
                        DateTime utc_end = utc_start + TimeSpan.FromMinutes(itemExclusion.TimeSpanInMinutes);
                        exclusionIntervals.Add(new CalendarInterval
                            (CalendarDayType.Holiday,
                            TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                            TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
                    }

                    item = calendar.WorkScheduleItems.First(item => item.DayOfYear == utcBeginOfNextDay.DayOfYear);
                    //добавление исключений следующего дня
                    foreach (var nextItemExclusion in item.WorkScheduleItemExclusions)
                    {
                        DateTime utc_start = utcBeginOfNextDay + nextItemExclusion.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME); ;
                        DateTime utc_end = utc_start + TimeSpan.FromMinutes(nextItemExclusion.TimeSpanInMinutes);
                        exclusionIntervals.Add(new CalendarInterval
                            (CalendarDayType.Holiday,
                            TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                            TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
                    }
                }
                //
                List<CalendarInterval> dayIntervals = new List<CalendarInterval>(exclusionIntervals);//все интервалы-исключения
                dayIntervals.Add(day);//текущий день
                dayIntervals.Add(nextDay);//следующий день, может кусочек есть
                dayIntervals = GetIntervalsByDay(dayIntervals, utcBeginOfDay);//обрезаем все интевалы до текущего дня
                dayIntervals = ToIntervalsWithoutIntersect(dayIntervals, true);//приводим к непересекающимся
                //
                for (int i = 0; i < dayIntervals.Count; i++)
                {
                    var interval = dayIntervals[i];
                    if (dt > interval.UtcEndDate && i < dayIntervals.Count - 1)
                    {
                        continue;
                    }
                    if (!interval.IsWorkDay())
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
                            return dt; // returns PromisedDate
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
                if (utcBeginOfDay == dt.Date)//GetIntervalsByDay может перевести исходный в следующий день
                    dt = GetNextDay(dt);
            }
            //
            return dt;
        }

        private async Task<CalendarInterval> GetDayInfoAsync(DateTime utcDate, CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone, List<CalendarInterval> defaultExclusions)
        {
            var _defaultCalendar = await _calendarWorkScheduleDefaultsRepository
                    .With(x => x.CalendarHoliday)
                    .With(x => x.CalendarWeekend)
                    .With(x => x.TimeZone)
                        .ThenWithMany(x => x.TimeZoneAdjustmentRules)
                    .FirstOrDefaultAsync();

            calendarTimeZone = calendarTimeZone ?? _defaultCalendar.TimeZone;
            //
            DateTime date = calendarTimeZone.ConvertTimeFromUtc(utcDate);//момент времени, приведенный к временной зоне календаря
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
                    var item = workCalendar.WorkScheduleItems.FirstOrDefault(item => item.DayOfYear == (short)date.DayOfYear);

                    if (item != null)
                    {//есть такой день календаря
                     //добавление исключений дня
                        foreach (var itemExclusion in item.WorkScheduleItemExclusions)
                        {
                            var startTimeSpan = itemExclusion.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME);
                            var utc_start = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, startTimeSpan.Hours, startTimeSpan.Minutes, startTimeSpan.Seconds, startTimeSpan.Milliseconds, DateTimeKind.Unspecified);
                            DateTime utc_end = utc_start + TimeSpan.FromMinutes(itemExclusion.TimeSpanInMinutes);
                            defaultExclusions.Add(new CalendarInterval
                                (CalendarDayType.Holiday,
                                TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                                TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
                        }
                        return CreateInterval((CalendarDayType)item.DayType, date, item.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME), TimeSpan.FromMinutes(item.TimeSpanInMinutes), calendarTimeZone);
                    }
                }
            }
            //
            //не задан календарь работы или день в нем не найден - берем из календаря по умолчанию
            var def = _defaultCalendar;
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
                DateTime utc_start = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, startSpan.Hours, startSpan.Minutes, startSpan.Seconds, startSpan.Milliseconds, DateTimeKind.Unspecified);
                DateTime utc_end = utc_start + TimeSpan.FromMinutes(def.ExclusionTimeSpanInMinutes.Value);
                defaultExclusions.Add(new CalendarInterval
                    (CalendarDayType.Holiday,
                    TimeZoneExtensions.ConvertTimeToUtc(utc_start, calendarTimeZone),
                    TimeZoneExtensions.ConvertTimeToUtc(utc_end, calendarTimeZone)));
            }
            //
            return CreateInterval(dayType, date, def.TimeStart.Subtract(TimeZoneExtensions.MIN_UTC_TIME), TimeSpan.FromMinutes(def.TimeSpanInMinutes), calendarTimeZone);
        }

        public async Task<DateTime> GetDatePromisedAsync(long timeToDo, CancellationToken cancellationToken = default)
        {
            var supportSettingsCalendar = await _supportSettingsCalendar.GetAsync(cancellationToken);

            var timeToDoMinutes = new TimeSpan(timeToDo).TotalMinutes;
            
            var firstPart = (supportSettingsCalendar.DinnerTimeStart - supportSettingsCalendar.TimeStart).Value.TotalMinutes;
            var secondPart = (supportSettingsCalendar.TimeStartEnd - supportSettingsCalendar.DinnerTimeEnd).Value.TotalMinutes;
            
            var entryPointDate = GetEntrypointDate(DateTime.UtcNow, supportSettingsCalendar);

            while (timeToDoMinutes > 0)
            {
                timeToDoMinutes -= firstPart;

                if (timeToDoMinutes <= 0)
                {
                    var time = supportSettingsCalendar.DinnerTimeStart.Value.TimeOfDay.Subtract(
                        TimeSpan.FromMinutes(timeToDoMinutes * (-1)));
                    
                    return new DateTime(entryPointDate.Year, entryPointDate.Month, entryPointDate.Day, 
                        time.Hours, time.Minutes, 0);
                }

                timeToDoMinutes -= secondPart;

                if (timeToDoMinutes <= 0)
                {
                    var time = supportSettingsCalendar.TimeStartEnd.TimeOfDay.Subtract(
                        TimeSpan.FromMinutes(timeToDoMinutes * (-1)));
                    
                    return new DateTime(entryPointDate.Year, entryPointDate.Month, entryPointDate.Day, 
                        time.Hours, time.Minutes, 0);
                }

                entryPointDate = entryPointDate.AddDays(1);
            }

            return entryPointDate;
        }
        
        // TODO: Логика взята из легаси. Требуется рефакторинг.
        public async Task<TimeSpan> GetWorkTimeByGroupAsync(WorkTimeByGroupData data, CancellationToken cancellationToken = default)
        {
            var usersID = (await _groupUserRepository
                    .ToArrayAsync(x => x.GroupID == data.GroupID, cancellationToken)
                ).Select(x => x.UserID);

            var intervalList = new List<CalendarInterval>();
            
            foreach (var userID in usersID)
            {
                var (userCalendar, userTimeZone) = await GetUserCalendar(userID, cancellationToken);
                userTimeZone = await GetDefaultTimeZoneIfNullAsync(userTimeZone, cancellationToken);
                var userWorkTimeIntervals = await GetWorkTimeIntervalsAsync(data.utcStartDate, data.utcFinishDate, userCalendar, userTimeZone, cancellationToken);
                intervalList.AddRange(userWorkTimeIntervals);
            }

            intervalList = ToIntervalsWithoutIntersect(intervalList, true);

            return intervalList
                .Where(x => x.IsWorkDay())
                .Select(x => x.Duration)
                .Aggregate(TimeSpan.Zero, (x, y) => x.Add(y));
        }

        private async Task<(CalendarWorkSchedule, TimeZone)> GetUserCalendar(Guid userID, CancellationToken cancellationToken = default)
        {
            CalendarWorkSchedule workCalendar = null;
            TimeZone calendarTimeZone = null;
            
            var user = await _userBLL.DetailsAsync(userID, cancellationToken) ??
                       throw new InvalidObjectException($"User {userID} not found");

            if (user.CalendarWorkScheduleId != null)
            {
                workCalendar =
                    await _calendarWorkSchedulesRepository.FirstOrDefaultAsync(
                        x => x.ID == user.CalendarWorkScheduleId.Value, cancellationToken);
            }
            else if(user.SubdivisionID != null)
            {
                var subsidary = await _subdivisionBLL.GetDetailsAsync(user.SubdivisionID.Value, cancellationToken);
                
                while(subsidary != null && subsidary.CalendarWorkScheduleID == null && subsidary.SubdivisionID != null)
                {
                    subsidary = await _subdivisionBLL.GetDetailsAsync(subsidary.SubdivisionID.Value, cancellationToken);
                }

                if (subsidary != null && subsidary.CalendarWorkScheduleID != null)
                {
                    workCalendar =
                        await _calendarWorkSchedulesRepository.FirstOrDefaultAsync(
                            x => x.ID == subsidary.CalendarWorkScheduleID.Value, cancellationToken);
                }
            }

            if (workCalendar == null && user.OrganizationID != null)
            {
                var organization = await _organizationFinder.FindAsync(user.OrganizationID, cancellationToken);
                if (organization?.CalendarWorkScheduleId != null)
                {
                    workCalendar = await _calendarWorkSchedulesRepository.FirstOrDefaultAsync(x => x.ID == organization.CalendarWorkScheduleId.Value, cancellationToken);
                }
            }

            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                calendarTimeZone = await _finderTimeZone.FindAsync(user.TimeZoneId, cancellationToken);
            }
            else if(user.WorkplaceID!=null)
            {
                var workPlace =
                    await _workplaceRepository.With(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                        .FirstOrDefaultAsync(x => x.ID == user.WorkplaceID.Value, cancellationToken);

                if (!string.IsNullOrWhiteSpace(workPlace?.Room?.Floor?.Building?.TimeZoneID))
                {
                    calendarTimeZone =
                        await _finderTimeZone.FindAsync(workPlace.Room.Floor.Building.TimeZoneID, cancellationToken);
                }
            }

            return (workCalendar, calendarTimeZone);
        }

        private async Task<TimeSpan> GetWorkTimeInternal(DateTime utcStartDate, DateTime utcFinishDate,
            CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone, CancellationToken cancellationToken)
        {
            calendarTimeZone = await GetDefaultTimeZoneIfNullAsync(calendarTimeZone, cancellationToken);

            var intervalList = await GetWorkTimeIntervalsAsync(utcStartDate, utcFinishDate, workCalendar, calendarTimeZone, cancellationToken);

            intervalList = ToIntervalsWithoutIntersect(intervalList, true);
            return intervalList.Where(x => x.IsWorkDay()).Select(x => x.Duration).Aggregate(TimeSpan.Zero, (x, y) => x.Add(y));
        }

        // TODO: рефактор логики, скопированной из легаси.
        private async Task<DateTime> GetWorkDateInternal(DateTime utcStartDate, TimeSpan duration, CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone, CancellationToken cancellationToken)
        {
            var dt = utcStartDate;
            var ts = duration;
            //            
            var exclusionIntervals = new List<CalendarInterval>();
            var fixedExclusionIntervals = new List<CalendarInterval>();
            
            while (ts > TimeSpan.Zero)
            {
                exclusionIntervals.Clear();
                exclusionIntervals.AddRange(fixedExclusionIntervals);
                //
                DateTime utcBeginOfDay = dt.Date;
                DateTime utcBeginOfNextDay = GetNextDay(utcBeginOfDay).Date;
                CalendarInterval day = await GetDayInfoAsync(utcBeginOfDay, workCalendar, calendarTimeZone, exclusionIntervals, cancellationToken);//рабочее время по дню
                CalendarInterval nextDay = await GetDayInfoAsync(utcBeginOfNextDay, workCalendar, calendarTimeZone, exclusionIntervals, cancellationToken);//рабочее время по следующему дню, в случае перекрытия
                //
                if (workCalendar != null)
                {
                    //добавление исключений дня
                    foreach (var itemExclusion in await _calendarWorkScheduleItemExclusionsRepository.ToArrayAsync(x=>x.CalendarWorkScheduleID == workCalendar.ID && x.DayOfYear == utcBeginOfDay.DayOfYear, cancellationToken))
                    {
                        DateTime utc_start = utcBeginOfDay + itemExclusion.TimeStart.DateTime2Time();
                        DateTime utc_end = utc_start + itemExclusion.TimeSpanInMinutes.ToTimeSpan();
                        exclusionIntervals.Add(new CalendarInterval(CalendarDayType.Holiday,ConvertTimeToUtc(utc_start, calendarTimeZone),ConvertTimeToUtc(utc_end, calendarTimeZone)));
                    }

                    //добавление исключений следующего дня
                    foreach (var nextItemExclusion in await _calendarWorkScheduleItemExclusionsRepository.ToArrayAsync(x => x.CalendarWorkScheduleID == workCalendar.ID && x.DayOfYear == utcBeginOfNextDay.DayOfYear, cancellationToken))
                    {
                        DateTime utc_start = utcBeginOfNextDay + nextItemExclusion.TimeStart.DateTime2Time();
                        DateTime utc_end = utc_start + nextItemExclusion.TimeSpanInMinutes.ToTimeSpan();
                        exclusionIntervals.Add(new CalendarInterval(CalendarDayType.Holiday,ConvertTimeToUtc(utc_start, calendarTimeZone),ConvertTimeToUtc(utc_end, calendarTimeZone)));
                    }
                }
                //
                List<CalendarInterval> dayIntervals = new List<CalendarInterval>(exclusionIntervals);//все интервалы-исключения
                dayIntervals.Add(day);//текущий день
                dayIntervals.Add(nextDay);//следующий день, может кусочек есть
                dayIntervals = GetIntervalsByDay(dayIntervals, utcBeginOfDay);//обрезаем все интевалы до текущего дня
                dayIntervals = ToIntervalsWithoutIntersect(dayIntervals, true);//приводим к непересекающимся
                //
                for (int i = 0; i < dayIntervals.Count; i++)
                {
                    var interval = dayIntervals[i];
                    if (dt > interval.UtcEndDate && i < dayIntervals.Count - 1)
                    {
                        continue;
                    }
                    if (!interval.IsWorkDay())
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
                if (utcBeginOfDay == dt.Date)//GetIntervalsByDay может перевести исходный в следующий день
                    dt = GetNextDay(dt);
            }
            //
            return dt;
        }


        // TODO: рефактор логики, скопированной из легаси.
        private async Task<List<CalendarInterval>> GetWorkTimeIntervalsAsync(DateTime utcStartDate, DateTime utcFinishDate, CalendarWorkSchedule workCalendar, TimeZone calendarTimeZone, CancellationToken cancellationToken = default)
        {
            if (utcFinishDate <= utcStartDate)
                return new List<CalendarInterval>();
            //
            List<CalendarInterval> intervalList = new List<CalendarInterval>();
            Action<CalendarDayType, DateTime, DateTime> processInterval = new Action<CalendarDayType, DateTime, DateTime>((dayType, utcStart, utcEnd) =>
            {
                while (utcStartDate.Date > utcStart.Date)
                {
                    //достался интервал с предыдущего дня
                    utcStart = GetNextDay(utcStart);
                }
                if (utcStartDate.Date == utcStart.Date)
                {
                    //начальный день
                    if (utcStart >= utcStartDate)
                    {
                        //начало работы после начала интервала
                        //все отлично
                    }
                    else if (utcEnd > utcStartDate)
                    {
                        //конец работы после начала интервала
                        utcStart = utcStartDate;
                    }
                    else
                    {
                        //конец работы до начала интервала
                        //выкалываем
                        utcEnd = utcStart;
                    }
                }
                if (utcFinishDate.Date <= utcEnd.Date)
                {
                    //конечный день
                    if (utcStart >= utcFinishDate)
                    {
                        //начало работы после конца интервала
                        //выкалываем
                        utcEnd = utcStart;
                    }
                    else if (utcEnd > utcFinishDate)
                    {
                        //конец работы после конца интервала
                        utcEnd = utcFinishDate;
                    }
                    else
                    {
                        //конец работы до конца интервала
                        //все отлично
                    }
                }
                //
                if (utcStart < utcEnd)
                {
                    //не выколота
                    var interval = new CalendarInterval(dayType, utcStart, utcEnd );
                    intervalList.Add(interval);
                }
            });
            //
            var utcDate = utcStartDate;
            List<CalendarInterval> defaultExclusions = new List<CalendarInterval>();
            while (utcDate < utcFinishDate)
            {
                CalendarInterval day = await GetDayInfoAsync(utcDate, workCalendar, calendarTimeZone, defaultExclusions, cancellationToken);
                //                
                processInterval(day.CalendarDayType, day.UtcStartDate, day.UtcEndDate);
                utcDate = GetUtcNextDayByTimeZone(utcDate, calendarTimeZone);
            }

            foreach (var e in defaultExclusions)
                processInterval(e.IsWorkDay() ? CalendarDayType.Work : CalendarDayType.Weekend, e.UtcStartDate, e.UtcEndDate);
            //           
            return intervalList;
        }

        private DateTime GetUtcNextDayByTimeZone(DateTime utcDate, TimeZone calendarTimeZone)
        {
            var date = ConvertTimeFromUtc(utcDate, calendarTimeZone);//момент времени, приведенный к временной зоне календаря
            var nextDate = (new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind)).AddDays(1);
            //
            return ConvertTimeToUtc(nextDate, calendarTimeZone);
        }

        private TimeZone CreateTimeZone(TimeZoneInfo timeZoneInfo)
        { 
           var timezone = new TimeZone
           {
               BaseUtcOffsetInMinutes = (short)timeZoneInfo.BaseUtcOffset.TotalMinutes,
               ID = timeZoneInfo.Id,
               Name = timeZoneInfo.DisplayName,
               SupportsDaylightSavingTime = timeZoneInfo.SupportsDaylightSavingTime
           };

            foreach(var rule in timeZoneInfo.GetAdjustmentRules().Select(x => _mapper.Map<TimeZoneAdjustmentRule>(x)))
            {
                timezone.TimeZoneAdjustmentRules.Add(rule);
            }

            return timezone;
        }

        private TimeZone UTC => CreateTimeZone(TimeZoneInfo.Utc);
        private TimeZone Local => CreateTimeZone(TimeZoneInfo.Local);


        private DateTime ConvertTimeFromUtc(DateTime date, TimeZone calendarTimeZone)
        {
            return ConvertTime(date, UTC, calendarTimeZone);
        }

        private DateTime ConvertTime(DateTime date, TimeZone sourceTz, TimeZone targetTz)
        {
            if (sourceTz == null)
                throw new ArgumentNullException(nameof(sourceTz));
            if (targetTz == null)
                throw new ArgumentNullException(nameof(targetTz));

            DateTimeKind correspondingKind = GetCorrespondingKind(sourceTz);
            var adjustmentRuleForTime = sourceTz.GetAdjustmentRuleForTime(date);

            TimeSpan baseUtcOffset = sourceTz.BaseUtcOffset();
            if (adjustmentRuleForTime != null)
            {
                bool flag = false;
                CalendarDaylightTime daylightTime = GetDaylightTime(date.Year, adjustmentRuleForTime);
                //мы не следим особо за DateTimeKind, вот и не будем ругаться
                //if (((flags & TimeZoneInfoOptions.NoThrowOnInvalidTime) == 0) && GetIsInvalidTime(dateTime, adjustmentRuleForTime, daylightTime))
                //    throw new ArgumentException("dateTime");
                //
                flag = GetIsDaylightSavings(date, adjustmentRuleForTime, daylightTime);
                baseUtcOffset += flag ? adjustmentRuleForTime.DaylightDelta() : TimeSpan.Zero;
            }
            DateTimeKind kind = GetCorrespondingKind(targetTz);
            if (((date.Kind != DateTimeKind.Unspecified) && (correspondingKind != DateTimeKind.Unspecified)) && (correspondingKind == kind))
                return date;
            //
            long ticks = date.Ticks - baseUtcOffset.Ticks;
            bool isAmbiguousLocalDst = false;
            DateTime time2 = ConvertUtcToTimeZone(ticks, targetTz, out isAmbiguousLocalDst);
            if (kind == DateTimeKind.Local)
            {
                //invoke internal method
                //return new DateTime(time2.Ticks, DateTimeKind.Local, isAmbiguousLocalDst);
                var ci = typeof(DateTime).GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, new Type[] { typeof(long), typeof(DateTimeKind), typeof(bool) }, null);
                return (DateTime)ci.Invoke(
                    System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.NonPublic,
                    null,
                    new object[] { time2.Ticks, DateTimeKind.Local, isAmbiguousLocalDst },
                    System.Globalization.CultureInfo.CurrentCulture);
            }
            else
                return new DateTime(time2.Ticks, kind);

        }

        private bool GetIsDaylightSavings(DateTime time, TimeZoneAdjustmentRule rule, CalendarDaylightTime daylightTime)
        {
            DateTime time2;
            DateTime end;
            if (rule == null)
                return false;
            //
            if (time.Kind == DateTimeKind.Local)
            {
                time2 = daylightTime.Start + daylightTime.Delta;
                end = daylightTime.End;
            }
            else
            {
                bool flag = rule.DaylightDelta() > TimeSpan.Zero;
                time2 = daylightTime.Start + (flag ? rule.DaylightDelta() : TimeSpan.Zero);
                end = daylightTime.End + (flag ? -rule.DaylightDelta() : TimeSpan.Zero);
            }
            bool flag2 = CheckIsDst(time2, time, end);
            if ((flag2 && (time.Kind == DateTimeKind.Local)) && GetIsAmbiguousTime(time, rule, daylightTime))
            {
                //invoke internal method
                //flag2 = time.IsAmbiguousDaylightSavingTime();
                var mi = typeof(DateTime).GetMethod("IsAmbiguousDaylightSavingTime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                flag2 = (bool)mi.Invoke(time, null);
            }
            return flag2;
        }

        private CalendarDaylightTime GetDaylightTime(int year, TimeZoneAdjustmentRule rule)
        {
            DateTime start = TransitionTimeToDateTime(year, rule.TransitionStart_IsFixedDateRule, rule.TransitionStart_Month, rule.TransitionStart_Day, rule.TransitionStart_TimeOfDay.DateTime2Time(), rule.TransitionStart_Week, rule.TransitionStart_DayOfWeek);
            DateTime end = TransitionTimeToDateTime(year, rule.TransitionEnd_IsFixedDateRule, rule.TransitionEnd_Month, rule.TransitionEnd_Day, rule.TransitionEnd_TimeOfDay.DateTime2Time(), rule.TransitionEnd_Week, rule.TransitionEnd_DayOfWeek);
            //
            return new CalendarDaylightTime(start, end, rule.DaylightDelta());
        }

        private DateTime TransitionTimeToDateTime(int year, bool isFixedDateRule, byte month, byte? day, TimeSpan timeOfDay, byte? week, byte? dayOfWeek)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (isFixedDateRule)
            {
                if (!day.HasValue)
                    throw new NotSupportedException("bad rule");
                //                
                return new DateTime(year, month, (daysInMonth < day.Value) ? daysInMonth : day.Value, timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds, timeOfDay.Milliseconds);
            }
            if (!week.HasValue || !dayOfWeek.HasValue)
                throw new NotSupportedException("bad rule");
            //
            DateTime time;
            if (week.Value <= 4)
            {
                time = new DateTime(year, month, 1, timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds, timeOfDay.Milliseconds);
                int timeDayOfWeek = (int)time.DayOfWeek;
                int num3 = ((int)dayOfWeek) - timeDayOfWeek;
                if (num3 < 0)
                    num3 += 7;
                num3 += 7 * (week.Value - 1);
                if (num3 > 0)
                    time = time.AddDays((double)num3);
                return time;
            }
            time = new DateTime(year, month, daysInMonth, timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds, timeOfDay.Milliseconds);
            int num6 = (int)(time.DayOfWeek - (DayOfWeek)dayOfWeek);
            if (num6 < 0)
                num6 += 7;
            if (num6 > 0)
                time = time.AddDays((double)-num6);
            return time;
        }

        private DateTime ConvertTimeToUtc(DateTime date, TimeZone calendarTimeZone)
        {
            return ConvertTime(date, calendarTimeZone, UTC);
        }

        // TODO: рефактор логики, скопированной из легаси.
        private async Task<CalendarInterval> GetDayInfoAsync(
            DateTime utcDate,
            CalendarWorkSchedule workCalendar,
            TimeZone calendarTimeZone,
            List<CalendarInterval> defaultExclusions,
            CancellationToken cancellationToken = default)
        {
            if (calendarTimeZone == null)
                calendarTimeZone = await GetDefaultTimeZoneIfNullAsync(calendarTimeZone, cancellationToken);
            //
            DateTime date = ConvertTimeFromUtc(utcDate, calendarTimeZone);//момент времени, приведенный к временной зоне календаря
            if (workCalendar != null)
            {
                //задан календарь работы
                //содержится ли такой день в этом календаре?
                if (workCalendar.Year != date.Year)
                {
                    //поиск подходящего календаря
                    workCalendar = await _calendarWorkSchedulesRepository.FirstOrDefaultAsync(x => x.Year == date.Year && x.Name == workCalendar.Name, cancellationToken);
                }
                //
                if (workCalendar != null && workCalendar.Year == date.Year)
                {
                    //поиск дня
                    CalendarWorkScheduleItem item = await _calendarWorkScheduleItemsRepository.FirstOrDefaultAsync(x => x.CalendarWorkScheduleID == workCalendar.ID && x.DayOfYear == (short)date.DayOfYear, cancellationToken);
                    if (item != null)
                    {//есть такой день календаря
                     //добавление исключений дня
                        foreach (var itemExclusion in (await _calendarWorkScheduleItemExclusionsRepository.ToArrayAsync(x => x.CalendarWorkScheduleID == item.CalendarWorkScheduleID && x.DayOfYear == item.DayOfYear, cancellationToken)))
                        {
                            DateTime utc_start = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, itemExclusion.TimeStart.DateTime2Time().Hours, itemExclusion.TimeStart.DateTime2Time().Minutes, itemExclusion.TimeStart.DateTime2Time().Seconds, itemExclusion.TimeStart.DateTime2Time().Milliseconds, DateTimeKind.Unspecified);
                            DateTime utc_end = utc_start + itemExclusion.TimeSpanInMinutes.ToTimeSpan();
                            defaultExclusions.Add(new CalendarInterval(CalendarDayType.Holiday,ConvertTimeToUtc(utc_start, calendarTimeZone),ConvertTimeToUtc(utc_end, calendarTimeZone)));
                        }

                        // добавление отклонений от графика
                        var scheduleShifts = await _calendarWorkScheduleShifts
                            .WithMany(x => x.WorkScheduleShiftExclusions)
                            .ToArrayAsync(x => x.CalendarWorkScheduleID == item.CalendarWorkScheduleID, cancellationToken);
                        foreach (var scheduleShift in scheduleShifts)
                        {
                            var utcStart = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, scheduleShift.TimeStart.DateTime2Time().Hours, scheduleShift.TimeStart.DateTime2Time().Minutes, scheduleShift.TimeStart.DateTime2Time().Seconds, scheduleShift.TimeStart.DateTime2Time().Milliseconds, DateTimeKind.Unspecified);
                            var utcEnd = utcStart + scheduleShift.TimeSpanInMinutes.ToTimeSpan();
                            defaultExclusions.Add(new CalendarInterval(CalendarDayType.Unknown,ConvertTimeToUtc(utcStart, calendarTimeZone),ConvertTimeToUtc(utcEnd, calendarTimeZone)));
                        }

                        return CreateInterval((CalendarDayType)item.DayType, date, item.TimeStart.DateTime2Time(), item.TimeSpanInMinutes.ToTimeSpan(), calendarTimeZone);
                    }
                }
            }
            //
            //не задан календарь работы или день в нем не найден - берем из календаря по умолчанию
            var defCalendar = await _calendarWorkScheduleDefaultsRepository.FirstOrDefaultAsync(cancellationToken);
            var defWeekend = defCalendar?.CalendarWeekendID == null ? null : await _calendarWeekendsRepository.FirstOrDefaultAsync(x => x.ID == defCalendar.CalendarWeekendID, cancellationToken);
            var defHoliday = defCalendar?.CalendarHolidayID == null ? null : await _calendarHolidaysRepository.FirstOrDefaultAsync(x => x.ID == defCalendar.CalendarHolidayID, cancellationToken);
            var defHolidays = defHoliday == null ? null : await _calendarHolidayItemsRepository.ToArrayAsync(x => x.CalendarHolidayID == defHoliday.ID, cancellationToken);
            //
            CalendarDayType dayType = CalendarDayType.Work;
            if (defWeekend != null && defWeekend.WeekendList()[date.DayOfWeek])
                dayType = CalendarDayType.Weekend;
            if (dayType.IsWorkDay() && defHolidays != null)
                foreach (var h in defHolidays)
                    if (h.Month == (Month)date.Month && h.Day == date.Day)
                    {
                        dayType = CalendarDayType.Holiday;
                        break;
                    }
            //
            //добавление исключений дня
            if (defaultExclusions != null && defCalendar.DinnerTimeStart.HasValue && defCalendar.ExclusionTimeSpanInMinutes.HasValue)
            {
                DateTime utc_start = new DateTime(
                    utcDate.Year,
                    utcDate.Month,
                    utcDate.Day,
                    defCalendar.DinnerTimeStart.Value.DateTime2Time().Hours,
                    defCalendar.DinnerTimeStart.Value.DateTime2Time().Minutes,
                    defCalendar.DinnerTimeStart.Value.DateTime2Time().Seconds,
                    defCalendar.DinnerTimeStart.Value.DateTime2Time().Milliseconds,
                    DateTimeKind.Unspecified);
                DateTime utc_end = utc_start + defCalendar.ExclusionTimeSpanInMinutes.Value.ToTimeSpan();
                defaultExclusions.Add(new CalendarInterval(CalendarDayType.Holiday,ConvertTimeToUtc(utc_start, calendarTimeZone),ConvertTimeToUtc(utc_end, calendarTimeZone)));
            }
            //
            return CreateInterval(dayType, date, defCalendar.TimeStart.DateTime2Time(), defCalendar.TimeSpanInMinutes.ToTimeSpan(), calendarTimeZone);
        }

        private async Task<TimeZone> GetDefaultTimeZoneIfNullAsync(TimeZone timeZone, CancellationToken cancellationToken)
        {
            if (timeZone == null)
            {
                var defCalendar = await _calendarWorkScheduleDefaultsRepository.FirstOrDefaultAsync(cancellationToken);
                if (defCalendar != null)
                    timeZone = await _finderTimeZone.FindAsync(defCalendar.TimeZoneID, cancellationToken);

            }
            return timeZone;
        }

        internal CalendarInterval CreateInterval(CalendarDayType dayType, DateTime date, TimeSpan timeStart, TimeSpan timeSpan, TimeZone calendarTimeZone)
        {
            if (!dayType.IsWorkDay())
            {
                timeStart = TimeSpan.Zero;
                timeSpan = TimeSpan.Zero;
            }
            //
            DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds, timeStart.Milliseconds, DateTimeKind.Unspecified);
            DateTime utcDateTime = ConvertTimeToUtc(dateTime, calendarTimeZone);
            //
            return new CalendarInterval(dayType, utcDateTime, utcDateTime.Add(timeSpan));
        }

        private DateTime GetNextDay(DateTime dt)
        {
            return (new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind)).AddDays(1);
        }
        private static bool CheckIsDst(DateTime startTime, DateTime time, DateTime endTime)
        {
            int year = startTime.Year;
            int num2 = endTime.Year;
            //
            if (year != num2)
                endTime = endTime.AddYears(year - num2);
            //
            int num3 = time.Year;
            if (year != num3)
                time = time.AddYears(year - num3);
            //
            if (startTime > endTime)
                return ((time < endTime) || (time >= startTime));
            //
            return ((time >= startTime) && (time < endTime));
        }
        private static bool GetIsAmbiguousTime(DateTime time, TimeZoneAdjustmentRule rule, CalendarDaylightTime daylightTime)
        {
            bool flag = false;
            if ((rule != null) && (rule.DaylightDelta() != TimeSpan.Zero))
            {
                DateTime end;
                DateTime time3;
                DateTime time4;
                DateTime time5;
                if (rule.DaylightDelta() > TimeSpan.Zero)
                {
                    end = daylightTime.End;
                    time3 = daylightTime.End - rule.DaylightDelta();
                }
                else
                {
                    end = daylightTime.Start;
                    time3 = daylightTime.Start + rule.DaylightDelta();
                }
                flag = (time >= time3) && (time < end);
                if (flag || (end.Year == time3.Year))
                    return flag;
                //
                try
                {
                    time4 = end.AddYears(1);
                    time5 = time3.AddYears(1);
                    flag = (time >= time5) && (time < time4);
                }
                catch (ArgumentOutOfRangeException)
                { }
                //
                if (flag)
                    return flag;
                try
                {
                    time4 = end.AddYears(-1);
                    time5 = time3.AddYears(-1);
                    flag = (time >= time5) && (time < time4);
                }
                catch (ArgumentOutOfRangeException)
                { }
            }
            return flag;
        }
        private DateTimeKind GetCorrespondingKind(TimeZone timeZone)
        {
            if (timeZone.ID == UTC.ID)
                return DateTimeKind.Utc;
            if (timeZone == Local)
                return DateTimeKind.Local;
            //
            return DateTimeKind.Unspecified;
        }
        private DateTime ConvertUtcToTimeZone(long ticks, TimeZone destinationTimeZone, out bool isAmbiguousLocalDst)
        {
            DateTime maxValue;
            if (ticks > DateTime.MaxValue.Ticks)
                maxValue = DateTime.MaxValue;
            else if (ticks < DateTime.MinValue.Ticks)
                maxValue = DateTime.MinValue;
            else
                maxValue = new DateTime(ticks);
            //
            TimeSpan span = GetUtcOffsetFromUtc(maxValue, destinationTimeZone, out var isDaylightSAving, out isAmbiguousLocalDst);
            ticks += span.Ticks;
            if (ticks > DateTime.MaxValue.Ticks)
                return DateTime.MaxValue;
            //
            if (ticks < DateTime.MinValue.Ticks)
                return DateTime.MinValue;
            //
            return new DateTime(ticks);
        }
        private TimeSpan GetUtcOffsetFromUtc(DateTime time, TimeZone zone, out bool isDaylightSavings, out bool isAmbiguousLocalDst)
        {
            int year;
            TimeZoneAdjustmentRule adjustmentRuleForTime;
            isDaylightSavings = false;
            isAmbiguousLocalDst = false;
            TimeSpan baseUtcOffset = zone.BaseUtcOffset();
            if (time > DateTime.MaxValue.Date)
            {
                adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(DateTime.MaxValue);
                year = DateTime.MaxValue.Date.Year;
            }
            else if (time < DateTime.MinValue)
            {
                adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(DateTime.MinValue);
                year = 1;
            }
            else
            {
                DateTime dateTime = time + baseUtcOffset;
                year = time.Year;
                adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(dateTime);
            }
            if (adjustmentRuleForTime != null)
            {
                isDaylightSavings = GetIsDaylightSavingsFromUtc(time, year, zone.BaseUtcOffset(), adjustmentRuleForTime, out isAmbiguousLocalDst);
                baseUtcOffset += isDaylightSavings ? adjustmentRuleForTime.DaylightDelta() : TimeSpan.Zero;
            }
            return baseUtcOffset;
        }

        private bool GetIsDaylightSavingsFromUtc(DateTime time, int Year, TimeSpan utc, TimeZoneAdjustmentRule rule, out bool isAmbiguousLocalDst)
        {
            DateTime time5;
            DateTime time6;
            isAmbiguousLocalDst = false;
            if (rule == null)
                return false;
            //
            TimeSpan span = utc;
            CalendarDaylightTime daylightTime = GetDaylightTime(Year, rule);
            DateTime startTime = daylightTime.Start - span;
            DateTime endTime = (daylightTime.End - span) - rule.DaylightDelta();
            if (daylightTime.Delta.Ticks > 0L)
            {
                time5 = endTime - daylightTime.Delta;
                time6 = endTime;
            }
            else
            {
                time5 = startTime;
                time6 = startTime - daylightTime.Delta;
            }
            bool flag = CheckIsDst(startTime, time, endTime);
            if (flag)
            {
                isAmbiguousLocalDst = (time >= time5) && (time < time6);
                if (isAmbiguousLocalDst || (time5.Year == time6.Year))
                    return flag;
                //
                try
                {
                    time5.AddYears(1);
                    time6.AddYears(1);
                    isAmbiguousLocalDst = (time >= time5) && (time < time6);
                }
                catch (ArgumentOutOfRangeException)
                { }
                //
                if (isAmbiguousLocalDst)
                    return flag;
                //
                try
                {
                    time5.AddYears(-1);
                    time6.AddYears(-1);
                    isAmbiguousLocalDst = (time >= time5) && (time < time6);
                }
                catch (ArgumentOutOfRangeException)
                { }
            }
            return flag;
        }


        private List<CalendarInterval> ToIntervalsWithoutIntersect(List<CalendarInterval> intervalList, bool influenceNotWorkInterval)
        {
            if (intervalList.Count <= 1)
                return intervalList;
            //
            List<CalendarInterval> retval = new List<CalendarInterval>();
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
                    List<CalendarInterval> intersectList = intervalList.FindAll(x =>
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
                        if (interval.CalendarDayType.IsWorkDay())
                            existsWork = true;
                        else
                            existsNotWork = true;
                    //
                    bool isWorkRegion = existsWork;
                    if (influenceNotWorkInterval)
                        isWorkRegion &= !existsNotWork;
                    retval.Add(new CalendarInterval(isWorkRegion ? CalendarDayType.Work : CalendarDayType.Weekend, r1.Value, r2));
                }
                //
                r1 = r2;
            }
            //
            return retval;
        }
        private List<CalendarInterval> GetIntervalsByDay(List<CalendarInterval> intervalList, DateTime utcDate)
        {
            if (intervalList.Count == 0)
                return intervalList;
            //
            DateTime utcStartDay = utcDate.Date;
            DateTime utcEndDay = utcStartDay.AddDays(1);
            //
            List<CalendarInterval> retval = new List<CalendarInterval>();
            foreach (var x in intervalList)
            {
                if (x.UtcStartDate <= utcStartDay && x.UtcEndDate >= utcStartDay && x.UtcEndDate <= utcEndDay)
                    retval.Add(new CalendarInterval(x.CalendarDayType, utcStartDay, x.UtcEndDate));
                else if (x.UtcStartDate >= utcStartDay && x.UtcEndDate <= utcEndDay)
                    retval.Add(x);
                else if (x.UtcStartDate >= utcStartDay && x.UtcStartDate <= utcEndDay && x.UtcEndDate >= utcEndDay)
                    retval.Add(new CalendarInterval(x.CalendarDayType, x.UtcStartDate, utcEndDay));
                else if (x.UtcStartDate <= utcStartDay && x.UtcEndDate >= utcEndDay)
                    retval.Add(new CalendarInterval(x.CalendarDayType, utcStartDay, utcEndDay));
                //остальные интервалы не пересекаются
            }
            //
            return retval;
        }

        private DateTime GetEntrypointDate(DateTime nowDateTime, CalendarSettingsDetails supportSettingsCalendar)
        {
            if (nowDateTime.TimeOfDay <= supportSettingsCalendar.TimeStart.TimeOfDay)
            {
                return new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, 
                    supportSettingsCalendar.TimeStart.Hour, supportSettingsCalendar.TimeStart.Minute, 0);
            }
            if (nowDateTime.TimeOfDay >= supportSettingsCalendar.TimeStart.TimeOfDay
                && nowDateTime.TimeOfDay <= supportSettingsCalendar.DinnerTimeStart.Value.TimeOfDay)
            {
                return DateTime.UtcNow;
            }
            if (nowDateTime.TimeOfDay >= supportSettingsCalendar.DinnerTimeStart.Value.TimeOfDay
                && nowDateTime.TimeOfDay <= supportSettingsCalendar.DinnerTimeEnd.Value.TimeOfDay)
            {
                return new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, 
                    supportSettingsCalendar.DinnerTimeEnd.Value.Hour, supportSettingsCalendar.DinnerTimeEnd.Value.Minute, 0);
            }
            if (nowDateTime.TimeOfDay >= supportSettingsCalendar.DinnerTimeEnd.Value.TimeOfDay
                && nowDateTime.TimeOfDay <= supportSettingsCalendar.TimeStartEnd.TimeOfDay)
            {
                return DateTime.UtcNow;
            }
            if (nowDateTime.TimeOfDay >= supportSettingsCalendar.TimeStartEnd.TimeOfDay)
            {
                var dateNow = nowDateTime.AddDays(1);
                return new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 
                    supportSettingsCalendar.TimeStart.Hour, supportSettingsCalendar.TimeStart.Minute, 0);
            }

            return DateTime.UtcNow;
        }

    }
