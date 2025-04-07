using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.Location;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Location.Buildings;

internal class BuildingBLL :
    StandardBLL<int, Building, BuildingData, BuildingDetails, BuildingListFilter>,
    IBuildingBLL,
    ISelfRegisteredService<IBuildingBLL>
{
    private readonly ILocalizeText _localizeText;
    public BuildingBLL(
        IRepository<Building> repository,
        ILogger<BuildingBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<BuildingDetails, Building> detailsBuilder,
        IInsertEntityBLL<Building, BuildingData> insertEntityBLL,
        IModifyEntityBLL<int, Building, BuildingData, BuildingDetails> modifyEntityBLL,
        IRemoveEntityBLL<int, Building> removeEntityBLL,
        IGetEntityBLL<int, Building, BuildingDetails> detailsBLL,
        IGetEntityArrayBLL<int, Building, BuildingDetails, BuildingListFilter> detailsArrayBLL,
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
        var hasFloors = await Repository.AnyAsync(c => c.ID == id && c.Floors.Any(), cancellationToken);
        if (hasFloors)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.BuildingHasNestedValues), cancellationToken));

        await DeleteAsync(id, cancellationToken);
    }

}