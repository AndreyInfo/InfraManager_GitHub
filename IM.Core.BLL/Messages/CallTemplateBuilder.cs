using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Notification;
using InfraManager.BLL.Notification.Templates;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.Location;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.Messages;

internal class CallTemplateBuilder :
    EntityTemplateBuilderBase<Call, CallTemplate>,
    IBuildEntityTemplate<Call, CallTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<Call, CallTemplate>>
{
    private const int LastFiveMessagesCount = 5;

    private readonly IReadonlyRepository<Call> _repository;
    private readonly IReadonlyRepository<CallDependency> _dependencies;
    private readonly IReadonlyRepository<CallReference<ChangeRequest>> _references;
    private readonly IReadonlyRepository<Note<Call>> _notes;
    private readonly INotificationHTMLMessageBuilder _htmlMessageBuilder;
    private readonly IFindEntityByGlobalIdentifier<Room> _roomFinder;
    private readonly IFindEntityByGlobalIdentifier<Workplace> _workplaceFinder;

    public CallTemplateBuilder(
        IReadonlyRepository<Call> repository,
        IMapper mapper,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter,
        IReadonlyRepository<CallDependency> dependencies,
        IReadonlyRepository<CallReference<ChangeRequest>> references,
        IReadonlyRepository<Negotiation> negotiations,
        IReadonlyRepository<Note<Call>> notes,
        INotificationHTMLMessageBuilder htmlMessageBuilder,
        IServiceDeskDateTimeConverter serviceDeskTimeConverter,
        IFindEntityByGlobalIdentifier<Room> roomFinder,
        IFindEntityByGlobalIdentifier<Workplace> workplaceFinder,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IReadonlyRepository<DocumentReference> documentReferences)
        : base(
            mapper,
            settingsBll,
            valueConverter,
            userFinder,
            serviceDeskTimeConverter,
            documentReferences,
            negotiations)
    {
        _dependencies = dependencies;
        _references = references;
        _notes = notes;
        _repository = repository;
        _htmlMessageBuilder = htmlMessageBuilder;
        _roomFinder = roomFinder;
        _workplaceFinder = workplaceFinder;
    }

    protected override async Task<Call> GetEntityOrRaiseErrorAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _repository
                   .With(x => x.Owner).ThenWith(x => x.Position)
                   .With(x => x.Owner).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Owner).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Executor).ThenWith(x => x.Position)
                   .With(x => x.Executor).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Executor).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Client).ThenWith(x => x.Position)
                   .With(x => x.Client).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Client).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Accomplisher).ThenWith(x => x.Position)
                   .With(x => x.Accomplisher).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Accomplisher).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Initiator).ThenWith(x => x.Position)
                   .With(x => x.Initiator).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Initiator).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Influence)
                   .With(x => x.Urgency)
                   .With(x => x.Queue)
                   .With(x => x.Priority)
                   .With(x => x.RequestForServiceResult)
                   .With(x => x.IncidentResult)
                   .With(x => x.CallService).ThenWith(x => x.ServiceItem).ThenWith(x => x.Service).ThenWith(x => x.Category)
                   .With(x => x.CallService).ThenWith(x => x.ServiceAttendance).ThenWith(x => x.Service).ThenWith(x => x.Category)
                   .With(x => x.CallType).ThenWith(x => x.Parent)
                   .With(x => x.Aggregate)
                   .DisableTrackingForQuery()
                   .SingleOrDefaultAsync(x => x.IMObjID == entityID, cancellationToken)
               ?? throw new ObjectNotFoundException($"Объект уведомления {entityID} (класс {ObjectClass.Call}) не найден");
    }

    protected override async Task AfterAutoMapAsync(Call entity, CallTemplate template, CancellationToken cancellationToken)
    {
        template.WebServerAddress = await GetWebServerAddressAsync(cancellationToken);
        
        if (entity.ServicePlaceID.HasValue && entity.ServicePlaceClassID.HasValue)
        {
            template.ServicePlaceFullName = await GetServicePlaceNameAsync(entity.ServicePlaceID.Value, entity.ServicePlaceClassID.Value, cancellationToken);
        }

        if (entity.CallService.ServiceItem is not null)
        {
            template.ServiceName = $"{entity.CallService.ServiceItem.Service.Category.Name} " +
                                   $"\\ {entity.CallService.ServiceItem.Service.Name}";

            template.ServiceItemFullName = $"{entity.CallService.ServiceItem.Service.Category.Name} " +
                                           $"\\ {entity.CallService.ServiceItem.Service.Name} " +
                                           $"\\ {entity.CallService.ServiceItem.Name}";
        }

        if (entity.CallService.ServiceAttendance is not null)
        {
            template.ServiceName = $"{entity.CallService.ServiceAttendance.Service.Category.Name} " +
                                   $"\\ {entity.CallService.ServiceAttendance.Service.Name}";

            template.ServiceAttendanceFullName = $"{entity.CallService.ServiceAttendance.Service.Category.Name} " +
                                                 $"\\ {entity.CallService.ServiceAttendance.Service.Name} " +
                                                 $"\\ {entity.CallService.ServiceAttendance.Name}";
        }

        template.DependencyObjectCountString = (await _dependencies.CountAsync(x => x.OwnerObjectID == entity.IMObjID, cancellationToken)).ToString();
        template.RFCCountString = (await _references.CountAsync(x => x.CallID == entity.IMObjID, cancellationToken)).ToString();
        template.NegotiationCountString = (await CountNegotiationsAsync(entity.IMObjID, ObjectClass.Call, cancellationToken)).ToString();

        var allMessages = (await _notes.DisableTrackingForQuery()
                .ToArrayAsync(x => x.ParentObjectID == entity.IMObjID && x.Type == SDNoteType.Message, cancellationToken)
            ).ToArray();

        if (allMessages.Any())
        {
            var finalMessage = new[] { allMessages.MaxBy(x => x.UtcDate), };
            var lastMessages = allMessages.OrderByDescending(x => x.UtcDate).Take(LastFiveMessagesCount).ToArray();

            template.MessageString = await _htmlMessageBuilder.BuildMessageTextAsync(allMessages, cancellationToken);
            template.HTMLMessageString =
                await _htmlMessageBuilder.BuildMessageHTMLTextAsync(allMessages, cancellationToken);
            template.FinalMessageString =
                await _htmlMessageBuilder.BuildMessageTextAsync(finalMessage, cancellationToken);
            template.HTMLFinalMessageString =
                await _htmlMessageBuilder.BuildMessageHTMLTextAsync(finalMessage, cancellationToken);
            template.LastMessageString =
                await _htmlMessageBuilder.BuildMessageTextAsync(lastMessages, cancellationToken);
            template.HTMLLastMessageString =
                await _htmlMessageBuilder.BuildMessageHTMLTextAsync(lastMessages, cancellationToken);
        }

        template.DateCreatedString = await ConvertDateTimeAsync(entity.UtcDateCreated, cancellationToken);
        template.DateModifiedString = await ConvertDateTimeAsync(entity.UtcDateModified, cancellationToken);
        template.DatePromisedString = await ConvertDateTimeAsync(entity.UtcDatePromised, cancellationToken);
        template.DateRegisteredString = await ConvertDateTimeAsync(entity.UtcDateRegistered, cancellationToken);
        template.DateOpenedString = await ConvertDateTimeAsync(entity.UtcDateOpened, cancellationToken);
        template.DateAccomplishedString = await ConvertDateTimeAsync(entity.UtcDateAccomplished, cancellationToken);
        template.DateClosedString = await ConvertDateTimeAsync(entity.UtcDateClosed, cancellationToken);

        template.OwnerSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.OwnerID, cancellationToken);
        template.AccomplisherSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.AccomplisherID, cancellationToken);
        template.InitiatorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.InitiatorID, cancellationToken);
        template.ExecutorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.ExecutorID, cancellationToken);
        template.ClientSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.ClientID, cancellationToken);
    }

    private async Task<string> GetServicePlaceNameAsync(Guid servicePlaceID, ObjectClass classID, CancellationToken cancellationToken)
    {
        if (classID == ObjectClass.Room)
        {
            var room = await _roomFinder
                .With(x => x.Floor)
                .ThenWith(x => x.Building)
                .ThenWith(x => x.Organization)
                .FindAsync(servicePlaceID, cancellationToken);

            if (room is not null)
            {
                return $"{room.Floor?.Building?.Organization?.Name} " +
                       $"\\ {room.Floor?.Building?.Name} " +
                       $"\\ {room.Floor?.Name} " +
                       $"\\ {room.Name}";
            }
        }

        if (classID == ObjectClass.Workplace)
        {
            var workplace = await _workplaceFinder
                .With(x => x.Room)
                .ThenWith(x => x.Floor)
                .ThenWith(x => x.Building)
                .ThenWith(x => x.Organization)
                .FindAsync(servicePlaceID, cancellationToken);

            if (workplace is not null)
            {
                return $"{workplace.Room?.Floor?.Building?.Organization?.Name} " +
                       $"\\ {workplace.Room?.Floor?.Building?.Name} " +
                       $"\\ {workplace.Room?.Floor?.Name} " +
                       $"\\ {workplace.Room?.Name} " +
                       $"\\ {workplace.Name}";
            }
        }

        return string.Empty;
    }
}
