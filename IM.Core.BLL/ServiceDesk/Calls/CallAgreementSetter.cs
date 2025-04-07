using InfraManager.BLL.Settings;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using static InfraManager.BLL.SlaUtils.SlaHelper;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

// TODO: копипаста кучи легаси кода, надо переписывать 
namespace InfraManager.BLL.ServiceDesk.Calls
{
    public enum RuleValueType: byte
    {
        Service = 0,
        ServiceAttendance = 1,
        ServiceItem = 2,
        DayOfWeek = 3,
        RegistrationTime = 4,
        Priority = 5,
        ClientPosition = 6,
        ClientOrgStructure = 11,
        CallType = 9,
        Urgency = 12,
        PromiseTime = 7,
        Price = 8,
        ParameterTemplate = 10
    }
    public enum FilterOperation
    {
        Equal = 1,
        NotEqual = 2,
        Like = 4,
        LT = 8,
        LTE = 16,
        GTE = 32,
        GT = 64
    }
    public enum TimeZoneInfoOptions : byte
    {
        None = 1,
        NoThrowOnInvalidTime = 2
    }

    internal class CallAgreementSetter : ISetAgreement<Call>,
        ISelfRegisteredService<ISetAgreement<Call>>
    {
        private readonly IReadonlyRepository<ServiceLevelAgreement> _serviceLevelAgreements;
        private readonly IReadonlyRepository<Subdivision> _subdivisions;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<long> _longConverter;
        private readonly IConvertSettingValue<byte> _byteConverter;
        private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _defaultCalendarRepository;
        private readonly IReadonlyRepository<CalendarWorkSchedule> _calendarWorkScheduleRepository;
        private readonly IReadonlyRepository<TimeZone> _timeZoneRepository;
        private readonly IMemoryCache _timeZoneCache;

        private CalendarWorkScheduleDefault _defaultCalendar;

        public CallAgreementSetter(
            IReadonlyRepository<ServiceLevelAgreement> serviceLevelAgreements, 
            IReadonlyRepository<Subdivision> subdivisions, 
            IFindEntityByGlobalIdentifier<User> userFinder, 
            ISettingsBLL settings, 
            IConvertSettingValue<long> longConverter,
            IConvertSettingValue<byte> byteConverter, 
            IReadonlyRepository<CalendarWorkScheduleDefault> defaultCalendarRepository,
            IReadonlyRepository<CalendarWorkSchedule> calendarWorkScheduleRepository,
            IReadonlyRepository<TimeZone> timeZoneRepository,
            IMemoryCache timeZoneCache)
        {
            _serviceLevelAgreements = serviceLevelAgreements;
            _subdivisions = subdivisions;
            _userFinder = userFinder;
            _settings = settings;
            _longConverter = longConverter;
            _byteConverter = byteConverter;
            _defaultCalendarRepository = defaultCalendarRepository;
            _timeZoneRepository = timeZoneRepository;
            _timeZoneCache = timeZoneCache;
            _calendarWorkScheduleRepository = calendarWorkScheduleRepository;
        }

        public async Task SetAsync(Call call, CancellationToken cancellationToken = default, DateTime? countCompleteDateFrom = null, Guid? slaID = null)
        {
            long? promiseTicks = null;
            Guid? calendarWorkScheduleID = null;
            string calendarTimeZoneID = null;
            ServiceLevelAgreement selectedSla;

            _defaultCalendar = await _defaultCalendarRepository
                    .With(x => x.CalendarHoliday)
                    .With(x => x.CalendarWeekend)
                    .With(x => x.TimeZone)
                        .ThenWithMany(x => x.TimeZoneAdjustmentRules)
                    .FirstOrDefaultAsync();

            if (call.CallService.Service != null)
            {
                if (!call.CallService.Service.IsAvailable || !call.CallService.PortfolioService.IsAvailable)
                {
                    return; // TODO: Видимо эта проверка должна быть на другом уровне, а не здесь
                }

                var client = await _userFinder
                    .With(x => x.Subdivision)
                    .FindAsync(call.ClientID, cancellationToken);
                var clientOrganizationID = client.Subdivision?.OrganizationID;
                await _subdivisions.With(x => x.ParentSubdivision)
                    .ToArrayAsync(x => x.OrganizationID == clientOrganizationID, cancellationToken);

                var clientOrganizationItemIDs =
                    SubdivisionIDs(client.Subdivision, new List<Guid>())
                    .Concat(clientOrganizationID.HasValue ? new[] { clientOrganizationID.Value } : Array.Empty<Guid>())
                    .ToArray();

                selectedSla = await GetSLA(call, slaID, clientOrganizationItemIDs, client, cancellationToken);

                if (selectedSla != null)
                {
                    var reference = GetReferenceForCall(selectedSla, call);
                    calendarWorkScheduleID = reference?.CalendarWorkScheduleID ?? selectedSla.CalendarWorkScheduleID;
                    calendarTimeZoneID = reference?.TimeZoneID ?? selectedSla.TimeZoneID;
                    var userRegistrationDate = await GetDateAsync(call.UtcDateRegistered ?? call.UtcDateModified, calendarTimeZoneID);

                    var rule = selectedSla.Rules
                        .OrderBy(x => x.Sequence)
                        .Select(Parse)
                        .FirstOrDefault(r => r.MatchCondition(call, client, clientOrganizationItemIDs, userRegistrationDate));

                    if (!string.IsNullOrWhiteSpace(rule?.Price))
                    {
                        var realSeparator = 1.23m.ToString("0.00")[1];
                        if (!rule.Price.Contains(realSeparator.ToString()))
                            rule.Price = realSeparator == '.' ? rule.Price.Replace(',', realSeparator) : rule.Price.Replace('.', realSeparator);
                        decimal tmp;
                        if (decimal.TryParse(rule.Price, out tmp))
                            call.Price = tmp;
                    }

                    promiseTicks = rule?.PromiseTime;
                }
            }
            else
            {
                selectedSla =
                    await _serviceLevelAgreements.FirstOrDefaultAsync(sla => sla.ID == slaID, cancellationToken);
            }

            call.SLAName = selectedSla?.Name;
            promiseTicks ??= _longConverter.Convert(await _settings.GetValueAsync(SystemSettings.CallPromiseDateDelta, cancellationToken));

            var calculationMode = (CountPlannedDateFrom)_byteConverter.Convert(await _settings.GetValueAsync(SystemSettings.CallPromiseDateCalculationMode, cancellationToken));
            var startDate = countCompleteDateFrom ?? (calculationMode == CountPlannedDateFrom.SinceDateCreation
                ? call.UtcDateCreated
                : (calculationMode == CountPlannedDateFrom.SinceDateRegistration 
                    ? (call.UtcDateRegistered ?? call.UtcDateModified) 
                    : DateTime.UtcNow));

            var calendarWorkSchedule = calendarWorkScheduleID.HasValue
                ? await _calendarWorkScheduleRepository.FirstOrDefaultAsync(x => x.ID == calendarWorkScheduleID.Value, cancellationToken)
                : null;
            var calendarTimeZone = !string.IsNullOrEmpty(calendarTimeZoneID)
                ? await _timeZoneRepository.FirstOrDefaultAsync(x => x.ID == calendarTimeZoneID)
                : null;

            call.UtcDatePromised = GetUtcFinishDateByCalendar(startDate, new TimeSpan(promiseTicks.Value), calendarWorkSchedule, calendarTimeZone, _defaultCalendar);
            call.CalendarWorkScheduleID = calendarWorkSchedule?.ID;
            call.TimeZoneID = calendarTimeZone?.ID;
        }

        private async Task<ServiceLevelAgreement> GetSLA(Call call, Guid? slaID, Guid[] clientOrganizationItemIDs,
            User client, CancellationToken cancellationToken)
        {
            ServiceLevelAgreement selectedSla;
            var slaRepo = _serviceLevelAgreements
                .DisableTrackingForQuery()
                .WithMany(x => x.OrganizationItemGroups)
                .WithMany(x => x.Rules)
                .WithMany(x => x.References);

            if (slaID != null)
            {
                selectedSla = await slaRepo.FirstOrDefaultAsync(sla => sla.ID == slaID, cancellationToken);
            }
            else
            {
                var listOfAvailableSla = await slaRepo.ToArrayAsync(
                        (ServiceLevelAgreement.IsRelatedToOrganizationGroups.Build(clientOrganizationItemIDs) &&
                         ServiceLevelAgreement.IsActive),
                        cancellationToken);

                selectedSla = listOfAvailableSla
                    .Where(sla => GetReferenceForCall(sla, call) != null)
                    .Select(async sla => new
                    {
                        Sla = sla,
                        UserRegistrationDate = await GetDateAsync(call.UtcDateRegistered ?? call.UtcDateModified, sla.TimeZoneID),
                    })
                    .Select(x => x.Result)
                    .Where(
                        sla =>
                        {
                            return sla.Sla.Rules.Any(r =>
                                Parse(r)?.MatchCondition(call, client, clientOrganizationItemIDs, sla.UserRegistrationDate) ??
                                false);
                        })
                    .OrderBy(
                        sla => sla.Sla.OrganizationItemGroups
                            .Where(x => clientOrganizationItemIDs.Contains(x.ItemID))
                            .Min(x => clientOrganizationItemIDs.IndexOf(x.ItemID)))
                    .ThenBy(item => item.Sla.UtcStartDate)
                    .FirstOrDefault()?.Sla;
            }

            return selectedSla;
        }

        private IEnumerable<Guid> SubdivisionIDs(Subdivision subdivision, List<Guid> antiLoopCache)
        {
            if (subdivision == null || antiLoopCache.Contains(subdivision.ID))
            {
                return Array.Empty<Guid>();
            }
            antiLoopCache.Add(subdivision.ID);

            return new[] { subdivision.ID }
                .Concat(SubdivisionIDs(subdivision.ParentSubdivision, antiLoopCache));
        }

        private static SLAReference GetReferenceForCall(ServiceLevelAgreement sla, Call call) =>
            sla.References.FirstOrDefault(x => x.ClassID == ObjectClass.ServiceItem && x.ObjectID == call.CallService.ServiceItemID)
                ?? sla.References.FirstOrDefault(x => x.ClassID == ObjectClass.ServiceAttendance && x.ObjectID == call.CallService.ServiceAttendanceID)
                    ?? sla.References.FirstOrDefault(x => x.ClassID == ObjectClass.Service && x.ObjectID == call.CallService.Service.ID);


        private async Task<DateTime> GetDateAsync(DateTime utcDate, string timeZoneID)
        {
            var timeZone = await GetTimeZoneAsync(timeZoneID);

            var tmp = new DateTime(utcDate.Ticks, DateTimeKind.Unspecified);
            var retval = (timeZone ?? _defaultCalendar.TimeZone).ConvertTimeFromUtc(tmp);

            return retval;
        }

        private async Task<TimeZone> GetTimeZoneAsync(string timeZoneID)
        {
            if (string.IsNullOrEmpty(timeZoneID))
            {
                return null;
            }

            return await _timeZoneCache.GetOrCreateAsync(
                timeZoneID,
                _ => _timeZoneRepository
                    .With(tz => tz.TimeZoneAdjustmentRules)
                    .FirstOrDefaultAsync(tz => tz.ID == timeZoneID));
        }
    }

