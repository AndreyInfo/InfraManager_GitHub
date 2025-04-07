using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentCauseBLL :
        StandardBLL<int, MassIncidentCause, LookupData, MassIncidentCauseDetails, MassIncidentCauseListFilter>,
        IMassIncidentCauseBLL,
        ISelfRegisteredService<IMassIncidentCauseBLL>
    {
        public MassIncidentCauseBLL(
            IRepository<MassIncidentCause> repository,
            ILogger<MassIncidentCauseBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<MassIncidentCauseDetails, MassIncidentCause> detailsBuilder,
            IInsertEntityBLL<MassIncidentCause, LookupData> insertEntityBLL,
            IModifyEntityBLL<int, MassIncidentCause, LookupData, MassIncidentCauseDetails> modifyEntityBLL,
            IRemoveEntityBLL<int, MassIncidentCause> removeEntityBLL,
            IGetEntityBLL<int, MassIncidentCause, MassIncidentCauseDetails> detailsBLL,
            IGetEntityArrayBLL<int, MassIncidentCause, MassIncidentCauseDetails, MassIncidentCauseListFilter> detailsArrayBLL)
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

        public Task<MassIncidentCauseDetails[]> GetDetailsPageAsync(MassIncidentCauseListFilter filterBy, CancellationToken cancellationToken = default)
        {
            return StandardBLLExtensions.GetDetailsPageAsync(this, filterBy, cancellationToken);
        }
    }
}
