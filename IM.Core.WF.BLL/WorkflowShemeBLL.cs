using IM.Core.WF.BLL.Interfaces;
using IM.Core.WF.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Message;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.WF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Core.WF.BLL
{
    internal static class WorkflowSchemeHelpExtensions
    {
        public static IQueryable<WorkflowSchemeModel> WorkflowSchemeSelect(this IQueryable<WorkFlowScheme> query)
        {
            return query.Select(x => new WorkflowSchemeModel
            {
                ID = x.Id,
                WorkflowSchemeFolderID = x.WorkflowSchemeFolderID,
                ObjectClassID = x.ObjectClassID,
                Identifier = x.Identifier,
                Name = x.Name,
                Note = x.Note,
                MajorVersion = (ushort)x.MajorVersion,
                MinorVersion = (ushort)x.MinorVersion,
                Status = x.Status,
                UtcDateModified = x.UtcDateModified,
                ModifierID = x.ModifierID,
                UtcDatePublished = x.UtcDatePublished,
                PublisherID = x.PublisherID,
                TraceIsEnabled = x.TraceIsEnabled
            });
        }
    }

    // TODO объединить с осноным BLL схем.
    public class WorkflowShemeBLL : IWorkflowShemeBLL, ISelfRegisteredService<IWorkflowShemeBLL>
    {
        private readonly IUnitOfWork saveChangesCommand;
        private readonly IReadonlyRepository<ChangeRequest> rfcRepository;
        private readonly IReadonlyRepository<Call> callRepository;
        private readonly IReadonlyRepository<Message> massageRepository;
        private readonly IReadonlyRepository<Problem> problemRepository;
        private readonly IReadonlyRepository<WorkOrder> workOrderRepository;
        private readonly IReadonlyRepository<WorkflowTracking> workflowTrackingRepository;
        private readonly IRepository<WorkFlowScheme> workFlowSchemeRepository;
        private readonly IReadonlyRepository<CustomWorkflowFilter> customFilterRepository;
        private readonly IReadonlyRepository<Workflow> workFlowRepository;

        public WorkflowShemeBLL(
                    IUnitOfWork saveChangesCommand,
                    IReadonlyRepository<ChangeRequest> rfcRepository,
                    IReadonlyRepository<Call> callRepository,
                    IReadonlyRepository<Message> massageRepository,
                    IReadonlyRepository<Problem> problemRepository,
                    IReadonlyRepository<WorkOrder> workOrderRepository,
                    IReadonlyRepository<WorkflowTracking> workflowTrackingRepository,
                    IReadonlyRepository<Workflow> workFlowRepository,
                    IRepository<WorkFlowScheme> workFlowSchemeRepository,
                    IReadonlyRepository<CustomWorkflowFilter> customFilterRepository)
        {
            this.saveChangesCommand = saveChangesCommand;
            this.rfcRepository = rfcRepository;
            this.callRepository = callRepository;
            this.massageRepository = massageRepository;
            this.problemRepository = problemRepository;
            this.workOrderRepository = workOrderRepository;
            this.workFlowSchemeRepository = workFlowSchemeRepository;
            this.customFilterRepository = customFilterRepository;
            this.workFlowRepository = workFlowRepository;
            this.workflowTrackingRepository = workflowTrackingRepository;
        }

        public WorkflowSchemeModel Get(Guid id)
        {
            return workFlowSchemeRepository
                .Query()
                .Where(x => x.Id == id)
                .WorkflowSchemeSelect()
                .FirstOrDefault();
        }

        public WorkflowSchemeModel[] GetActiveSchemes()
        {
            return workFlowSchemeRepository
                        .Query()
                        .Where(x => x.Status == 1 || x.Status == 2)
                        .WorkflowSchemeSelect()
                        .ToArray();
        }

        public WorkflowSchemeModel[] GetAllByParent(Guid? workflowSchemeFolderID)
        {
            if (!workflowSchemeFolderID.HasValue)
            {
                return workFlowSchemeRepository.Query()
                            .Where(x => x.WorkflowSchemeFolderID == null &&
                                !workFlowSchemeRepository.Query().Any(z => z.Identifier == x.Identifier && z.WorkflowSchemeFolderID != null))
                            .WorkflowSchemeSelect()
                            .ToArray();
            }
            else
            {
                return workFlowSchemeRepository.Query()
                            .Where(x => workFlowSchemeRepository.Query()
                                            .Any(z => z.Identifier == x.Identifier &&
                                                      z.WorkflowSchemeFolderID == workflowSchemeFolderID))
                            .WorkflowSchemeSelect()
                            .ToArray();
            }
        }

        public string GetVisualScheme(Guid id)
        {
            return workFlowSchemeRepository.Query()
                        .Where(x => x.Id == id)
                        .Select(x => x.VisualScheme)
                        .FirstOrDefault();
        }

        public string GetLogicalScheme(Guid id)
        {
            return workFlowSchemeRepository.Query()
                        .Where(x => x.Id == id)
                        .Select(x => x.LogicalScheme)
                        .FirstOrDefault();
        }

        public WorkflowSchemeModel GetPublishedScheme(string identifier)
        {
            return workFlowSchemeRepository.Query()
                        .Where(x => x.Identifier == identifier && x.Status == 1)
                        .OrderByDescending(x => x.MajorVersion)
                        .ThenByDescending(x => x.MinorVersion)
                        .WorkflowSchemeSelect()
                        .FirstOrDefault();
        }

        public (ushort majorV, ushort minorV) GetMaxVersion(string identifier)
        {
            ushort majorV = default;
            ushort minorV = default;
            var vers = workFlowSchemeRepository.Query()
                     .Where(x => x.Identifier == identifier)
                     .OrderByDescending(x => x.MajorVersion)
                     .ThenByDescending(x => x.MinorVersion)
                     .Select(x => new
                     {
                         x.MajorVersion,
                         x.MinorVersion
                     })
                     .FirstOrDefault();
            if (vers != null)
            {
                majorV = (ushort)vers.MajorVersion;
                minorV = (ushort)vers.MinorVersion;
            }
            return (majorV, minorV);
        }

        public List<Tuple<Guid, string, string, byte, string, string>> GetCustomFilterSchemes(Guid userID, int objectClassID)
        {
            return workFlowSchemeRepository.Query()
                    .Where(x => x.ObjectClassID == objectClassID &&
                            customFilterRepository.Query().Any(z => z.UserID == userID && z.WorkflowSchemeID == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.MajorVersion,
                        x.MinorVersion,
                        x.Status,
                        x.LogicalScheme,
                        x.Identifier
                    })
                    .ToList()
                    .Select(x => Tuple.Create(x.Id, x.Name, 
                                              string.Join(".", x.MajorVersion.ToString(), x.MinorVersion.ToString()),
                                              x.Status, x.LogicalScheme, x.Identifier))
                    .ToList();
        }

        public List<Guid> GetBlockedSchemes()
        {
            var listIds = workFlowSchemeRepository.Query()
                    .Where(x => x.Status == 2 &&
                           !workFlowRepository.Query().Any(z => z.WorkflowSchemeID == x.Id))
                    .Select(x => x.Id)
                    .ToList();
            if (listIds.Count != 0)
            {
                workFlowSchemeRepository.Query()
                         .Where(x => listIds.Contains(x.Id))
                         .ToList()
                         .ForEach(x => x.Status = 3);
                saveChangesCommand.Save();
            }
            return listIds;
        }

        public List<Tuple<Guid, string, string, string>> GetLogicalSchemesForExistsObjects()
        {
            return workFlowSchemeRepository.Query()
                           .Where(x => x.Status == 1)
                           .Select(x => new
                           {
                               x.Id,
                               x.Identifier,
                               x.MajorVersion,
                               x.MinorVersion,
                               x.LogicalScheme
                           })
                           .ToList()
                           .Select(x => Tuple.Create(x.Id, x.Identifier,
                                                     string.Join(".", x.MajorVersion.ToString(), x.MinorVersion.ToString()), x.LogicalScheme))
                           .ToList();
        }

        public WorkflowSchemeModel[] GetList()
        {
            return workFlowSchemeRepository.Query()
                        .WorkflowSchemeSelect()
                        .ToArray();
        }
        public WorkflowSchemeModel[] GetList(int skip, int take, string searchString, int? classID, byte? status)
        {
            var query = workFlowSchemeRepository.Query();

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            if (classID.HasValue)
                query = query.Where(x => x.ObjectClassID == classID.Value);
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            var orderedQuery = query.OrderBy(x => x.Name).Skip(skip).Take(take);

            return orderedQuery
                .WorkflowSchemeSelect()
                .ToArray();
        }

        public WorkflowSchemeModel[] GetListByIdentifier(string identifier)
        {
            return workFlowSchemeRepository.Query()
                        .Where(x => x.Identifier == identifier)
                        .WorkflowSchemeSelect()
                        .ToArray();
        }

        public bool ExistsByID(Guid id)
        {
            return workFlowSchemeRepository.Query().Any(x => x.Id == id);
        }

        public bool ExistsByName(Guid id, string name, string identifier, Guid? workflowSchemeFolderID)
        {
            return workFlowSchemeRepository.Query()
                         .Any(x => x.Id != id && x.Name == name && x.Identifier != identifier &&
                                   x.WorkflowSchemeFolderID == workflowSchemeFolderID);
        }

        public bool ExistsByIdentifier(string identifier)
        {
            return workFlowSchemeRepository.Query().Any(x => x.Identifier == identifier);
        }

        public bool InUse(Guid id)
        {
            var exists = workflowTrackingRepository.Query().Any(x => x.WorkflowSchemeID == id) ||
                         workFlowRepository.Query().Any(x => x.WorkflowSchemeID == id) ||
                         callRepository.Query().Any(x => x.WorkflowSchemeID == id) ||
                         workOrderRepository.Query().Any(x => x.WorkflowSchemeID == id) ||
                         problemRepository.Query().Any(x => x.WorkflowSchemeID == id) ||
                         rfcRepository.Query().Any(x => x.WorkflowSchemeID == id) ||
                         massageRepository.Query().Any(x => x.WorkflowSchemeID == id);
            if (!exists)
            {
                var identifier = workFlowSchemeRepository.Query()
                                    .Where(x => x.Id == id)
                                    .Select(x => x.Identifier)
                                    .FirstOrDefault();
                var count = workFlowSchemeRepository.Query().Count(x => x.Identifier == identifier);
                // TODO peterm add code
                // if (count == 1)
                //    exists = 
            }
            return exists;
        }

        public bool Delete(Guid id, byte[] rowVersion)
        {
            var scheme = workFlowSchemeRepository.FirstOrDefault(x => x.Id == id);
            if (scheme == null)
                return false;
            scheme.RowVersion = rowVersion;
            workFlowSchemeRepository.Delete(scheme);
            saveChangesCommand.Save();
            return true;
        }

        public void Insert(WorkflowSchemeModel model)
        {
            workFlowSchemeRepository.Insert(new WorkFlowScheme
            {
                Id = model.ID==Guid.Empty ? Guid.NewGuid() : model.ID,
                Identifier = model.Identifier,
                MajorVersion = (short)model.MajorVersion,
                MinorVersion = (short)model.MinorVersion,
                ModifierID = model.ModifierID,
                Name = model.Name,
                Note = model.Note,
                ObjectClassID = model.ObjectClassID,
                PublisherID = model.PublisherID,
                Status = model.Status,
                TraceIsEnabled = model.TraceIsEnabled,
                UtcDateModified = model.UtcDateModified,
                UtcDatePublished = model.UtcDatePublished,
                WorkflowSchemeFolderID = model.WorkflowSchemeFolderID,
                VisualScheme = model.VisualScheme,
                LogicalScheme = model.LogicalScheme
            });
            saveChangesCommand.Save();
        }
        
        public void Save(WorkflowSchemeModel model)
        {
            var existingScheme = workFlowSchemeRepository.Query().FirstOrDefault(x => x.Id == model.ID) ??
                                 throw new ArgumentNullException("Cant find workflow scheme");

            existingScheme.Name = model.Name;
            existingScheme.Note = model.Note;
            existingScheme.Status = model.Status;
            existingScheme.MinorVersion = (short)model.MinorVersion;
            existingScheme.MajorVersion = (short)model.MajorVersion;
            existingScheme.WorkflowSchemeFolderID = model.WorkflowSchemeFolderID;
            existingScheme.UtcDateModified = model.UtcDateModified;
            existingScheme.ModifierID = model.ModifierID;
            existingScheme.TraceIsEnabled = model.TraceIsEnabled;

            if (!string.IsNullOrEmpty(model.VisualScheme))
            {
                existingScheme.VisualScheme = model.VisualScheme;
            }

            if (!string.IsNullOrEmpty(model.LogicalScheme))
            {
                existingScheme.LogicalScheme = model.LogicalScheme;   
            }

            saveChangesCommand.Save();
        }
    }
}
