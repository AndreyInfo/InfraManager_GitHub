using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using static InfraManager.BLL.SlaUtils.SlaHelper;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

public class MassIncidentAgreementSetter : ISetAgreement<MassIncident>,
    ISelfRegisteredService<ISetAgreement<MassIncident>>
{
    private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _defaultCalendarRepository;

    private readonly IReadonlyRepository<OperationalLevelAgreement> _operationalLevelAgreements;
    private CalendarWorkScheduleDefault _defaultCalendar;
    private readonly ISettingsBLL _settings;
    private readonly IConvertSettingValue<long> _longConverter;
    private readonly IConvertSettingValue<byte> _byteConverter;

    public MassIncidentAgreementSetter(
        IReadonlyRepository<OperationalLevelAgreement> operationalLevelAgreements,
        IReadonlyRepository<CalendarWorkScheduleDefault> defaultCalendarRepository,
        ISettingsBLL settings,
        IConvertSettingValue<long> longConverter,
        IConvertSettingValue<byte> byteConverter)
    {
        _operationalLevelAgreements = operationalLevelAgreements;
        _defaultCalendarRepository = defaultCalendarRepository;
        _settings = settings;
        _longConverter = longConverter;
        _byteConverter = byteConverter;
    }

    public async Task SetAsync(MassIncident massIncident, CancellationToken cancellationToken = default, DateTime? countCompleteDateFrom = null, Guid? olaID = null)
    {
        long? promiseTicks = null;
        CalendarWorkSchedule calendarWorkSchedule = null;
        TimeZone calendarTimeZone = null;

        _defaultCalendar = await _defaultCalendarRepository
            .With(x => x.CalendarHoliday)
            .With(x => x.CalendarWeekend)
            .With(x => x.TimeZone)
            .ThenWithMany(x => x.TimeZoneAdjustmentRules)
            .FirstOrDefaultAsync();

        var listOfAvailableOla = await _operationalLevelAgreements
            .WithMany(x => x.Rules)
            .With(x => x.TimeZone)
            .ThenWithMany(x => x.TimeZoneAdjustmentRules)
            .With(x => x.CalendarWorkSchedule)
            .ThenWithMany(x => x.WorkScheduleItems)
            .ThenWithMany(x => x.WorkScheduleItemExclusions)
            .With(x => x.CalendarWorkSchedule)
            .ThenWithMany(x => x.WorkScheduleShifts)
            .ThenWithMany(x => x.WorkScheduleShiftExclusions)
            .ToArrayAsync(OperationalLevelAgreement.IsActive && OperationalLevelAgreement.IsRelatedToService.Build(massIncident.ServiceID)
                , cancellationToken);

        var olaRules = new Dictionary<OperationalLevelAgreement, RuleValue>();

        listOfAvailableOla.ForEach(ola =>
        {
            var rulesList = new List<RuleValue>();
            ola.Rules.Select(Parse)
                .Where(r => !r.Services.Any() || r.Services.Contains(massIncident.ServiceID))
                .ForEach(rule => rulesList.Add(rule));
            if(rulesList.Any())
                olaRules.Add(ola, rulesList.MinBy(r => r.PromiseTime));
        });

        var selectedOla = olaRules.Keys.FirstOrDefault(
            key => olaRules[key] == olaRules.Values
                .Where(r => r != null)
                .MinBy(rule => rule.PromiseTime));
        if (selectedOla != null)
            promiseTicks = olaRules[selectedOla].PromiseTime;

        selectedOla ??= listOfAvailableOla.FirstOrDefault();
        massIncident.OperationalLevelAgreementID = selectedOla?.ID;
        
        
        calendarWorkSchedule = selectedOla?.CalendarWorkSchedule;
        calendarTimeZone = selectedOla?.TimeZone;

        promiseTicks ??=
            _longConverter.Convert(await _settings.GetValueAsync(SystemSettings.MassIncidentsDateDelta,
                cancellationToken));

        var calculationMode = (CountPlannedDateFrom)_byteConverter.Convert(
            await _settings.GetValueAsync(SystemSettings.MassIncidentsDateCalculationMode, cancellationToken));
        var startDate = countCompleteDateFrom ?? (calculationMode == CountPlannedDateFrom.SinceDateCreation
            ? massIncident.UtcCreatedAt
            : (calculationMode == CountPlannedDateFrom.SinceDateRegistration
                ? (massIncident.UtcRegisteredAt ?? massIncident.UtcDateModified)
                : DateTime.UtcNow));

        massIncident.UtcCloseUntil = GetUtcFinishDateByCalendar(startDate, new TimeSpan(promiseTicks.Value),
            calendarWorkSchedule, calendarTimeZone, _defaultCalendar);
    }
}