using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    internal class ManhoursWorkBLL : IManhoursWorkBLL, ISelfRegisteredService<IManhoursWorkBLL>
    {
        private const int MaxManhourForUserPerDay = 60 * 24;
        private const string ErrorTextFormat = "ManhoursLimitExceedFormat";

        private readonly ICurrentUser _currentUser;
        private readonly ILoadEntity<Guid, ManhoursWork> _loader;
        private readonly IGetEntityArrayBLL<Guid, ManhoursWork, ManhoursWorkDetails, ManhoursListFilter> _arrayLoader;
        private readonly IBuildEntityQuery<ManhoursWork, ManhoursWorkDetails, ManhoursListFilter> _queryBuilder;
        private readonly IBuildObject<ManhoursWorkDetails, ManhoursWork> _detailsBuilder;
        private readonly IObjectAccessBLL _parentObjectAccess;
        private readonly IServiceMapper<ObjectClass, IFindEntityWithManhours> _objectsFinder;
        private readonly IInsertEntityBLL<ManhoursWork, ManhoursWorkData> _insertService;
        private readonly IModifyEntityBLL<Guid, ManhoursWork, ManhoursWorkData, ManhoursWorkDetails> _modifyService;
        private readonly IRemoveEntityBLL<Guid, ManhoursWork> _removeService;
        private readonly ILocalizeText _localization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ManhoursWorkBLL> _logger;

        public ManhoursWorkBLL(ILogger<ManhoursWorkBLL> logger,
            ICurrentUser currentUser,
            ILoadEntity<Guid, ManhoursWork> loader,
            IGetEntityArrayBLL<Guid, ManhoursWork, ManhoursWorkDetails, ManhoursListFilter> arrayLoader,
            IBuildEntityQuery<ManhoursWork, ManhoursWorkDetails, ManhoursListFilter> queryBuilder,
            IBuildObject<ManhoursWorkDetails, ManhoursWork> detailsBuilder,
            IUnitOfWork unitOfWork,
            IObjectAccessBLL parentObjectAccess,
            IServiceMapper<ObjectClass, IFindEntityWithManhours> objectsFinder,
            IInsertEntityBLL<ManhoursWork, ManhoursWorkData> insertService,
            IModifyEntityBLL<Guid, ManhoursWork, ManhoursWorkData, ManhoursWorkDetails> modifyService,
            IRemoveEntityBLL<Guid, ManhoursWork> removeService,
            ILocalizeText localization)
        {
            _currentUser = currentUser;
            _loader = loader;
            _arrayLoader = arrayLoader;
            _queryBuilder = queryBuilder;
            _detailsBuilder = detailsBuilder;
            _unitOfWork = unitOfWork;
            _parentObjectAccess = parentObjectAccess;
            _objectsFinder = objectsFinder;
            _insertService = insertService;
            _modifyService = modifyService;
            _removeService = removeService;
            _localization = localization;
            _logger = logger;
        }

        public async Task<ManhoursWorkDetails> AddWorkAsync(InframanagerObject parentObject, ManhoursWorkData data,
            CancellationToken cancellationToken)
        {
            var manhoursWork = await _insertService.CreateAsync(data, cancellationToken);
            var parentEntity = await FindParentEntityAsync(manhoursWork, cancellationToken);
            parentEntity.OnManhoursWorkAdded();
            await _unitOfWork.SaveAsync(cancellationToken);
            return await _detailsBuilder.BuildAsync(manhoursWork, cancellationToken);
        }

        public async Task<ManhoursWorkDetails> UpdateWorkAsync(Guid id, ManhoursWorkData data,
            CancellationToken cancellationToken)
        {
            var manhoursWork = await _modifyService.ModifyAsync(id, data, cancellationToken);
            var parentEntity = await FindParentEntityAsync(manhoursWork, cancellationToken);
            parentEntity.OnManhoursWorkAdded();
            await _unitOfWork.SaveAsync(cancellationToken);
            return await _detailsBuilder.BuildAsync(manhoursWork, cancellationToken);
        }

        public async Task<ManhoursWorkDetails> AddManhourEntryAsync(Guid id, ManhourData data,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) adding new {typeof(ManhoursEntry).Name}.");
            _logger.TraceObject($"New {typeof(ManhoursEntry).Name} from user (ID = {_currentUser.UserId})", data);
            var manhoursWork = await _loader.LoadAsync(id, cancellationToken);
            var parentEntity = await FindParentEntityAsync(manhoursWork, cancellationToken);
            using(var transaction = TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
            {
                await ValidateUserManhours(manhoursWork, data, cancellationToken);
                manhoursWork.AddManhour(data.Value, data.UtcDate);
                _logger.LogTrace($"New {typeof(ManhoursEntry).Name} is created by user (ID = {_currentUser.UserId})");
                parentEntity.IncrementTotalManhours(data.Value);
                await _unitOfWork.SaveAsync(cancellationToken, IsolationLevel.Serializable);
                transaction.Complete();
            }
            _logger.LogTrace($"New {typeof(ManhoursEntry).Name} created by user (ID = {_currentUser.UserId}) saved to DB");
            return await _detailsBuilder.BuildAsync(manhoursWork, cancellationToken);
        }

        public async Task<ManhoursWorkDetails> UpdateManhourEntryAsync(Guid id, ManhourData data,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) is modifying {typeof(ManhoursEntry).Name} (id = {id}).");
            _logger.TraceObject($"Update to {typeof(ManhoursEntry).Name} (id = {id}) from user (ID = {_currentUser.UserId})", data);
            var manhoursWork = await _loader.LoadAsync(id, cancellationToken);
            var parentEntity = await FindParentEntityAsync(manhoursWork, cancellationToken);
            using(var transaction = TransactionScopeCreator.Create(IsolationLevel.Serializable, TransactionScopeOption.Required))
            {
                await ValidateUserManhours(manhoursWork, data, cancellationToken);
                var current = manhoursWork.Entries.FirstOrDefault(i => i.ID == data.ID) ??
                              throw new ObjectNotFoundException($"Manhour with id ${data.ID} of work ${id} not found");
                _logger.LogTrace($"{typeof(ManhoursEntry)} (id = {id}) is found.");
                parentEntity.IncrementTotalManhours(data.Value - current.Value);
                current.Value = data.Value;
                current.UtcDate = data.UtcDate;
                await _unitOfWork.SaveAsync(cancellationToken, IsolationLevel.Serializable);
                transaction.Complete();
            }
            _logger.LogTrace($"Modified {typeof(ManhoursEntry)} (id = {id}) saved to DB.");
            return await _detailsBuilder.BuildAsync(manhoursWork, cancellationToken);
        }

        public async Task<IReadOnlyList<ManhoursWorkDetails>> GetWorkDetailsArrayAsync(
            ManhoursListFilter manhoursListFilter,
            CancellationToken cancellationToken)
        {
            return await _arrayLoader.ArrayAsync(manhoursListFilter, cancellationToken);
        }

        public async Task DeleteWorkAsync(Guid workID, CancellationToken cancellationToken)
        {
            var manhoursWork = await _loader.LoadAsync(workID, cancellationToken);

            var parentEntity = await FindParentEntityAsync(manhoursWork, cancellationToken);
            parentEntity.DecrementTotalManhours(manhoursWork.Entries.Sum(e => e.Value));
            await _removeService.RemoveAsync(workID, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task DeleteManhourEntryAsync(Guid workID, Guid id,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User (ID = {_currentUser.UserId}) is deleting {typeof(ManhoursEntry).Name}.");
            var manhoursWork = await _loader.LoadAsync(workID, cancellationToken);

            var manhour = manhoursWork.Entries.FirstOrDefault(m => m.ID == id) ??
                          throw new ObjectNotFoundException($"Manhour with id ${id} of work ${workID} not found");

            manhoursWork.Entries.Remove(manhour);
            var parentEntity = await FindParentEntityAsync(manhoursWork, cancellationToken);
            parentEntity.DecrementTotalManhours(manhour.Value);
            await _unitOfWork.SaveAsync(cancellationToken);
            _logger.LogTrace($"{typeof(ManhoursEntry)} (id = {id}) deleted from DB.");
        }

        public async Task<ManhoursWorkDetails> GetWorkDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var manhoursWork = await _loader.LoadAsync(id, cancellationToken);
            return await _detailsBuilder.BuildAsync(manhoursWork, cancellationToken);
        }

        private async Task ValidateUserManhours(ManhoursWork work, ManhourData newData, CancellationToken cancellationToken)
        {
            var currentSum = (await _queryBuilder.Query(new ManhoursListFilter
            {
                ExecutorID = _currentUser.UserId
            }).ExecuteAsync(cancellationToken))
                .Sum(m => m.Entries.Where(_ => _.UtcDate.Date == newData.UtcDate.Date && _.ID != newData.ID)
                .Sum(_ => _.Value));

            if (currentSum + newData.Value > MaxManhourForUserPerDay)
                throw new InvalidObjectException(string.Format(await _localization.LocalizeAsync(ErrorTextFormat, cancellationToken),
                    work.Description, work.Executor.FullName));
        }

        private async Task<IHaveManhours> FindParentEntityAsync(ManhoursWork manhoursWork,
            CancellationToken cancellationToken)
        {
            if (!await _parentObjectAccess.AccessIsGrantedAsync(_currentUser.UserId, manhoursWork.ObjectID,
                    manhoursWork.ObjectClassID))
            {
                throw new AccessDeniedException(manhoursWork.ObjectID, manhoursWork.ObjectClassID);
            }

            var parentEntity =
                await _objectsFinder.Map(manhoursWork.ObjectClassID)
                    .FindAsync(manhoursWork.ObjectID, cancellationToken) ??
                throw new ObjectNotFoundException<Guid>(manhoursWork.ObjectID, manhoursWork.ObjectClassID);

            return parentEntity;
        }
    }
}