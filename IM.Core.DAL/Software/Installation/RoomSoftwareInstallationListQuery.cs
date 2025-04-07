using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.Installation
{
    [ObjectClassMapping(ObjectClass.Room)]
    internal class RoomSoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _softwareInstallationDataContext;

        /// <summary>
        /// Инициализирует экземпляр <see cref="RoomSoftwareInstallationListQuery"/>.
        /// </summary>
        /// <param name="softwareInstallationDataContext">DataContext для работы со инсталляциями</param>
        public RoomSoftwareInstallationListQuery(CrossPlatformDbContext softwareInstallationDataContext)
        {
            _softwareInstallationDataContext = softwareInstallationDataContext ?? throw new ArgumentNullException(nameof(softwareInstallationDataContext));
        }

        /// <inheritdoc />
        public async Task<IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _softwareInstallationDataContext.Set<ViewSoftwareInstallation>();

            var queryList =
                     from room in _softwareInstallationDataContext.Set<Room>()
                     join rack in _softwareInstallationDataContext.Set<Rack>() on room.ID equals rack.Room.ID
                        into rackLeftGroup
                     from rackLeft in rackLeftGroup.DefaultIfEmpty()
                     join workplace in _softwareInstallationDataContext.Set<Workplace>() on room.ID equals workplace.Room.ID
                      into workplaceLeftGroup
                     from workplaceLeft in workplaceLeftGroup.DefaultIfEmpty()
                     join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on rackLeft.ID equals activeDevice.RackID
                       into activeDeviceLeftGroup
                     from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                     join terminalEquipment1 in _softwareInstallationDataContext.Set<TerminalDevice>() on room.ID equals terminalEquipment1.RoomID
                      into terminalEquipment1LeftGroup
                     from terminalEquipment1Left in terminalEquipment1LeftGroup.DefaultIfEmpty()
                     join terminalEquipment2 in _softwareInstallationDataContext.Set<TerminalDevice>() on workplaceLeft.ID equals terminalEquipment2.Workplace.ID
                      into terminalEquipment2LeftGroup
                     from terminalEquipment2Left in terminalEquipment2LeftGroup.DefaultIfEmpty()
                     where room.IMObjID == filter.TreeSettings.FiltrationObjectID
                     select new QueryBuildingListItem
                     {
                         ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                         TerminalEquipment1ImobjId = terminalEquipment1Left.IMObjID,
                         TerminalEquipment2ImobjId = terminalEquipment2Left.IMObjID
                     };

            var list = await queryList.ToListAsync(cancellationToken);

            var guidList = list.QueryBuildingListItemToListGuid();

            return query.Where(item => guidList.Contains(item.DeviceId));
        }
    }
}
