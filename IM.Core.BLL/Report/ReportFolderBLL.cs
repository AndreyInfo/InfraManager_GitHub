using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.Report
{
    internal class ReportFolderBLL : IReportFolderBLL, ISelfRegisteredService<IReportFolderBLL>
    {
        private readonly IReadonlyRepository<ReportFolder> _readOnlyRepository;
        private readonly IReadonlyRepository<Reports> _reportReadOnlyRepository;
        private readonly IRepository<ReportFolder> _repository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IValidateObject<ReportFolderData> _validator;
        private readonly IValidatePermissions<ReportFolder> _validatePermissions;
        private readonly ILogger<ReportFolderBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly ILocalizeText _localizeText;

        private readonly Guid DefaultReportFolderID = Guid.Empty;
        public const byte UNCHANGABLE_SECURITY_LEVEL = 1;
        public const byte CHANGABLE_SECURITY_LEVEL = 0;

        private readonly IMapper _mapper;
        public ReportFolderBLL(IReadonlyRepository<ReportFolder> readOnlyRepository,
            IReadonlyRepository<Reports> reportReadOnlyRepository,
            IRepository<ReportFolder> repository,
            IUnitOfWork saveChangesCommand,
            IValidateObject<ReportFolderData> validator,
            IMapper mapper,
            IValidatePermissions<ReportFolder> validatePermissions, 
            ILogger<ReportFolderBLL> logger, 
            ICurrentUser currentUser,
            ILocalizeText localizeText)
        {
            _readOnlyRepository = readOnlyRepository;
            _reportReadOnlyRepository = reportReadOnlyRepository;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _validator = validator;
            _mapper = mapper;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
            _localizeText = localizeText;
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

            await DeleteIncludeFoldersRecursive(id, cancellationToken);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<ReportFolderDetails> GetReportFolderAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

            var folder = await _readOnlyRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.ReportFolder);

            return _mapper.Map<ReportFolderDetails>(folder);
        }


        public async Task<ReportFolderDetails[]> GetReportFoldersAsync(ReportFolderFilter filter,
            CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var foldersQuery = _readOnlyRepository.Query();

            if (filter.ParentFolderID.HasValue)
            {
                foldersQuery = foldersQuery.Where(x => x.ReportFolderID == filter.ParentFolderID);
            }

            return _mapper.Map<ReportFolderDetails[]>(await foldersQuery.Where(x => x.ID != DefaultReportFolderID)
                .ExecuteAsync(cancellationToken));
        }

        public async Task<ReportFolderDetails> InsertAsync(ReportFolderData folder, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

            using var transaction =
              new TransactionScope(
                  TransactionScopeOption.Required,
                  new TransactionOptions { IsolationLevel = IsolationLevel.Serializable },
                  TransactionScopeAsyncFlowOption.Enabled);

            if (await HasDuplicateFolderInParentReportFolderAsync(folder.ReportFolderID, folder.Name, cancellationToken))
            {
                throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorSameReportFolderName), cancellationToken));
            }

            var entity = _mapper.Map<ReportFolder>(folder);
            _repository.Insert(entity);

            await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
            transaction.Complete();
            return _mapper.Map<ReportFolderDetails>(entity);
        }
       
        public async Task PutAsync(Guid id, ReportFolderData folder, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

            await _validator.ValidateOrRaiseErrorAsync(folder, cancellationToken);
            using var transaction =
              new TransactionScope(
                  TransactionScopeOption.Required,
                  new TransactionOptions { IsolationLevel = IsolationLevel.Serializable },
                  TransactionScopeAsyncFlowOption.Enabled);

            var foundEntity = await _repository.FirstOrDefaultAsync(p => p.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, "Report folder not found");

            if (await HasDuplicateFolderInParentReportFolderWhenUpdateAsync(folder.ReportFolderID, folder.Name, foundEntity.Name, cancellationToken))
            {
                throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorSameReportFolderName), cancellationToken));
            }

            var reports = await _reportReadOnlyRepository.ToArrayAsync(x => x.ReportFolderID == foundEntity.ID,
                    cancellationToken);
            
            if (reports.Length != 0)
            {
                await UpdateSecurityParentFolders(folder, foundEntity, cancellationToken);
            }

            _mapper.Map(folder, foundEntity);

            await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
            transaction.Complete();
        }

        public async Task<Guid[]> GetAllGenerationsChildsFoldersIDsAsync(Guid parentFolderID, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var childIDs = new List<Guid>
            {
                parentFolderID
            };

            var childFolders = (await _repository.With(x => x.Childs).FirstOrDefaultAsync(x=> x.ID == parentFolderID, cancellationToken)).Childs.Where(x=>x.ID != parentFolderID);

            foreach (var folder in childFolders)
            {
                var folderChilds = await GetAllGenerationsChildsFoldersIDsAsync(folder.ID,cancellationToken);
                childIDs.AddRange(folderChilds);
            }

            return childIDs.ToArray();
        }

        private async Task<bool> HasDuplicateFolderInParentReportFolderAsync(Guid reportFolderID, string name, CancellationToken cancellationToken)
        {
            var reportsWithSameReportFolder = await _readOnlyRepository.ToArrayAsync(x => x.ReportFolderID == reportFolderID, cancellationToken);
            return reportsWithSameReportFolder.Any(x=>x.Name == name);
        }

        private async Task<bool> HasDuplicateFolderInParentReportFolderWhenUpdateAsync(Guid reportFolderID, string newName, string oldName, CancellationToken cancellationToken)
        {
            return oldName.Equals(newName)
                ? false
                : await HasDuplicateFolderInParentReportFolderAsync(reportFolderID, newName, cancellationToken);
        }

        private async Task UpdateSecurityParentFolders(ReportFolderData updateFolder, ReportFolder oldFolder, CancellationToken cancellationToken)
        {
            if (updateFolder.ReportFolderID != oldFolder.ReportFolderID)
            {
                return;
            }

            if (oldFolder.ReportFolderID != Guid.Empty)
            {
                await ChangeSecurityLevel(oldFolder.ReportFolderID, (folder) =>
                {
                    if (folder.SecurityLevel == UNCHANGABLE_SECURITY_LEVEL)
                    {
                        folder.SecurityLevel = CHANGABLE_SECURITY_LEVEL;
                    }
                }, cancellationToken);
            }

            if (updateFolder.ReportFolderID != Guid.Empty)
            {
                await ChangeSecurityLevel(updateFolder.ReportFolderID, (folder) =>
                {
                    if (folder.SecurityLevel == CHANGABLE_SECURITY_LEVEL)
                    {
                        folder.SecurityLevel = UNCHANGABLE_SECURITY_LEVEL;
                    }
                }, cancellationToken);
            }
            
        }

        private async Task ChangeSecurityLevel(Guid id, Action<ReportFolder> changeFunc, CancellationToken cancellationToken)
        {
            if (id != Guid.Empty)
            {
                var folder = await _repository.FirstOrDefaultAsync(p => p.ID == id, cancellationToken);
                var isContainsReportsInFolder = _reportReadOnlyRepository.Any(x => x.ReportFolderID == folder.ID);

                if (!isContainsReportsInFolder)
                {
                    changeFunc(folder);

                    await ChangeSecurityLevel(folder.ReportFolderID, changeFunc, cancellationToken);
                }
            }
        }
       
        

        private async Task<ReportFolder> FindOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken)
        {
            var reportFolder = await _readOnlyRepository.FirstOrDefaultAsync(x => x.ID.Equals(id), cancellationToken);
            return reportFolder ?? throw new ObjectNotFoundException($"Report folder (ID = {id})");
        }

        //TODO избавить от рекурсии, передлать на цикл
        private async Task DeleteIncludeFoldersRecursive(Guid id, CancellationToken cancellationToken)
        {
            var entity = await FindOrRaiseErrorAsync(id, cancellationToken);

            foreach (var childFolder in entity.Childs)
            {
                if (childFolder.ID != Guid.Empty)
                {
                    await DeleteIncludeFoldersRecursive(childFolder.ID, cancellationToken);
                }
            }

            if (entity.SecurityLevel == UNCHANGABLE_SECURITY_LEVEL)
                throw new InvalidObjectException("Папку нельзя удалить"); //TODO locale

            _repository.Delete(entity);
        }
    }
}
