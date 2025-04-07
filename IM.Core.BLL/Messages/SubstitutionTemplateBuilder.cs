using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Notification.Templates;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.BLL.Messages;

public class SubstitutionTemplateBuilder : IBuildEntityTemplate<DeputyUser, SubstitutionTemplate>,
    ISelfRegisteredService<IBuildEntityTemplate<DeputyUser, SubstitutionTemplate>>
{
    private readonly IFinder<DeputyUser> _finder;
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _defaultCalendarRepository;
    private readonly IReadonlyRepository<User> _users;

    public SubstitutionTemplateBuilder(IFinder<DeputyUser> finder, IMapper mapper,
        IReadonlyRepository<CalendarWorkScheduleDefault> defaultCalendarRepository, IReadonlyRepository<User> users)
    {
        _finder = finder;
        _mapper = mapper;
        _defaultCalendarRepository = defaultCalendarRepository;
        _users = users;
    }
    public async Task<SubstitutionTemplate> BuildAsync(Guid id, CancellationToken cancellationToken = default,
        Guid? userID = null)
    {
        var entity = await _finder
            .With(d => d.Child)
            .With(d => d.Parent)
            .FindAsync(id, cancellationToken);
        var template = _mapper.Map<SubstitutionTemplate>(entity);

        var user = await _users.FirstOrDefaultAsync(u => u.IMObjID == userID, cancellationToken);
        var timeZone = user?.TimeZone ??
                       user?.Workplace?.Room?.Floor?.Building?.TimeZone ??
                       (await _defaultCalendarRepository
                           .With(c => c.TimeZone)
                           .FirstOrDefaultAsync(cancellationToken))?.TimeZone;

        if (timeZone != null) AdjustTimeAccordingTimeZone(entity, timeZone, template);

        return template;
    }

    private static void AdjustTimeAccordingTimeZone(DeputyUser entity, TimeZone timeZone,
        SubstitutionTemplate template)
    {
        DateTimeOffset deputyFromWithTimeOffset =
            entity.UtcDataDeputyWith.AddMinutes(timeZone.BaseUtcOffsetInMinutes);
        DateTimeOffset deputyToWithTimeOffset =
            entity.UtcDataDeputyBy.AddMinutes(timeZone.BaseUtcOffsetInMinutes);

        template.DateDeputyWithString = deputyFromWithTimeOffset.ToString("dd.MM.yyyy HH:mm:ss");
        template.DateDeputyByString = deputyToWithTimeOffset.ToString("dd.MM.yyyy HH:mm:ss");
    }
}