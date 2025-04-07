using AutoMapper;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;

namespace InfraManager.BLL.Catalog
{
    /*
     * В данном классе не должна быть логика валидации входящих моделей.
     * Все, что решает этот класс - шаблонная бизнес логика для всех каталогов с которыми приходится работать.
     */
    internal class BasicCatalogBLL<TCatalog, TCatalogDetails, TKey, TTable> : IBasicCatalogBLL<TCatalog, TCatalogDetails, TKey,TTable>
        where TCatalog : Catalog<TKey>
        where TKey : struct
        where TCatalogDetails : class
    {
        
        private readonly IRepository<TCatalog> _catalogRepository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly IGuidePaggingFacade<TCatalog, TTable> _pagging;
        private readonly IValidatePermissions<TCatalog> _validatePermissions;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<TCatalog> _logger;
        private readonly IFinder<TCatalog> _finder;
        private readonly IPagingQueryCreator _paggingQuery;

        private Guid _currentUserID => _currentUser.UserId;
        
        public BasicCatalogBLL(
            IRepository<TCatalog> catalogRepository,
            IMapper mapper,
            IUnitOfWork saveChangesCommand,
            IGuidePaggingFacade<TCatalog, TTable> pagging,
            IValidatePermissions<TCatalog> validatePermissions,
            ICurrentUser currentUser, 
            ILogger<TCatalog> logger,
            IFinder<TCatalog> finder,
            IPagingQueryCreator paggingQuery)
        {
            _saveChangesCommand = saveChangesCommand;
            _catalogRepository = catalogRepository;
            _mapper = mapper;
            _pagging = pagging;
            _validatePermissions = validatePermissions;
            _currentUser = currentUser;
            _logger = logger;
            _finder = finder;
            _paggingQuery = paggingQuery;
        }

        [Obsolete("Use RemoveAsync instead")]
        public async Task<string[]> DeleteAsync(IEnumerable<DeleteModel<TKey>> deleteModels,
            CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Delete, cancellationToken);

            if (deleteModels == null)
                throw new ArgumentNullException(nameof(deleteModels));

            var invalidCatalogNames = new List<string>();
            var catalogsToDelete = _mapper.Map<TCatalog[]>(deleteModels);

            foreach (var catalog in catalogsToDelete)
            {
                try
                {
                    _catalogRepository.Attach(catalog);
                    _catalogRepository.Delete(catalog);
                    await _saveChangesCommand.SaveAsync(cancellationToken);
                }
                catch
                {
                    invalidCatalogNames.Add(catalog.Name);
                }
            }

            return invalidCatalogNames.ToArray();
        }

        public async Task RemoveAsync(TKey id, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Delete, cancellationToken);

            var entity = await _finder.FindOrRaiseErrorAsync(id, cancellationToken);
            
            _catalogRepository.Delete(entity);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            
            _logger.LogTrace($"UserID = {_currentUserID} deleted {nameof(TCatalog)}");
        }
        
        public async Task<TKey> InsertAsync(TCatalogDetails catalogDetails, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Insert, cancellationToken);

            _logger.LogTrace($"UserID = {_currentUser.UserId} has permission to insert {typeof(TCatalog).Name}");
            
            var entity = MapOrThrowIfNull(catalogDetails);
            await ThrowIfExistsSameNameAsync(entity.ID, entity.Name, cancellationToken);

            if (typeof(TKey) == typeof(int))
            {
                int maxID = Convert.ToInt32(_catalogRepository.Query().Max(x => x.ID)) + 1;
                entity.ID = (TKey)(object)maxID;
            }

            if (typeof(TKey) == typeof(Guid)) //TODO пофиксить проблему, когда он не генерируется сам на стороне еф кора
            {
                entity.ID = (TKey)(object)Guid.NewGuid();
            }

            _catalogRepository.Insert(entity);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            
            _logger.LogTrace($"UserID = {_currentUser.UserId} inserted new {typeof(TCatalog).Name}");
            
            return entity.ID;
        }

        public async Task<TKey> UpdateAsync(TKey id, TCatalogDetails catalogDetails,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"UserID = {_currentUser.UserId} started updating {typeof(TCatalog).Name}");
           
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Update, cancellationToken);

            var entity = MapOrThrowIfNull(catalogDetails);
            await ThrowIfExistsSameNameAsync(id, entity.Name, cancellationToken);

            var foundEntity = await _catalogRepository.FirstOrDefaultAsync(x => x.ID.Equals(entity.ID), cancellationToken);
            ThrowIfNull(foundEntity);

            _mapper.Map(catalogDetails, foundEntity);
            
            await _saveChangesCommand.SaveAsync(cancellationToken);

            _logger.LogTrace($"UserID = {_currentUser.UserId} updated {typeof(TCatalog).Name}");
            
            return foundEntity.ID;
        }
        
        public async Task<TCatalogDetails[]> GetByFilterAsync(BaseFilter filter, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"UserID = {_currentUserID} requested a list of {nameof(TCatalog)} with filtration");

            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.ViewDetailsArray, cancellationToken);

            var items = await _pagging.GetPaggingAsync(filter,
                _catalogRepository.Query(),
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken);

            return _mapper.Map<TCatalogDetails[]>(items);
        }

        public async Task<TCatalogDetails[]> GetListAsync(string searchString = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"UserID = {_currentUserID} requested a list of {nameof(TCatalog)}");
            
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.ViewDetailsArray, cancellationToken);

            var query = _catalogRepository.Query();
            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));

            var pagging = _paggingQuery.Create(query.OrderBy(c=> c.Name));

            var entities = await pagging.PageAsync(0, 0, cancellationToken);

            return _mapper.Map<TCatalogDetails[]>(entities);
        }
        
        public async Task<TCatalogDetails> GetByIDAsync(TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"UserID = {_currentUserID} requested {nameof(TCatalog)} with id = {id}");

            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.ViewDetails, cancellationToken);
            var result = await _catalogRepository.FirstOrDefaultAsync(c => c.ID.Equals(id), cancellationToken);
            ThrowIfNull(result);

            return _mapper.Map<TCatalogDetails>(result);
        }
        
        
        [Obsolete("Use AddAsync or UpdateAsync instead")]
        public async Task<TKey> SaveOrUpdateAsync(TCatalogDetails catalogDetails,
            CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<TCatalog>(catalogDetails);

            if (EqualityComparer<TKey>.Default.Equals(entity.ID, default))
            {
                return await InsertAsync(catalogDetails, cancellationToken);
            }
            else
            {
                return await UpdateAsync(entity.ID, catalogDetails, cancellationToken);
            }
        }
        
        public void SetIncludeItems<TProperty>(Expression<Func<TCatalog, TProperty>> expression) where TProperty : class
        {
            _catalogRepository.With(expression);
        }

        private TCatalog MapOrThrowIfNull(TCatalogDetails catalogDto)
        {
            var entity = _mapper.Map<TCatalog>(catalogDto);
            if (entity == null)
                throw new ArgumentNullException(nameof(catalogDto));

            return entity;
        }

        private async Task ThrowIfExistsSameNameAsync(TKey id, string name, CancellationToken cancellationToken)
        {
            var isExistingEntity =
              await _catalogRepository.AnyAsync(x => x.Name.Equals(name) && !x.ID.Equals(id),
                  cancellationToken);

            if (isExistingEntity)
                throw new InvalidObjectException("Значение справочника с данным именем уже существует"); //TODO LCOALE
        }

        private void ThrowIfNull(TCatalog catalog)
        {
            if (catalog is null)
                throw new ObjectNotFoundException($"{nameof(TCatalog)} not found");
        }
    }
}
