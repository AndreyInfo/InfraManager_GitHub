using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    [Obsolete("Use InfraManager.BLL.UsersActivityType.UserActivityTypeBLL instead.")]
    internal class UserActivityTypeBLL : IUserActivityTypeBLL, ISelfRegisteredService<IUserActivityTypeBLL>
    {
        private readonly IReadonlyRepository<UserActivityType> _readOnlyRepository;
        private readonly IRepository<UserActivityType> _repository;
        private readonly IUnitOfWork _saveChangesCommand;

        private readonly IMapper _mapper;

        private readonly IValidatePermissions<UserActivityType> _validatePermissions;
        private readonly ILogger<UserActivityTypeBLL> _logger;
        private readonly ICurrentUser _currentUser;
        public UserActivityTypeBLL(IMapper mapper,
                            IReadonlyRepository<UserActivityType> readOnlyRepository,
                            IRepository<UserActivityType> repository,
                            IUnitOfWork saveChangesCommand,
                            IValidatePermissions<UserActivityType> validatePermissions,
                            ILogger<UserActivityTypeBLL> logger,
                            ICurrentUser currentUser)
        {
            _mapper = mapper;
            _readOnlyRepository = readOnlyRepository;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _validatePermissions = validatePermissions;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(UserActivityTypeDetails userActivityTypeDetails, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

            var entity = _mapper.Map<UserActivityType>(userActivityTypeDetails);

            _repository.Insert(entity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return entity.ID;
        }

        public async Task<Guid> UpdateAsync(UserActivityTypeDetails userActivityTypeDetails, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

            if (userActivityTypeDetails.ID == Guid.Empty)
                throw new ObjectNotFoundException("UserActivityType not found");
            else if (userActivityTypeDetails.Name == null)
                //TODO добавить локализацию
                throw new InvalidObjectException("Имя для вида деятельности не задано");

            var foundEntity = await _repository.FirstOrDefaultAsync(p => p.ID == userActivityTypeDetails.ID, cancellationToken);

            if (foundEntity == null)
                throw new ObjectNotFoundException<Guid>(userActivityTypeDetails.ID, "UserActivityType not found");

            _mapper.Map(userActivityTypeDetails, foundEntity);

            await _saveChangesCommand.SaveAsync(cancellationToken);

            return foundEntity.ID;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

            var entity = await FindOrRaiseErrorAsync(id, cancellationToken);

            if (entity.Childs.Count() > 0)
            {
                foreach (var child in entity.Childs)
                {
                    await DeleteAsync(child.ID, cancellationToken);
                }
            }

            _repository.Delete(entity);

            await _saveChangesCommand.SaveAsync(cancellationToken);
            return true;
        }

        public async Task<UserActivityTypeDetails[]> FindByParentAsync(Guid? parentID, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var entities = await _readOnlyRepository.ToArrayAsync(x => x.ParentID == parentID, cancellationToken);

            var result = _mapper.Map<UserActivityTypeDetails[]>(entities);
            return result;
        }

        public async Task<UserActivityTypeDetails[]> ListAsync(CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var userActivityTypeList = await _readOnlyRepository.ToArrayAsync(x => x.ID != default, cancellationToken);
            var result = _mapper.Map<UserActivityTypeDetails[]>(userActivityTypeList);

            return result;
        }

        private async Task<UserActivityType> FindOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken)
        {
            var userActivityType = await _readOnlyRepository.FirstOrDefaultAsync(x => x.ID.Equals(id), cancellationToken);
            return userActivityType ?? throw new ObjectNotFoundException($"User activity type (ID = {id})");
        }
    }
}
