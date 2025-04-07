using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Location;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.Location.Workplaces;

internal class WorkplaceBLL :
    StandardBLL<int, Workplace, WorkplaceData, WorkplaceDetails, WorkplaceListFilter>,
    IWorkplaceBLL,
    ISelfRegisteredService<IWorkplaceBLL>
{
    public WorkplaceBLL(
        IRepository<Workplace> repository,
        ILogger<WorkplaceBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<WorkplaceDetails, Workplace> detailsBuilder,
        IInsertEntityBLL<Workplace, WorkplaceData> insertEntityBLL,
        IModifyEntityBLL<int, Workplace, WorkplaceData, WorkplaceDetails> modifyEntityBLL,
        IRemoveEntityBLL<int, Workplace> removeEntityBLL,
        IGetEntityBLL<int, Workplace, WorkplaceDetails> detailsBLL,
        IGetEntityArrayBLL<int, Workplace, WorkplaceDetails, WorkplaceListFilter> detailsArrayBLL)
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
    { }
}