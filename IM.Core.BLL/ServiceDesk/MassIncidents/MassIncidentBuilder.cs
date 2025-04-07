using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentBuilder :
        IBuildObject<MassIncident, NewMassIncidentData>,
        ISelfRegisteredService<IBuildObject<MassIncident, NewMassIncidentData>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly IReadonlyRepository<Priority> _priorityRepository;
        private readonly IMapper _mapper;
        private readonly IFinder<Service> _services;
        private readonly IFinder<Call> _calls;
        private readonly IFindEntityByGlobalIdentifier<Group> _groupFinder;
        private readonly ISetAgreement<MassIncident> _slaSetter;

        public MassIncidentBuilder(
            ICurrentUser currentUser,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IReadonlyRepository<Priority> priorityRepository,
            IMapper mapper,
            IFinder<Service> services,
            IFinder<Call> calls,
            IFindEntityByGlobalIdentifier<Group> groupFinder,
            ISetAgreement<MassIncident> slaSetter)
        {
            _currentUser = currentUser;
            _userFinder = userFinder;
            _priorityRepository = priorityRepository;
            _mapper = mapper;
            _services = services;
            _calls = calls;
            _groupFinder = groupFinder;
            _slaSetter = slaSetter;
        }

        public async Task<MassIncident> BuildAsync(NewMassIncidentData data, CancellationToken cancellationToken = default)
        {
            var massIncident = new MassIncident(data.InformationChannelID, data.TypeID);
            _mapper.Map(data, massIncident);

            if (!data.PriorityID.HasValue)
            {
                var defaultPriority = await _priorityRepository.FirstOrDefaultAsync(p => p.Default, cancellationToken)
                    ?? throw new InvalidObjectException("Default priority is not defined");
                massIncident.PriorityID = defaultPriority.ID;
            }

            var nullUser = await _userFinder.FindAsync(User.NullUserGloablIdentifier, cancellationToken);
            massIncident.CreatedBy = await GetUserAsync(data.CreatedByUserID, nullUser, "Initiator", cancellationToken);
            massIncident.OwnedBy = await GetUserAsync(data.OwnedByUserID, nullUser, "Owner", cancellationToken);
            massIncident.ExecutedByUser = await GetUserAsync(data.ExecutedByUserID, nullUser, "Executor", cancellationToken);
            massIncident.ExecutedByGroup = await _groupFinder
                .WithMany(g => g.QueueUsers)
                .FindAsync(data.ExecutedByGroupID ?? Group.NullGroupID, cancellationToken);

            foreach (var serviceID in (data.AffectedServiceIDs ?? Array.Empty<Guid>()).Distinct())
            {
                var affectedService = await _services.FindAsync(serviceID, cancellationToken);

                if (affectedService != null // Проигнорируем те сервисы, которые были удалены пока пользователь "думал"
                    && affectedService.ID != massIncident.ServiceID) // Проигнорируем те сервисы, которые совпадают с основным
                {
                    massIncident.AffectedServices.Add(
                        new ManyToMany<MassIncident, Service>(affectedService));
                }
            }

            foreach (var callID in data.Calls ?? Array.Empty<Guid>())
            {
                var call = await _calls.FindAsync(callID, cancellationToken);

                if (call != null)
                {
                    massIncident.Calls.Add(new ManyToMany<MassIncident, Call>(call));
                }
            }

            await _slaSetter.SetAsync(massIncident, cancellationToken);

            return massIncident;
        }

        private async Task<User> GetUserAsync(Guid? userID, User defaultUser, string role, CancellationToken cancellationToken = default)
        {
            return userID.HasValue
                ? (await _userFinder.FindAsync(userID.Value, cancellationToken) ?? throw new InvalidObjectException($"{role} not found."))
                : defaultUser;
        }

        public Task<IEnumerable<MassIncident>> BuildManyAsync(IEnumerable<NewMassIncidentData> dataItems, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
