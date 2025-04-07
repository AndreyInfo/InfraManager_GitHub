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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.Report
{
    internal class ReportBLL : IReportBLL, ISelfRegisteredService<IReportBLL>
    {
        private readonly IReadonlyRepository<Reports> _readOnlyReportRepository;
        private readonly IRepository<Reports> _reportRepository;
        private readonly IRepository<ReportFolder> _reportFoldersRepository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IValidateObject<ReportData> _reportDataValidator;
        private readonly IGuidePaggingFacade<Reports, ReportsForTable> _paggingFacade;
        private readonly IReportFolderBLL _reportFolderBLL;
        private readonly IValidatePermissions<Reports> _validatePermissionsReport;
        private readonly ILogger<ReportBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly ILocalizeText _localizeText;

        private readonly IInsertEntityBLL<Reports, ReportData> _insertReportEntityBLL;


        public ReportBLL(
            IReadonlyRepository<Reports> readOnlyReportRepository,
            IRepository<Reports> reportRepository,
            IRepository<ReportFolder> reportFoldersRepository,
            IUnitOfWork saveChangesCommand,
            IValidateObject<ReportData> reportDataValidator,
            IMapper mapper,
            IValidatePermissions<Reports> validatePermissions,
            ILogger<ReportBLL> logger,
            ICurrentUser currentUser,
            IGuidePaggingFacade<Reports, ReportsForTable> paggingFacade, 
            IReportFolderBLL reportFolderBLL,
            ILocalizeText localizeText,
            IInsertEntityBLL<Reports, ReportData> insertReportEntityBLL)
        {
            _readOnlyReportRepository = readOnlyReportRepository;
            _reportRepository = reportRepository;
            _reportFoldersRepository = reportFoldersRepository;
            _saveChangesCommand = saveChangesCommand;
            _reportDataValidator = reportDataValidator;
            _mapper = mapper;
            _paggingFacade = paggingFacade;
            _currentUser = currentUser;
            _logger = logger;
            _validatePermissionsReport = validatePermissions;
            _reportFolderBLL = reportFolderBLL;
            _localizeText = localizeText;
            _insertReportEntityBLL = insertReportEntityBLL;
        }

        public async Task InsertAsync(ReportData report, CancellationToken cancellationToken)
        {
            await _validatePermissionsReport.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);
            using var transaction =
             new TransactionScope(
                 TransactionScopeOption.Required,
                 new TransactionOptions { IsolationLevel = IsolationLevel.Serializable },
                 TransactionScopeAsyncFlowOption.Enabled);

            if (await HasDuplicateReportInParentReportFolderAsync(report.ReportFolderID, report.Name, cancellationToken))
            {
                throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorSameReportName), cancellationToken));
            }

            var reportEntity = await _insertReportEntityBLL.CreateAsync(report, cancellationToken);

            await LockFolderAsync(reportEntity.ReportFolderID, cancellationToken);
           
            await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
            transaction.Complete();
        }

        public async Task UpdateAsync(Guid id, ReportData report, CancellationToken cancellationToken)
        {
            await _validatePermissionsReport.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);
            await _reportDataValidator.ValidateOrRaiseErrorAsync(report, cancellationToken);
           
            using var transaction =
             new TransactionScope(
                 TransactionScopeOption.Required,
                 new TransactionOptions { IsolationLevel = IsolationLevel.Serializable },
                 TransactionScopeAsyncFlowOption.Enabled);

            var foundEntity = await _reportRepository.FirstOrDefaultAsync(p => p.ID == id, cancellationToken)
                              ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Report);

            if (await HasDuplicateReportInParentReportFolderWhenUpdateAsync(report.ReportFolderID, report.Name, foundEntity.Name, cancellationToken))
            {
                throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorSameReportName), cancellationToken));
            }

            foundEntity.DateModified = DateTime.UtcNow; 
            await ChangeFolderSecurity(foundEntity, report, cancellationToken);

            _mapper.Map(report, foundEntity);

            await _saveChangesCommand.SaveAsync(cancellationToken, IsolationLevel.Serializable);
            transaction.Complete();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissionsReport.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);
            var entity = await FindOrRaiseErrorAsync(id, cancellationToken);

            await UnlockFolderAsync(entity.ReportFolderID, cancellationToken, entity);

            _reportRepository.Delete(entity);

            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<ReportDetails> GetReportAsync(Guid id, CancellationToken cancellationToken)
        {
            await _validatePermissionsReport.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

            var report = await _readOnlyReportRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Report);
            return _mapper.Map<ReportDetails>(report);
        }

        public async Task<ReportForTableDetails[]> GetReportsAsync(ReportsFilter filter, CancellationToken cancellationToken)
        {
            await _validatePermissionsReport.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);
            var query = _readOnlyReportRepository.Query();

            if (filter.FolderID.HasValue)
            {
                var allChildFolderIDs = await _reportFolderBLL.GetAllGenerationsChildsFoldersIDsAsync((Guid)filter.FolderID, cancellationToken);

                query = query.Where(x => allChildFolderIDs.Contains(x.ReportFolderID));
            }

            var reports = await _paggingFacade.GetPaggingAsync(
                    filter,
                    query: query,
                    x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                    cancellationToken: cancellationToken);



            var resultReports = _mapper.Map<ReportWithPathDetails[]>(reports);
            foreach (var report in resultReports)
            {
                //TODO избавиться от n+1 проблемы
                report.StringFolder = await GetPathAsync(report.Folder, cancellationToken) + "/" + report.Folder.Name;
            }

            return _mapper.Map<ReportForTableDetails[]>(resultReports);
        }

        public async Task<ReportDetails[]> GetReportsAsync(CancellationToken cancellationToken = default)
        {
            var reports = await _readOnlyReportRepository.ToArrayAsync(cancellationToken);
            return _mapper.Map<ReportDetails[]>(reports);
        }

        public async Task UpdateReportDataAsync(Guid id, string data, CancellationToken cancellationToken = default)
        {
            var report = await _reportRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken) ?? throw new ObjectNotFoundException($"Report with ID = {id} not found");
            report.Data = data;
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        //TODO Избавиться от рекурсии, передлать на цикл 
        private async Task<string> GetPathAsync(ReportFolder reportFolder, CancellationToken cancellationToken, string path = null)
        {
            if (reportFolder.ID != Guid.Empty)
            {
                var folder = await _reportFoldersRepository.FirstOrDefaultAsync(x => x.ID == reportFolder.ReportFolderID, cancellationToken);
                if (folder != null)
                {
                    path = await GetPathAsync(folder, cancellationToken, path) + "/" + folder.Name;
                }
            }

            return path;

        }

        private async Task<bool> HasDuplicateReportInParentReportFolderAsync(Guid reportFolderID, string name, CancellationToken cancellationToken)
        {
            var reportsWithSameReportFolder = await _readOnlyReportRepository.ToArrayAsync(x => x.ReportFolderID == reportFolderID, cancellationToken);
            return reportsWithSameReportFolder.Any(x => x.Name == name);
        }

        private async Task<bool> HasDuplicateReportInParentReportFolderWhenUpdateAsync(Guid reportFolderID, string newName, string oldName, CancellationToken cancellationToken)
        {
            return oldName.Equals(newName)
                ? false
                : await HasDuplicateReportInParentReportFolderAsync(reportFolderID, newName, cancellationToken);
        }

        private async Task LockFolderAsync(Guid reportFolderID, CancellationToken cancellationToken)
        {
            if (reportFolderID != Guid.Empty)
            {
                return;
            }

            var folder =
                await _reportFoldersRepository.FirstOrDefaultAsync(x => x.ID == reportFolderID, cancellationToken);

            if (folder.ReportFolderID != Guid.Empty)
            {
                await LockFolderAsync(folder.ReportFolderID, cancellationToken);
            }

            if (folder.SecurityLevel != ReportFolderBLL.UNCHANGABLE_SECURITY_LEVEL)
            {
                folder.SecurityLevel = ReportFolderBLL.UNCHANGABLE_SECURITY_LEVEL;
            }
        }

        private async Task UnlockFolderAsync(Guid reportFolderID, CancellationToken cancellationToken, Reports report = null,
            bool isAscendingDelete = false)
        {
            if (reportFolderID != Guid.Empty)
            {
                return;
            }

            var folder =
                await _reportFoldersRepository.FirstOrDefaultAsync(x => x.ID == reportFolderID, cancellationToken);
            bool isContains = await IsContainsAnotherReportsInFolderAsync(folder, report, cancellationToken);

            if (folder.ReportFolderID != Guid.Empty)
            {
                if (isContains)
                {
                    isAscendingDelete = true;
                }

                await UnlockFolderAsync(folder.ReportFolderID, cancellationToken, isAscendingDelete: isAscendingDelete);
            }

            if (folder.SecurityLevel == ReportFolderBLL.UNCHANGABLE_SECURITY_LEVEL && !isContains && !isAscendingDelete)
            {
                folder.SecurityLevel = ReportFolderBLL.CHANGABLE_SECURITY_LEVEL;
            }
        }


        private async Task<bool> IsContainsAnotherReportsInFolderAsync(ReportFolder folder, Reports report, CancellationToken cancellationToken)
        {
            var reports = await _readOnlyReportRepository.ToArrayAsync(x => x.ReportFolderID == folder.ID, cancellationToken);
            reports = reports.Except(new[] { report }).ToArray();
            return reports.Length > 0;
        }


        private async Task ChangeFolderSecurity(Reports foundEntity, ReportData report, CancellationToken cancellationToken)
        {
            if (foundEntity.ReportFolderID != report.ReportFolderID)
            {
                var reportFolder = await _reportFoldersRepository.FirstOrDefaultAsync(x => x.ID == report.ReportFolderID, cancellationToken);
                var reportsInDestinationFolder = await _readOnlyReportRepository.ToArrayAsync(x => x.ReportFolderID == reportFolder.ID, cancellationToken);
                var isDestinationFolderEmpty = reportsInDestinationFolder.Except(new[] { foundEntity }).Count() == 0;

                var reportsInCurrentFolder = await _readOnlyReportRepository.ToArrayAsync(x => x.ReportFolderID == foundEntity.Folder.ID, cancellationToken);
                var isCurrentFolderEmpty = reportsInCurrentFolder.Except(new[] { foundEntity }).Count() == 0;

                var isDownChange = GetAllChildsFromFolderRecursive(reportFolder).Contains(foundEntity.Folder);
                var isUpperChange = GetAllChildsFromFolderRecursive(foundEntity.Folder).Contains(reportFolder);

                switch (isDownChange, isUpperChange, isDestinationFolderEmpty, isCurrentFolderEmpty)
                {
                    case (false, false, false, false):
                    case (false, true, false, true):
                    case (true, false, false, false):
                    case (true, false, true, false):
                    case (false, true, false, false):
                        break;

                    case (false, false, true, false):
                    case (false, true, true, false):
                    case (false, true, true, true):
                        await LockFolderAsync(reportFolder.ID, cancellationToken);
                        break;

                    case (true, false, false, true):
                    case (true, false, true, true):
                        await UnlockFolderAsync(foundEntity.Folder.ID, cancellationToken, foundEntity);
                        await LockFolderAsync(reportFolder.ID, cancellationToken);
                        break;

                    case (false, false, false, true):
                        await UnlockFolderAsync(foundEntity.Folder.ID, cancellationToken, foundEntity);
                        break;

                    case (false, false, true, true):
                        await LockFolderAsync(reportFolder.ID, cancellationToken);
                        await UnlockFolderAsync(foundEntity.Folder.ID, cancellationToken, foundEntity);
                        break;
                }
            }
        }

        private List<ReportFolder> GetAllChildsFromFolderRecursive(ReportFolder reportFolder)
        {
            List<ReportFolder> childFolders = new List<ReportFolder>();

            if (reportFolder.ID != Guid.Empty)
            {
                foreach (var child in reportFolder.Childs)
                {
                    if (child.ID != Guid.Empty)
                    {
                        childFolders.Add(child);
                        childFolders.AddRange(GetAllChildsFromFolderRecursive(child));
                    }
                }
            }
            return childFolders;
        }

        private async Task<Reports> FindOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken)
        {
            var report = await _readOnlyReportRepository.FirstOrDefaultAsync(x => x.ID.Equals(id), cancellationToken)
                         ?? throw new ObjectNotFoundException($"Report (ID = {id})");

            return report;
        }
    }
}
