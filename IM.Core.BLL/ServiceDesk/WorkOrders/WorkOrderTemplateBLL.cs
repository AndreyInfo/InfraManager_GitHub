using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.WorkOrderTemplates;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    internal class WorkOrderTemplateBLL :
        StandardBLL<Guid, WorkOrderTemplate, WorkOrderTemplateData, WorkOrderTemplateDetails, WorkOrderTemplateLookupListFilter>,
        IWorkOrderTemplateBLL,
        ISelfRegisteredService<IWorkOrderTemplateBLL>
    {
        public WorkOrderTemplateBLL(
            IRepository<WorkOrderTemplate> repository, 
            ILogger<WorkOrderTemplateBLL> logger, 
            IUnitOfWork unitOfWork, 
            ICurrentUser currentUser,
            IBuildObject<WorkOrderTemplateDetails, WorkOrderTemplate> detailsBuilder,
            IInsertEntityBLL<WorkOrderTemplate, WorkOrderTemplateData> insertEntityBLL, 
            IModifyEntityBLL<Guid, WorkOrderTemplate, WorkOrderTemplateData, WorkOrderTemplateDetails> modifyEntityBLL, 
            IRemoveEntityBLL<Guid, WorkOrderTemplate> removeEntityBLL, 
            IGetEntityBLL<Guid, WorkOrderTemplate, WorkOrderTemplateDetails> detailsBLL, 
            IGetEntityArrayBLL<Guid, WorkOrderTemplate, WorkOrderTemplateDetails, WorkOrderTemplateLookupListFilter> detailsArrayBLL) 
            : base(
                  repository,
                  logger,
                  unitOfWork,
                  currentUser,
                  detailsBuilder,
                  insertEntityBLL,
                  modifyEntityBLL,
                  removeEntityBLL,
                  detailsBLL,
                  detailsArrayBLL)
        {
        }
    }
}
