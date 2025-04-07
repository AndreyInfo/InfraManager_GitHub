using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ObjectIcons
{
    internal class ObjectIconBLL : IObjectIconBLL, ISelfRegisteredService<IObjectIconBLL>
    {
        private readonly IRepository<ObjectIcon> _repository;
        private readonly IServiceMapper<ObjectClass, IValidatePermissions> _permissionsValidator;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ObjectIconBLL(
            IRepository<ObjectIcon> repository,
            IServiceMapper<ObjectClass, IValidatePermissions> permissionsValidator,
            IMapper mapper,
            IMemoryCache cache,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork,
            ILogger<ObjectIconBLL> logger)
        {
            _repository = repository;
            _permissionsValidator = permissionsValidator;
            _mapper = mapper;
            _cache = cache;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ObjectIconDetails> GetAsync(InframanagerObject objectID, CancellationToken cancellationToken = default)
        {
            return await _cache.GetOrCreateAsync(
                CacheKey(objectID),
                async entry =>
                {
                    var objectIcon = await FindOrDefaultAsync(objectID, cancellationToken);
                    return objectIcon == null
                        ? _mapper.Map<ObjectIconDetails>(objectID)
                        : _mapper.Map<ObjectIconDetails>(objectIcon);
                });
        }

        // TODO: Добавить проверку, что связанный объект существует, в DAL реализовать удаление иконки при удалении связанного объекта
        public async Task<ObjectIconDetails> SetAsync(InframanagerObject objectID, ObjectIconData data, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"User (ID = {_currentUser.UserId}) is attempting to set icon for {objectID}");
            
            if (!_permissionsValidator.HasKey(objectID.ClassId))
            {
                throw new ObjectNotFoundException($"Object class {objectID.ClassId} is not supported.");
            }

            if (!await _permissionsValidator
                .Map(objectID.ClassId)
                .UserHasPermissionAsync(objectID.Id, ObjectAction.Update, cancellationToken))
            {
                throw new AccessDeniedException($"Set icon for {objectID}");
            }
            _logger.LogTrace($"Permissions to set icon for {objectID} were granted to user (ID = {_currentUser.UserId})");

            var objectIcon = await FindOrDefaultAsync(objectID, cancellationToken);

            if (objectIcon == null)
            {
                _logger.LogTrace($"Icon for object ({objectID}) was not found. Creating a new entity.");
                objectIcon = data.Content != null 
                    ? new ObjectIcon(objectID, data.Content) 
                    : new ObjectIcon(objectID, data.Name);
                _repository.Insert(objectIcon);
            }
            _mapper.Map(data, objectIcon);

            await _unitOfWork.SaveAsync(cancellationToken);
            _cache.Remove(CacheKey(objectID));

            _logger.LogInformation($"New icon is set for {objectID} by user (ID = {_currentUser.UserId})");
            return _mapper.Map<ObjectIconDetails>(objectIcon);
        }

        private async Task<ObjectIcon> FindOrDefaultAsync(InframanagerObject objectID, CancellationToken cancellationToken = default)
        {
            return await _repository.FirstOrDefaultAsync(ObjectIcon.WhereObjectEquals(objectID), cancellationToken);
        }

        internal static string CacheKey(InframanagerObject objectID) => $"icon_{objectID.Id}_{objectID.ClassId}";
    }
}
