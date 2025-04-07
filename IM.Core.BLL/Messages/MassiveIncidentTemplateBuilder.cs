using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Location.TimeZones;
using InfraManager.BLL.Notification.Templates;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Settings.Calendar;
using InfraManager.Core;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Linq;
using InfraManager.BLL.Notification;
using InfraManager.BLL.ServiceDesk;

namespace InfraManager.BLL.Messages;

internal class MassiveIncidentTemplateBuilder :
    IBuildEntityTemplate<MassIncident, MassIncidentTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<MassIncident, MassIncidentTemplate>>
{
    private readonly IRepository<MassIncident> _repository;
    private readonly IMapper _mapper;
    private readonly ISettingsBLL _settingsBll;
    private readonly IConvertSettingValue<string> _valueConverter;
    private readonly IReadonlyRepository<DocumentReference> _documentReferences;
    private readonly IReadonlyRepository<Negotiation> _negotiations;
    private readonly IReadonlyRepository<WorkOrder> _workOrders;
    private readonly ISupportSettingsCalendarBLL _supportSettingsCalendar;
    private readonly ITimeZoneBLL _timeZoneBLL;
    private readonly IRepository<Note<MassIncident>> _notes;
    private readonly INotificationHTMLMessageBuilder _htmlMessageBuilder;

    public MassiveIncidentTemplateBuilder(IRepository<MassIncident> repository,
        IMapper mapper,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter, 
        IReadonlyRepository<DocumentReference> documentReferences,
        IReadonlyRepository<Negotiation> negotiations,
        IReadonlyRepository<WorkOrder> workOrders,
        ISupportSettingsCalendarBLL supportSettingsCalendar,
        ITimeZoneBLL timeZoneBLL,
        INotificationHTMLMessageBuilder htmlMessageBuilder,
        IRepository<Note<MassIncident>> notes)
    {
        _repository = repository;
        _mapper = mapper;
        _settingsBll = settingsBll;
        _valueConverter = valueConverter;
        _documentReferences = documentReferences;
        _negotiations = negotiations;
        _workOrders = workOrders;
        _supportSettingsCalendar = supportSettingsCalendar;
        _timeZoneBLL = timeZoneBLL;
        _notes = notes;
        _htmlMessageBuilder = htmlMessageBuilder;
        _notes = notes;
    }
    
    public async Task<MassIncidentTemplate> BuildAsync(Guid id, CancellationToken cancellationToken = default,
        Guid? userID = null)
    {
        var entity = await _repository
                         .With(x => x.Description)
                         .With(x => x.Solution)
                         .With(x => x.OwnedBy).ThenWith(x => x.Position)
                         .With(x => x.OwnedBy).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor)
                            .ThenWith(x => x.Building)
                         .With(x => x.OwnedBy).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                         .With(x => x.CreatedBy).ThenWith(x => x.Position)
                         .With(x => x.CreatedBy).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor)
                            .ThenWith(x => x.Building)
                         .With(x => x.CreatedBy).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                         .With(x => x.ExecutedByUser).ThenWith(x => x.Position)
                         .With(x => x.ExecutedByUser).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor)
                            .ThenWith(x => x.Building)
                         .With(x => x.ExecutedByUser).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                         .With(x => x.Criticality)
                         .With(x => x.Type)
                         .With(x => x.Priority)
                         .WithMany(x => x.ChangeRequests)
                         .WithMany(x => x.Calls)
                         .WithMany(x => x.Problems)
                         .WithMany(x => x.AffectedServices)
                         .With(x => x.Service)
                         .With(x => x.ExecutedByGroup)
                         .WithMany(x => x.ChangeRequests)
                         .With(x => x.TechnicalFailureCategory)
                         .With(x => x.Cause)
                         .With(x => x.MassIncidentCause)
                         .With(x => x.MassIncidentInformationChannel)
                         .DisableTrackingForQuery()
                         .FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken)
                     ?? throw new ObjectNotFoundException(
                         $"Объект уведомления {id} (класс {ObjectClass.MassIncident}) не найден");
        
        var supportSettings = await _supportSettingsCalendar.GetAsync(cancellationToken);
        var timeZone = await _timeZoneBLL.GetAsync(supportSettings.TimeZoneID, cancellationToken);

        var template = _mapper.Map<MassIncidentTemplate>(entity);


        template.DateAccomplishedString = ConvertToStandardTimeZone(entity.UtcDateAccomplished, timeZone);
        template.DateClosedString = ConvertToStandardTimeZone(entity.UtcDateClosed, timeZone);
        template.DatePromisedString = ConvertToStandardTimeZone(entity.UtcCloseUntil, timeZone);
        template.DateOpenedString = ConvertToStandardTimeZone(entity.UtcOpenedAt, timeZone);
        template.DateRegisteredString = ConvertToStandardTimeZone(entity.UtcRegisteredAt, timeZone);
        template.DateCreatedString = ConvertToStandardTimeZone(entity.UtcCreatedAt, timeZone);

        var setting = await _settingsBll.GetValueAsync(SystemSettings.WebServerAddress, cancellationToken);
        template.WebServerAddress = _valueConverter.Convert(setting);

        template.DocumentCountString =
            (await _documentReferences.CountAsync(x => x.ObjectID == entity.IMObjID, cancellationToken)).ToString();

        template.RequestForChangeCountString = entity.ChangeRequests.Count.ToString();
        template.CallCountString = entity.Calls.Count.ToString();
        template.ProblemCountString = entity.Problems.Count.ToString();
        template.AdditionalServicesCountString = entity.AffectedServices.Count.ToString();
        template.NegotiationCountString =
            (await _negotiations.CountAsync(
                x => x.ObjectID == entity.IMObjID && x.ObjectClassID == ObjectClass.MassIncident, cancellationToken))
            .ToString();
        template.ProblemCountString = entity.Problems.Count.ToString();
        template.WorkOrderCountString = (
            await _workOrders.With(wo => wo.WorkOrderReference)
                .CountAsync(wo => wo.WorkOrderReference.ObjectID == entity.IMObjID, cancellationToken))
            .ToString();

        var notes = await _notes.ToArrayAsync(x=>x.ParentObjectID == entity.IMObjID, cancellationToken);
        if (notes.Any())
        {
            var orderedNotes = notes.OrderBy(x => x.UtcDate).ToArray();

            template.LastNote =
                await _htmlMessageBuilder.BuildMessageTextAsync(new[] { orderedNotes.Last() }, cancellationToken);
            template.HTMLLastNote =
                await _htmlMessageBuilder.BuildMessageHTMLTextAsync(new[] { orderedNotes.Last() }, cancellationToken);

            template.Last5Notes =
                await _htmlMessageBuilder.BuildMessageTextAsync(orderedNotes.TakeLast(5), cancellationToken);
            template.HTMLLast5Notes =
                await _htmlMessageBuilder.BuildMessageHTMLTextAsync(orderedNotes.TakeLast(5), cancellationToken);
                
            template.AllNotes = await _htmlMessageBuilder.BuildMessageTextAsync(orderedNotes, cancellationToken);
            template.HTMLAllNotes = await _htmlMessageBuilder.BuildMessageHTMLTextAsync(orderedNotes, cancellationToken);
        }
        
        return template;
    }

    private string ConvertToStandardTimeZone(DateTime? date, TimeZoneDetails defaultTimeZone)
    {
        if (date == null)
        {
            return string.Empty;
        }

        var convertedDate = date.Value.AddMinutes(defaultTimeZone.BaseUtcOffsetInMinutes);

        return DateTimeExtensions.Format(convertedDate, Global.DateTimeFormat);
    }
}