using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.Installation
{
    [ObjectClassMapping(ObjectClass.Building)]
    internal class BuildingSoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        private readonly CrossPlatformDbContext _db;

        public BuildingSoftwareInstallationListQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<System.Linq.IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken token = default)
        {
            var query = _db.Set<ViewSoftwareInstallation>();

            var queryList =
                      from building in _db.Set<Building>()
                      join floor in _db.Set<Floor>() on building.ID equals floor.Building.ID
                          into floorLeftGroup
                      from floorLeft in floorLeftGroup.DefaultIfEmpty()
                      join room in _db.Set<Room>() on floorLeft.ID equals room.Floor.ID
                        into roomLeftGroup
                      from roomLeft in roomLeftGroup.DefaultIfEmpty()
                      join rack in _db.Set<Rack>() on roomLeft.ID equals rack.Room.ID
                         into rackLeftGroup
                      from rackLeft in rackLeftGroup.DefaultIfEmpty()
                      join workplace in _db.Set<Workplace>() on roomLeft.ID equals workplace.Room.ID
                       into workplaceLeftGroup
                      from workplaceLeft in workplaceLeftGroup.DefaultIfEmpty()
                      join activeDevice in _db.Set<NetworkDevice>() on rackLeft.ID equals activeDevice.RackID
                        into activeDeviceLeftGroup
                      from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                      join terminalEquipment1 in _db.Set<TerminalDevice>() on roomLeft.ID equals terminalEquipment1.RoomID
                       into terminalEquipment1LeftGroup
                      from terminalEquipment1Left in terminalEquipment1LeftGroup.DefaultIfEmpty()
                      join terminalEquipment2 in _db.Set<TerminalDevice>() on workplaceLeft.ID equals terminalEquipment2.Workplace.ID
                       into terminalEquipment2LeftGroup
                      from terminalEquipment2Left in terminalEquipment2LeftGroup.DefaultIfEmpty()
                      where building.IMObjID == filter.TreeSettings.FiltrationObjectID
                      select new QueryBuildingListItem
                      {
                          ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                          TerminalEquipment1ImobjId = terminalEquipment1Left.IMObjID,
                          TerminalEquipment2ImobjId = terminalEquipment2Left.IMObjID
                      };

            var list = await queryList.ToListAsync(token);
            var guidList = list.QueryBuildingListItemToListGuid();

            return query.Where(item => guidList.Contains(item.DeviceId));
        }
    }
}