    internal static class TimeZoneExtensions
    {
        public static readonly DateTime MIN_UTC_TIME = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime __maxDateOnly = new DateTime(0x270f, 12, 0x1f);
        private static DateTime __minDateOnly = new DateTime(1, 1, 2);

        public static TimeZone Utc => CreateFromInfo(TimeZoneInfo.Utc);

        public static TimeZone Local => CreateFromInfo(TimeZoneInfo.Local);

        private static TimeZone CreateFromInfo(TimeZoneInfo info)
        {
            var timezone = new TimeZone
            {
                ID = info.Id,
                Name = info.DisplayName,
                BaseUtcOffsetInMinutes = (short)info.BaseUtcOffset.TotalMinutes,
                SupportsDaylightSavingTime = info.SupportsDaylightSavingTime,
            };

            foreach (var rule in info.GetAdjustmentRules())
            {
                timezone.TimeZoneAdjustmentRules.Add(
                    new TimeZoneAdjustmentRule
                    {
                        TimeZoneID = timezone.ID,
                        DateStart = rule.DateStart,
                        DateEnd = rule.DateEnd,
                        DaylightDeltaInMinutes = (short)rule.DaylightDelta.TotalMinutes,
                        TransitionStart_IsFixedDateRule = rule.DaylightTransitionStart.IsFixedDateRule,
                        TransitionStart_Month = (byte)rule.DaylightTransitionStart.Month,
                        TransitionStart_Day = rule.DaylightTransitionStart.IsFixedDateRule ? (byte?)rule.DaylightTransitionStart.Day : null,
                        TransitionStart_TimeOfDay = MIN_UTC_TIME.Add(rule.DaylightTransitionStart.TimeOfDay.TimeOfDay),
                        TransitionStart_Week = rule.DaylightTransitionStart.IsFixedDateRule ? null : (byte?)rule.DaylightTransitionStart.Week,
                        TransitionStart_DayOfWeek = rule.DaylightTransitionStart.IsFixedDateRule ? null : (byte?)rule.DaylightTransitionStart.DayOfWeek,
                        TransitionEnd_IsFixedDateRule = rule.DaylightTransitionEnd.IsFixedDateRule,
                        TransitionEnd_Month = (byte)rule.DaylightTransitionEnd.Month,
                        TransitionEnd_Day = rule.DaylightTransitionEnd.IsFixedDateRule ? (byte?)rule.DaylightTransitionEnd.Day : null,
                        TransitionEnd_TimeOfDay = MIN_UTC_TIME.Add(rule.DaylightTransitionEnd.TimeOfDay.TimeOfDay),
                        TransitionEnd_Week = rule.DaylightTransitionEnd.IsFixedDateRule ? null : (byte?)rule.DaylightTransitionEnd.Week,
                        TransitionEnd_DayOfWeek = rule.DaylightTransitionEnd.IsFixedDateRule ? null : (byte?)rule.DaylightTransitionEnd.DayOfWeek
                    });
            }

            return timezone;
        }

