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
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.Messages;

internal class ProblemTemplateBuilder :
    EntityTemplateBuilderBase<Problem, ProblemTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<Problem, ProblemTemplate>>
{
    private readonly IReadonlyRepository<Problem> _repository;
    private readonly INotificationHTMLMessageBuilder _htmlMessageBuilder;
    private readonly IReadonlyRepository<ManyToMany<MassIncident, Problem>> _massIncidentsWithProblems;

    public ProblemTemplateBuilder(IReadonlyRepository<Problem> repository,
        IMapper mapper,
        ISettingsBLL settingsBll,
        IConvertSettingValue<string> valueConverter,
        IReadonlyRepository<DocumentReference> documentReferences,
        INotificationHTMLMessageBuilder htmlMessageBuilder,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IServiceDeskDateTimeConverter serviceDeskTimeConverter,
        IReadonlyRepository<Negotiation> negotiations,
        IReadonlyRepository<ManyToMany<MassIncident, Problem>> massIncidentsWithProblems)
        : base(
            mapper,
            settingsBll,
            valueConverter,
            userFinder,
            serviceDeskTimeConverter,
            documentReferences,
            negotiations)
    {
        _repository = repository;
        _htmlMessageBuilder = htmlMessageBuilder;
        _massIncidentsWithProblems = massIncidentsWithProblems;
    }

    protected override async Task<Problem> GetEntityOrRaiseErrorAsync(Guid entityID, CancellationToken cancellationToken)
    {
        return await _repository
                   .With(x => x.Type).ThenWith(x => x.Parent)
                   .With(x => x.ProblemCause)
                   .With(x => x.Urgency)
                   .With(x => x.Influence)
                   .With(x => x.Priority)
                   .With(x => x.Owner).ThenWith(x => x.Position)
                   .With(x => x.Owner).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Owner).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Executor).ThenWith(x => x.Position)
                   .With(x => x.Executor).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Executor).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .With(x => x.Initiator).ThenWith(x => x.Position)
                   .With(x => x.Initiator).ThenWith(x => x.Workplace).ThenWith(x => x.Room).ThenWith(x => x.Floor).ThenWith(x => x.Building)
                   .With(x => x.Initiator).ThenWith(x => x.Subdivision).ThenWith(x => x.Organization)
                   .WithMany(x => x.Negotiations)
                   .WithMany(x => x.CallReferences)
                   .WithMany(x => x.WorkOrderReferences)
                   .WithMany(x => x.Dependencies)
                   .WithMany(x => x.Notes)
                   .With(x => x.Service).ThenWith(x => x.Category)
                   .DisableTrackingForQuery()
                   .FirstOrDefaultAsync(x => x.IMObjID == entityID, cancellationToken)
               ?? throw new ObjectNotFoundException($"Объект уведомления {entityID} (класс {ObjectClass.Problem}) не найден");
    }

    protected override async Task AfterAutoMapAsync(Problem entity, ProblemTemplate template, CancellationToken cancellationToken)
    {
        template.WebServerAddress = await GetWebServerAddressAsync(cancellationToken);

        template.DocumentCountString = (await CountDocumentAsync(entity.IMObjID, cancellationToken)).ToString();
        if (entity.Notes.Any())
        {
            var orderedNotes = entity.Notes.OrderBy(x => x.UtcDate).ToArray();

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

        template.OwnerSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.OwnerID, cancellationToken);
        template.InitiatorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.InitiatorID, cancellationToken);
        template.ExecutorSubdivisionName = await GetUserSubdivisionFullNameOrDefaultAsync(entity.ExecutorID, cancellationToken);

        template.DateDetectedString = await ConvertDateTimeAsync(entity.UtcDateDetected, cancellationToken);
        template.DatePromisedString = await ConvertDateTimeAsync(entity.UtcDatePromised, cancellationToken);
        template.DateSolvedString = await ConvertDateTimeAsync(entity.UtcDateSolved, cancellationToken);
        template.DateClosedString = await ConvertDateTimeAsync(entity.UtcDateClosed, cancellationToken);
        template.DateModifiedString = await ConvertDateTimeAsync(entity.UtcDateModified, cancellationToken);

        template.NegotiationCountString = (await CountNegotiationsAsync(entity.IMObjID, ObjectClass.Problem, cancellationToken)).ToString();
        template.MassIncidentsCountString =
            (await _massIncidentsWithProblems.CountAsync(m => m.Reference == entity, cancellationToken)).ToString();
    }
}