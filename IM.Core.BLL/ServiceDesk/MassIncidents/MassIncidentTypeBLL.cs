using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentTypeBLL :
        StandardBLL<int, MassIncidentType, MassIncidentTypeData, MassIncidentTypeDetails, MassIncidentTypeListFilter>,
        IMassIncidentTypeBLL,
        ISelfRegisteredService<IMassIncidentTypeBLL>
    {
        public MassIncidentTypeBLL(
            IRepository<MassIncidentType> repository,
            ILogger<MassIncidentTypeBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<MassIncidentTypeDetails, MassIncidentType> detailsBuilder,
            IInsertEntityBLL<MassIncidentType, MassIncidentTypeData> insertEntityBLL,
            IModifyEntityBLL<int, MassIncidentType, MassIncidentTypeData, MassIncidentTypeDetails> modifyEntityBLL,
            IRemoveEntityBLL<int, MassIncidentType> removeEntityBLL,
            IGetEntityBLL<int, MassIncidentType, MassIncidentTypeDetails> detailsBLL,
            IGetEntityArrayBLL<int, MassIncidentType, MassIncidentTypeDetails, MassIncidentTypeListFilter> detailsArrayBLL) 
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

        public Task<MassIncidentTypeDetails[]> GetDetailsPageAsync(MassIncidentTypeListFilter filterBy, CancellationToken cancellationToken = default)
        {
            return StandardBLLExtensions.GetDetailsPageAsync(this, filterBy, cancellationToken);
        }
    }
}
