using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallTypeBLL : StandardBLL<Guid, CallType, CallTypeData, CallTypeDetails, CallTypeListFilter>,
        ICallTypeBLL,
        ISelfRegisteredService<ICallTypeBLL>
    {
        public CallTypeBLL(
            IRepository<CallType> repository,
            ILogger<CallTypeBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<CallTypeDetails, CallType> detailsBuilder,
            IInsertEntityBLL<CallType, CallTypeData> insertEntityBLL,
            IModifyEntityBLL<Guid, CallType, CallTypeData, CallTypeDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, CallType> removeEntityBLL,
            IGetEntityBLL<Guid, CallType, CallTypeDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, CallType, CallTypeDetails, CallTypeListFilter> detailsArrayBLL) 
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

        public Task<CallTypeDetails[]> GetDetailsPageAsync(CallTypeListFilter filterBy, CancellationToken cancellationToken = default)
        {
            return GetDetailsPageAsync(filterBy, filterBy, cancellationToken);
        }

        public async Task<byte[]> GetImageBytesAsync(Guid callTypeID, CancellationToken cancellationToken = default)
        {
            var entity = await Repository.FirstOrDefaultAsync(x => x.ID == callTypeID, cancellationToken);
            return entity?.Icon ?? Array.Empty<byte>();
        }
    }
}
