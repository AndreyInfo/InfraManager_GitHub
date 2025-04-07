using InfraManager.DAL;
using InfraManager.DAL.Location;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Location.Floors;

internal class FloorBLL :
    StandardBLL<int, Floor, FloorData, FloorDetails, FloorListFilter>,
    IFloorBLL,
    ISelfRegisteredService<IFloorBLL>
{
    private readonly ILocalizeText _localizeText;
    public FloorBLL(
        IRepository<Floor> repository,
        ILogger<FloorBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<FloorDetails, Floor> detailsBuilder,
        IInsertEntityBLL<Floor, FloorData> insertEntityBLL,
        IModifyEntityBLL<int, Floor, FloorData, FloorDetails> modifyEntityBLL,
        IRemoveEntityBLL<int, Floor> removeEntityBLL,
        IGetEntityBLL<int, Floor, FloorDetails> detailsBLL,
        IGetEntityArrayBLL<int, Floor, FloorDetails, FloorListFilter> detailsArrayBLL,
        ILocalizeText localizeText)
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
        _localizeText = localizeText;
    }


    public async Task RemoveAsync(int id, CancellationToken cancellationToken)
    {
        var hasRooms = await Repository.AnyAsync(c => c.ID == id && c.Rooms.Any(), cancellationToken);
        if (hasRooms)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.FloorHasNestedValues), cancellationToken));

        await DeleteAsync(id, cancellationToken);
    }
}
