using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Calls.DTO;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Calls
{
    [Obsolete]
    internal class CallTypeBLL : ICallTypeBLL, ISelfRegisteredService<ICallTypeBLL> //TODO Убрать дубль BLL (namespace InfraManager.BLL.ServiceDesk.Calls CallTypeBLL)
    {
        private readonly IRepository<CallType> _callTypeRepository;
        private readonly IMapper _mapper;
        private readonly IReadonlyRepository<CallType> _callTypeReadonlyRepository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IValidatePermissions<CallType> _validatePermissions;
        private readonly ILogger<CallTypeBLL> _logger;
        private readonly ICurrentUser _currentUser;

        public CallTypeBLL(IMapper mapper,
                           IReadonlyRepository<CallType> callTypeReadonlyRepository,
                           IRepository<CallType> callTypRepository,
                           IUnitOfWork saveChangesCommand,
                           IValidatePermissions<CallType> validatePermissions,
                           ILogger<CallTypeBLL> logger,
                           ICurrentUser currentUser)
        {
            _mapper = mapper;
            _callTypeReadonlyRepository = callTypeReadonlyRepository;
            _callTypeRepository = callTypRepository;
            _saveChangesCommand = saveChangesCommand;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<string[]> DeleteAsync(DeleteModel<Guid>[] deleteModels, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

            var result = new List<string>();
            foreach (var model in deleteModels)
            {
                try
                {
                    _callTypeRepository.Delete(_mapper.Map<CallType>(model));
                }
                catch
                {
                    result.Add(model.Name);
                }
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);

            return result.ToArray();
        }

        public async Task<CallTypeDetails[]> GetByParentIDAsync(Guid? parentID, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var query = _callTypeReadonlyRepository.With(x => x.Parent);
            var data = await query.ToArrayAsync(x => x.Parent.ID == parentID, cancellationToken);
            var result = _mapper.Map<CallTypeDetails[]>(data);

            return result;
        }


        public async Task<CallTypeDetails[]> GetPathItemAsync(Guid? id, CancellationToken cancellationToken = default)
        {
            var pathItems = new List<CallType>();
            _callTypeReadonlyRepository.With(x => x.Parent);

            while (id.HasValue)
            {
                var model = await _callTypeReadonlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
                pathItems.Add(model);
                if (model.Parent == null)
                {
                    break;
                }

                id = model.Parent.ID;
            }

            var result = _mapper.Map<CallTypeDetails[]>(pathItems);

            return result;
        }

        public async Task<CallTypeDetails[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var result = await _callTypeReadonlyRepository.ToArrayAsync(cancellationToken);
            return _mapper.Map<CallTypeDetails[]>(result);
        }
    }
}
