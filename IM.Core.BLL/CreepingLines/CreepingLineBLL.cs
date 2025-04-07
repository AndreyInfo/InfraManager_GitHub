using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.CreepingLines
{
    internal class CreepingLineBLL : ICreepingLineBLL, ISelfRegisteredService<ICreepingLineBLL>
    {
        private readonly IReadonlyRepository<CreepingLine> _readOnlyRepository;
        private readonly IRepository<CreepingLine> _repository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IValidateObject<CreepingLineData> _creepingLineDetailsValidator;
        private readonly IValidatePermissions<Role> _validatePermissions;
        private readonly ILogger<CreepingLineBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public CreepingLineBLL(IMapper mapper,
                            IReadonlyRepository<CreepingLine> readOnlyRepository,
                            IRepository<CreepingLine> repository,
                            IUnitOfWork saveChangesCommand,
                            IValidateObject<CreepingLineData> creepingLineDetailsValidator,
                            IValidatePermissions<Role> validatePermissions,
                            ILogger<CreepingLineBLL> logger,
                            ICurrentUser currentUser)
        {
            _mapper = mapper;
            _readOnlyRepository = readOnlyRepository;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _creepingLineDetailsValidator = creepingLineDetailsValidator;
            _validatePermissions = validatePermissions;
            _currentUser = currentUser;
            _logger = logger;
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete,
                cancellationToken);
            
            var entity = await FindOrRaiseErrorAsync(id, cancellationToken);
            _repository.Delete(entity);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<CreepingLineDetails> GetAsync(Guid id, CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails,
                token);

            var creepingLine = await _readOnlyRepository.FirstOrDefaultAsync(x => x.ID.Equals(id), token)
                               ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CreepingLine);

            return _mapper.Map<CreepingLineDetails>(creepingLine);
        }

        public async Task<CreepingLineDetails[]> ListAsync(CreepingLineFilter filter, CancellationToken token = default)
        {
            var query = _readOnlyRepository.Query().Where(x => x.ID != default);

            if (filter.Visible.HasValue)
            {
                query = query.Where(x => x.Visible == filter.Visible);
            }

            var creepingLines = await query.ExecuteAsync(token);
            return _mapper.Map<CreepingLineDetails[]>(creepingLines);
        }

        public async Task<Guid> InsertAsync(CreepingLineData data, CancellationToken token = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
                token);
            
            await _creepingLineDetailsValidator.ValidateOrRaiseErrorAsync(data, token);
            
            var entity = _mapper.Map<CreepingLine>(data);
            _repository.Insert(entity);

            await _saveChangesCommand.SaveAsync(token);

            return entity.ID;
        }

        public async Task<Guid> UpdateAsync(Guid id, CreepingLineData data, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update,
                cancellationToken);
            
            await _creepingLineDetailsValidator.ValidateOrRaiseErrorAsync(data, cancellationToken);

            var foundEntity =
                await _repository.FirstOrDefaultAsync(p => p.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, "CreepingLine not found");
            _mapper.Map(data, foundEntity);

            await _saveChangesCommand.SaveAsync(cancellationToken);

            return foundEntity.ID;
        }

        private Task<CreepingLine> FindOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken)
        {
            return _readOnlyRepository.FirstOrDefaultAsync(x => x.ID.Equals(id), cancellationToken) ??
                   throw new ObjectNotFoundException($"Creeping Line (ID = {id})");
        }
    }
}
