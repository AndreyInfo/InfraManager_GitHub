using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentInformationChannelBLL : 
        IMassIncidentInformationChannelBLL,
        ISelfRegisteredService<IMassIncidentInformationChannelBLL>
    {
        private readonly IReadonlyRepository<MassIncidentInformationChannel> _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IValidatePermissions<MassIncidentInformationChannel> _permissionsValidator;

        public MassIncidentInformationChannelBLL(
            IReadonlyRepository<MassIncidentInformationChannel> repository,
            IMemoryCache cache,
            ILogger<MassIncidentInformationChannelBLL> logger,
            ICurrentUser currentUser,
            IMapper mapper,
            IValidatePermissions<MassIncidentInformationChannel> permissionsValidator)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
            _currentUser = currentUser;
            _mapper = mapper;
            _permissionsValidator = permissionsValidator;
        }

        public async Task<LookupListItem<short>[]> AllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) requested all massive incidents information channels.");

            if (!await _permissionsValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetailsArray,
                    cancellationToken))
            {
                throw new AccessDeniedException("View MassiveIncidentInformationChannel list");
            }
            _logger.LogTrace($"Permissions to view all massive incidents information channels is granted to User (ID = {_currentUser.UserId})");

            var allItems = await GetFromCacheOrRepositoryAsync(cancellationToken);

            return allItems.Values.OrderBy(x => x.ID).Select(x => _mapper.Map<LookupListItem<short>>(x)).ToArray();
        }

        public LookupListItem<short>[] All() => GetFromCacheOrRepository()
            .Values
            .Select(x => _mapper.Map<LookupListItem<short>>(x))
            .ToArray();

        public LookupListItem<short> Find(short id) => _mapper.Map<LookupListItem<short>>(GetFromCacheOrRepository()[id]);

        public async Task<LookupListItem<short>> FindAsync(short id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) requested massive incidents information channel (id = {id}).");

            if (!await _permissionsValidator.UserHasPermissionAsync(_currentUser.UserId, ObjectAction.ViewDetails,
                    cancellationToken))
            {
                throw new AccessDeniedException("View MassiveIncidentInformationChannel details");
            }
            _logger.LogTrace($"Permissions to view massive incidents information channel details is granted to User (ID = {_currentUser.UserId})");

            var allItems = await GetFromCacheOrRepositoryAsync(cancellationToken);

            if (!allItems.ContainsKey(id))
            {
                throw new ObjectNotFoundException<short>(id, "MassiveIncidentInformationChannel");
            }

            return _mapper.Map<LookupListItem<short>>(allItems[id]);
        }

        private const string CacheKey = "MassiveIncidentInformationChannel";

        private Task<Dictionary<short, MassIncidentInformationChannel>> GetFromCacheOrRepositoryAsync(CancellationToken cancellationToken = default)
        {
            return _cache.GetOrCreateAsync(
                CacheKey,
                async entry =>
                {
                    var data = await GetFromRepositoryAsync(cancellationToken);
                    return data.ToDictionary(x => x.ID);
                });
        }

        private Dictionary<short, MassIncidentInformationChannel> GetFromCacheOrRepository() => 
            _cache.GetOrCreate(CacheKey, entry => GetFromRepository().ToDictionary(x => x.ID));

        private Task<MassIncidentInformationChannel[]> GetFromRepositoryAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Massive incidents information channels dictionary is loading from repository as requested by User (ID = {_currentUser.UserId})");
            return _repository.ToArrayAsync(cancellationToken);
        }

        private MassIncidentInformationChannel[] GetFromRepository() => _repository.ToArray();
    }
}
