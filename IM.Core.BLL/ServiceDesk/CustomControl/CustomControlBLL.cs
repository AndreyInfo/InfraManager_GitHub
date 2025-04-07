using InfraManager.DAL;
using Entity = InfraManager.DAL.ServiceDesk.CustomControl;
using Inframanager.BLL.ListView;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using InfraManager.BLL.AccessManagement;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    internal class CustomControlBLL :
        ICustomControlBLL, ISelfRegisteredService<ICustomControlBLL>,
        IEditCustomControl, ISelfRegisteredService<IEditCustomControl>
    {
        private readonly IRepository<Entity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUser _currentUser;
        private readonly IObjectAccessBLL _objectAccess;
        private readonly IUserAccessBLL _userAccess;
        private readonly ILogger<CustomControlBLL> _logger;
        private readonly IListViewBLL<ObjectUnderControl, ServiceDeskListFilter> _dataListBuilder;

        public CustomControlBLL(
            IRepository<Entity> repository,
            IUnitOfWork unitOfWork,
            IMemoryCache cache,
            ICurrentUser currentUser,
            IObjectAccessBLL objectAccess,
            IUserAccessBLL userAccess,
            ILogger<CustomControlBLL> logger,
            IListViewBLL<ObjectUnderControl, ServiceDeskListFilter> dataListBuilder)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _currentUser = currentUser;
            _objectAccess = objectAccess;
            _userAccess = userAccess;
            _logger = logger;
            _dataListBuilder = dataListBuilder;
        }

        public Task<CustomControlDetails> GetCustomControlDetailsAsync(InframanagerObject inframanagerObject, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"Request custom control for user (ID = {_currentUser.UserId}) and {inframanagerObject}.");
            return _cache.GetOrCreateAsync(
                GetCacheKey(_currentUser.UserId, inframanagerObject),
                cacheEntry => LoadCustomControlDetailsAsync(_currentUser.UserId, inframanagerObject, cancellationToken));
        }

        private async Task<CustomControlDetails> LoadCustomControlDetailsAsync(Guid userId, InframanagerObject inframanagerObject, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace($"Loading custom control for user (ID = {userId}) and {inframanagerObject}.");
            return new CustomControlDetails
            {
                UserID = userId,
                ObjectID = inframanagerObject.Id,
                ClassID = inframanagerObject.ClassId,
                UnderControl = await _repository.AnyAsync(
                    GetPredicate(userId, inframanagerObject),
                    cancellationToken)
            };
        }

        public async Task<CustomControlDetails> SetCustomControlDetailsAsync(
            InframanagerObject inframanagerObject, 
            CustomControlData data,
            CancellationToken cancellationToken = default)
        {
            var userID = data.UserID ?? _currentUser.UserId;

            await SetCustomControlAsync(inframanagerObject, userID, data.UnderControl, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            _cache.Remove(GetCacheKey(userID, inframanagerObject));

            return new CustomControlDetails
            {
                UserID = userID,
                ObjectID = inframanagerObject.Id,
                ClassID = inframanagerObject.ClassId,
                UnderControl = data.UnderControl
            };
        }

        private static string GetCacheKey(Guid userId, InframanagerObject inframanagerObject)
        {
            return $"custom_control_{userId}_{inframanagerObject.Id}_{inframanagerObject.ClassId}";
        }

        private static Expression<Func<Entity, bool>> GetPredicate(Guid userId, InframanagerObject inframanagerObject)
        {
            return x => x.UserId == userId
                    && x.ObjectId == inframanagerObject.Id
                    && x.ObjectClass == inframanagerObject.ClassId;
        }

        public Task<ObjectUnderControl[]> GetListAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _dataListBuilder.BuildAsync(filterBy, cancellationToken);
        }

        public async Task SetCustomControlAsync(InframanagerObject imObject, Guid userID, bool underControl, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) is {(underControl ? "taking under control" : "releasing control over")} {imObject} {(userID == _currentUser.UserId ? string.Empty : $" on behalf of another user (ID = {userID})")}.");

            var entity = await _repository.FirstOrDefaultAsync(
                GetPredicate(userID, imObject),
                cancellationToken);

            if (entity == null && underControl)
            {

                if (!await _objectAccess.AccessIsGrantedAsync(_currentUser.UserId, imObject.Id, imObject.ClassId, cancellationToken: cancellationToken)
                    || (_currentUser.UserId != userID && !await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
                {
                    // если у пользователя нет доступа к объекту и он пытается его получить, ставя себя на контроль - отказ
                    // если пользователь пытается поставить объект на контроль кому-то, не имея в системе ролей - отказ
                    throw new AccessDeniedException($"Поставить на контроль объект {imObject} пользователю (ID = {userID})");
                }

                _repository.Insert(new Entity(userID, imObject));
            }

            if (entity != null && !underControl)
            {
                if (_currentUser.UserId != userID
                    && (!await _objectAccess.AccessIsGrantedAsync(_currentUser.UserId, imObject.Id, imObject.ClassId, cancellationToken: cancellationToken)
                        || !await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
                {
                    // если пользователь снимает с контроля другого пользователя объект, не имея к нему доступа или не имея в системе ролей - отказ
                    throw new AccessDeniedException($"Снять объект {imObject} с контроля пользователя (ID = {userID})");
                }

                _repository.Delete(entity);
            }
        }
    }
}