        public static DateTime ConvertTimeFromUtc(this TimeZone destinationTimeZone, DateTime dateTime)
        {
            return ConvertTime(dateTime, Utc, destinationTimeZone, TimeZoneInfoOptions.None);
        }

        private static DateTimeKind GetCorrespondingKind(TimeZone timeZone)
        {
            if (timeZone.ID == TimeZoneInfo.Utc.Id)
                return DateTimeKind.Utc;
            if (timeZone.ID == TimeZoneInfo.Local.Id)
                return DateTimeKind.Local;
            //
            return DateTimeKind.Unspecified;
        }

        private static DateTime ConvertTime(DateTime dateTime, TimeZone sourceTimeZone, TimeZone destinationTimeZone, TimeZoneInfoOptions flags)
        {
            DateTimeKind correspondingKind = GetCorrespondingKind(sourceTimeZone);
            //if ((((flags & TimeZoneInfoOptions.NoThrowOnInvalidTime) == 0) && (dateTime.Kind != DateTimeKind.Unspecified)) && (dateTime.Kind != correspondingKind))
            //    throw new ArgumentException("sourceTimeZone");
            //
            var adjustmentRuleForTime = sourceTimeZone.GetAdjustmentRuleForTime(dateTime);
            TimeSpan baseUtcOffset = TimeSpan.FromMinutes(sourceTimeZone.BaseUtcOffsetInMinutes);
            if (adjustmentRuleForTime != null)
            {
                bool flag = false;
                var daylightTime = GetDaylightTime(dateTime.Year, adjustmentRuleForTime);
                //мы не следим особо за DateTimeKind, вот и не будем ругаться
                //if (((flags & TimeZoneInfoOptions.NoThrowOnInvalidTime) == 0) && GetIsInvalidTime(dateTime, adjustmentRuleForTime, daylightTime))
                //    throw new ArgumentException("dateTime");
                //
                flag = GetIsDaylightSavings(dateTime, adjustmentRuleForTime, daylightTime, flags);
                baseUtcOffset += flag ? TimeSpan.FromMinutes(adjustmentRuleForTime.DaylightDeltaInMinutes) : TimeSpan.Zero;
            }
            DateTimeKind kind = GetCorrespondingKind(destinationTimeZone);
            if (((dateTime.Kind != DateTimeKind.Unspecified) && (correspondingKind != DateTimeKind.Unspecified)) && (correspondingKind == kind))
                return dateTime;
            //
            long ticks = dateTime.Ticks - baseUtcOffset.Ticks;
            bool isAmbiguousLocalDst = false;
            DateTime time2 = ConvertUtcToTimeZone(ticks, destinationTimeZone, out isAmbiguousLocalDst);
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

        private static TimeZoneAdjustmentRule GetAdjustmentRuleForTime(this TimeZone timezone, DateTime dateTime)
        {
            var date = dateTime.Date;

            return timezone.TimeZoneAdjustmentRules.FirstOrDefault(rule => rule.DateStart <= date && rule.DateEnd >= date);
        }

        private static CalendarDaylightTime GetDaylightTime(int year, TimeZoneAdjustmentRule rule)
        {
            DateTime start = TransitionTimeToDateTime(year, rule.TransitionStart_IsFixedDateRule, rule.TransitionStart_Month, rule.TransitionStart_Day, rule.TransitionStart_TimeOfDay.Subtract(MIN_UTC_TIME), rule.TransitionStart_Week, rule.TransitionStart_DayOfWeek);
            DateTime end = TransitionTimeToDateTime(year, rule.TransitionEnd_IsFixedDateRule, rule.TransitionEnd_Month, rule.TransitionEnd_Day, rule.TransitionEnd_TimeOfDay.Subtract(MIN_UTC_TIME), rule.TransitionEnd_Week, rule.TransitionEnd_DayOfWeek);
            //
            return new CalendarDaylightTime(start, end, TimeSpan.FromMinutes(rule.DaylightDeltaInMinutes));
        }

        private static DateTime TransitionTimeToDateTime(int year, bool isFixedDateRule, byte month, byte? day, TimeSpan timeOfDay, byte? week, byte? dayOfWeek)
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

        private static bool GetIsDaylightSavings(DateTime time, TimeZoneAdjustmentRule rule, CalendarDaylightTime daylightTime, TimeZoneInfoOptions flags)
        {
            var ruleDaylightDelta = TimeSpan.FromMinutes(rule.DaylightDeltaInMinutes);
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
                bool flag = ruleDaylightDelta > TimeSpan.Zero;
                time2 = daylightTime.Start + (flag ? ruleDaylightDelta : TimeSpan.Zero);
                end = daylightTime.End + (flag ? -ruleDaylightDelta : TimeSpan.Zero);
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
            var ruleDaylightDelta = TimeSpan.FromMinutes(rule.DaylightDeltaInMinutes);
            if ((rule != null) && (ruleDaylightDelta != TimeSpan.Zero))
            {
                DateTime end;
                DateTime time3;
                DateTime time4;
                DateTime time5;
                if (ruleDaylightDelta > TimeSpan.Zero)
                {
                    end = daylightTime.End;
                    time3 = daylightTime.End - ruleDaylightDelta;
                }
                else
                {
                    end = daylightTime.Start;
                    time3 = daylightTime.Start + ruleDaylightDelta;
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

        private static DateTime ConvertUtcToTimeZone(long ticks, TimeZone destinationTimeZone, out bool isAmbiguousLocalDst)
        {
            DateTime maxValue;
            if (ticks > DateTime.MaxValue.Ticks)
                maxValue = DateTime.MaxValue;
            else if (ticks < DateTime.MinValue.Ticks)
                maxValue = DateTime.MinValue;
            else
                maxValue = new DateTime(ticks);
            //
            TimeSpan span = GetUtcOffsetFromUtc(maxValue, destinationTimeZone, out isAmbiguousLocalDst);
            ticks += span.Ticks;
            if (ticks > DateTime.MaxValue.Ticks)
                return DateTime.MaxValue;
            //
            if (ticks < DateTime.MinValue.Ticks)
                return DateTime.MinValue;
            //
            return new DateTime(ticks);
        }

        private static TimeSpan GetUtcOffsetFromUtc(DateTime time, TimeZone zone, out bool isDaylightSavings)
        {
            bool flag;
            return GetUtcOffsetFromUtc(time, zone, out isDaylightSavings, out flag);
        }

        private static TimeSpan GetUtcOffsetFromUtc(DateTime time, TimeZone zone, out bool isDaylightSavings, out bool isAmbiguousLocalDst)
        {
            int year;
            TimeZoneAdjustmentRule adjustmentRuleForTime;
            isDaylightSavings = false;
            isAmbiguousLocalDst = false;
            TimeSpan baseUtcOffset = TimeSpan.FromMinutes(zone.BaseUtcOffsetInMinutes);
            if (time > __maxDateOnly)
            {
                adjustmentRuleForTime = zone.GetAdjustmentRuleForTime(DateTime.MaxValue);
                year = 0x270f;
            }
            else if (time < __minDateOnly)
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
                isDaylightSavings = GetIsDaylightSavingsFromUtc(time, year, TimeSpan.FromMinutes(zone.BaseUtcOffsetInMinutes), adjustmentRuleForTime, out isAmbiguousLocalDst);
                baseUtcOffset += isDaylightSavings 
                    ? TimeSpan.FromMinutes(adjustmentRuleForTime.DaylightDeltaInMinutes) 
                    : TimeSpan.Zero;
            }
            return baseUtcOffset;
        }

        private static bool GetIsDaylightSavingsFromUtc(DateTime time, int Year, TimeSpan utc, TimeZoneAdjustmentRule rule, out bool isAmbiguousLocalDst)
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
            DateTime endTime = (daylightTime.End - span) - TimeSpan.FromMinutes(rule.DaylightDeltaInMinutes);
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

        internal static DateTime ConvertTimeToUtc(DateTime dateTime, TimeZone sourceTimeZone)
        {
            if (sourceTimeZone == null)
                throw new ArgumentNullException("sourceTimeZone");
            //
            return ConvertTime(dateTime, sourceTimeZone, Utc, TimeZoneInfoOptions.None);
        }
    }

    internal class CalendarDaylightTime
    {
        public CalendarDaylightTime(DateTime start, DateTime end, TimeSpan delta)
        {
            Start = start;
            End = end;
            Delta = delta;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
        public TimeSpan Delta { get; }
    }
}
