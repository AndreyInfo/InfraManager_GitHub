using IM.Core.WF.BLL.Interfaces;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.WF;
using System;
using System.Linq;

namespace IM.Core.WF.BLL
{
    internal class WorkflowRequestBLL : IWorkflowRequestBLL, ISelfRegisteredService<IWorkflowRequestBLL>
    {
        private readonly IUnitOfWork saveChangesCommand;
        private readonly IRepository<WorkflowRequest> workflowRequestRepository;

        public WorkflowRequestBLL(
                    IUnitOfWork saveChangesCommand,
                    IRepository<WorkflowRequest> workflowRequestRepository)
        {
            this.saveChangesCommand = saveChangesCommand;
            this.workflowRequestRepository = workflowRequestRepository;
        }

        public void Delete(Guid? id)
        {
            var requests = workflowRequestRepository.Query()
                                .Where(x => x.Id == id || (id == null))
                                .ToList();
            requests.ForEach(x => workflowRequestRepository.Delete(x));
            saveChangesCommand.Save();
        }

        public void Insert(Guid id)
        {
            var wfRequest = new WorkflowRequest()
            {
                Id = id
            };
            workflowRequestRepository.Insert(wfRequest);
            saveChangesCommand.Save();
        }

        public bool Exists(Guid id)
        {
            return workflowRequestRepository.Query().Any(x => x.Id == id);
        }
    }
}
