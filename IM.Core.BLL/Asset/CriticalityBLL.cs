using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;
using System;

namespace InfraManager.BLL.Asset
{
    internal class CriticalityBLL : StandardBLL<Guid, Criticality, LookupData, LookupDetails<Guid>, LookupListFilter>,
        ICriticalityBLL,
        ISelfRegisteredService<ICriticalityBLL>
    {
        public CriticalityBLL(
            IRepository<Criticality> repository,
            ILogger<CriticalityBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<LookupDetails<Guid>, Criticality> detailsBuilder,
            IInsertEntityBLL<Criticality, LookupData> insertEntityBLL,
            IModifyEntityBLL<Guid, Criticality, LookupData, LookupDetails<Guid>> modifyEntityBLL,
            IRemoveEntityBLL<Guid, Criticality> removeEntityBLL,
            IGetEntityBLL<Guid, Criticality, LookupDetails<Guid>> detailsBLL,
            IGetEntityArrayBLL<Guid, Criticality, LookupDetails<Guid>, LookupListFilter> detailsArrayBLL) 
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
