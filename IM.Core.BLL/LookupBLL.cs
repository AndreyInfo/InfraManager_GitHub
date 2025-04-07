using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL // Мнение: Лучше нагенерить таких классов. чем продолжать в том же духе.
{
    [Obsolete("Use StandardBLL")]
    internal class LookupBLL<TListModel, TDetailsModel, TModel, TEntity, TKey> : 
        ILookupBLL<TListModel, TDetailsModel, TModel, TKey> 
        where TKey : struct
        where TEntity : Lookup
        where TListModel : LookupListItem<TKey>
        where TDetailsModel : LookupDetails<TKey>
        where TModel : LookupData
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IFinder<TEntity> _finder;
        private readonly IUnitOfWork _saveChagesCommand;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IValidatePermissions<TEntity> _validatePermissions;
        private readonly ILogger<LookupBLL<TListModel, TDetailsModel, TModel, TEntity, TKey>> _logger;
        private readonly ICurrentUser _currentUser;

        private string CacheKey => $"lookup_{typeof(TEntity)}";

        public LookupBLL(
            IRepository<TEntity> repository, 
            IFinder<TEntity> finder, 
            IUnitOfWork saveChagesCommand, 
            IMapper mapper, 
            IMemoryCache cache,
            IValidatePermissions<TEntity> validatePermissions,
            ILogger<LookupBLL<TListModel, TDetailsModel, TModel, TEntity, TKey>> logger,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _finder = finder;
            _saveChagesCommand = saveChagesCommand;
            _mapper = mapper;
            _cache = cache;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<TDetailsModel> AddAsync(TModel model, CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, token);

            var entity = _mapper.Map<TEntity>(model);
            _repository.Insert(entity);

            return await SaveAndGetDetails(entity, token);
        }

        public async Task DeleteAsync(TKey id, CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, token);

            var entity = await FindOrRaiseError(id, token);
            _repository.Delete(entity);
            await _saveChagesCommand.SaveAsync(token);
            
            ClearCache();
        }

        public async Task<TDetailsModel> FindAsync(TKey id, CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, token);
            var entity = await FindOrRaiseError(id, token);
            return _mapper.Map<TDetailsModel>(entity);
        }

        public async Task<TListModel[]> ListAsync(CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, token);

            var data = await _cache.GetOrCreateAsync(
                CacheKey,
                entry => _repository.ToArrayAsync(token));

            return data
                .Select(entity => _mapper.Map<TListModel>(entity))
                .ToArray();
        }

        public async Task<TDetailsModel> UpdateAsync(TKey id, TModel model, CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, token);

            var entity = await FindOrRaiseError(id, token);
            _mapper.Map(model, entity);

            return await SaveAndGetDetails(entity, token);
        }

        private async Task<TDetailsModel> SaveAndGetDetails(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _saveChagesCommand.SaveAsync(cancellationToken);
            ClearCache();
            return _mapper.Map<TDetailsModel>(entity);
        }

        private async Task<TEntity> FindOrRaiseError(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await _finder.FindAsync(id, cancellationToken);
            return entity 
                ?? throw new ObjectNotFoundException(
                    $"{typeof(TModel).Name} (ID = {id}) is either deleted or does not exist");
        }

        private void ClearCache()
        {
            _cache.Remove(CacheKey);
        }
    }
}
