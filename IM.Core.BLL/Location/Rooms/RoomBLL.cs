using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Location;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Location.Rooms;

internal class RoomBLL :
    StandardBLL<int, Room, RoomData, RoomDetails, RoomListFilter>,
    IRoomBLL,
    ISelfRegisteredService<IRoomBLL>
{
    private readonly ILocalizeText _localizeText;
    public RoomBLL(
        IRepository<Room> repository,
        ILogger<RoomBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<RoomDetails, Room> detailsBuilder,
        IInsertEntityBLL<Room, RoomData> insertEntityBLL,
        IModifyEntityBLL<int, Room, RoomData, RoomDetails> modifyEntityBLL,
        IRemoveEntityBLL<int, Room> removeEntityBLL,
        IGetEntityBLL<int, Room, RoomDetails> detailsBLL,
        IGetEntityArrayBLL<int, Room, RoomDetails, RoomListFilter> detailsArrayBLL,
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
        var hasWorkplaces = await Repository.AnyAsync(c => c.ID == id && c.Workplaces.Any(), cancellationToken);
        if (hasWorkplaces)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.RoomHasNestedValues), cancellationToken));

        await DeleteAsync(id, cancellationToken);
    }
}