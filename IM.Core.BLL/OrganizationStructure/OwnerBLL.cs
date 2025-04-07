using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class OwnerBLL : IOwnerBLL, ISelfRegisteredService<IOwnerBLL>
    {
        private static readonly string CacheKey = "owners";

        private readonly IReadonlyRepository<Owner> _repository;
        private readonly IInsertEntityBLL<Owner, OwnerData> _insertBll;
        private readonly IModifyEntityBLL<Guid, Owner, OwnerData, OwnerDetails> _modifyBLL;
        private readonly IRemoveEntityBLL<Guid, Owner> _deleteBll;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<OwnerBLL> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidatePermissions<Owner> _permissionValidator;

        public OwnerBLL(
            IReadonlyRepository<Owner> repository,
            IInsertEntityBLL<Owner, OwnerData> insertBll,
            IModifyEntityBLL<Guid, Owner, OwnerData, OwnerDetails> modifyBLL,
            IRemoveEntityBLL<Guid, Owner> deleteBll,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUser currentUser,
             ILogger<OwnerBLL> logger,
            IMemoryCache cache,
            IValidatePermissions<Owner> permissionValidator)
        {
            _repository = repository;
            _insertBll = insertBll;
            _modifyBLL = modifyBLL;
            _deleteBll = deleteBll;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
            _cache = cache;
            _permissionValidator = permissionValidator;
        }

        public async Task<OwnerDetails[]> AllAsync(int? take, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) is requesting all owners");
            await _permissionValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var allOwners = await FromCacheOrRepository(cancellationToken);
            IEnumerable<OwnerDetails> query = allOwners.Values.OrderBy(x => x.IMObjID);
            query = take.HasValue ? query.Take(take.Value) : query;

            return query.ToArray();
        }

        private Task<Dictionary<Guid, OwnerDetails>> FromCacheOrRepository(CancellationToken cancellationToken)
        {
            return _cache.GetOrCreateAsync(
                CacheKey,
                async entry =>
                {
                    var owners = await FromRepository(cancellationToken);
                    return owners.ToDictionary(x => x.IMObjID, x => _mapper.Map<OwnerDetails>(x));
                });
        }

        private Task<Owner[]> FromRepository(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Owners dictionary is loading from repository");
            return _repository.ToArrayAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) is trying to delete owner (ID = {id})");

            if (id == Owner.DefaultOwnerID)
            {
                throw new ObjectReadonlyException(new InframanagerObject(id, ObjectClass.Owner));
            }

            await _deleteBll.RemoveAsync(id, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully deleted owner (ID = {id}).");
            _cache.Remove(CacheKey);
        }

        public async Task<OwnerDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) is requesting owner details (ID = {id})");

            await _permissionValidator.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

            var owners = await FromCacheOrRepository(cancellationToken);

            if (!owners.ContainsKey(id))
            {
                throw new ObjectNotFoundException<Guid>(id, ObjectClass.Owner);
            }

            return owners[id];
        }

        public async Task<OwnerDetails> ModifyAsync(Guid id, OwnerData data, CancellationToken cancellationToken)
        {
            var owner = await _modifyBLL.ModifyAsync(id, data, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            _cache.Remove(CacheKey);

            _logger.LogInformation($"User (ID = {_currentUser.UserId}) successfully modified owner (ID = {id})");
            return _mapper.Map<OwnerDetails>(owner);
        }
    }
}
