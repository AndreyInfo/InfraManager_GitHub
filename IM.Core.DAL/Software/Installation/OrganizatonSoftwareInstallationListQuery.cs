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
    [ObjectClassMapping(ObjectClass.Organizaton)]
    internal class OrganizatonSoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _softwareInstallationDataContext;

        /// <summary>
        /// Инициализирует экземпляр <see cref="OrganizatonSoftwareInstallationListQuery"/>.
        /// </summary>
        /// <param name="softwareInstallationDataContext">DataContext для работы со инсталляциями</param>
        public OrganizatonSoftwareInstallationListQuery(CrossPlatformDbContext softwareInstallationDataContext)
        {
            _softwareInstallationDataContext = softwareInstallationDataContext ?? throw new ArgumentNullException(nameof(softwareInstallationDataContext));
        }

        /// <inheritdoc />
        public async Task<IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _softwareInstallationDataContext.Set<ViewSoftwareInstallation>();

            IQueryable<QueryBuildingListItem> queryList = null;
            switch(filter.TreeSettings.FiltrationTreeType)
            {
                case UserTreeSettings.FiltrationTreeTypeEnum.OrgStructure:
                    switch (filter.TreeSettings.FiltrationField)
                    {
                        case UserTreeSettings.FiltrationFieldEnum.MOL:
                            queryList = from organization in _softwareInstallationDataContext.Set<Organization>()
                                        join division in _softwareInstallationDataContext.Set<Subdivision>() on organization.ID equals division.Organization.ID
                                        join user in _softwareInstallationDataContext.Set<User>() on division.ID equals user.Subdivision.ID
                                        join assetOrg in _softwareInstallationDataContext.Set<Asset.Asset>() on user.ID equals assetOrg.UserID
                                        join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on assetOrg.DeviceID equals activeDevice.ID
                                           into activeDeviceLeftGroup
                                        from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                        join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on assetOrg.DeviceID equals terminalEquipment.ID
                                            into terminalEquipmentLeftGroup
                                        from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                        where organization.ID == filter.TreeSettings.FiltrationObjectID
                                        select new QueryBuildingListItem()
                                        {
                                            ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                            TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID
                                        };
                            break;
                        case UserTreeSettings.FiltrationFieldEnum.Owner:
                            queryList = from organization in _softwareInstallationDataContext.Set<Organization>()
                                        join asset in _softwareInstallationDataContext.Set<Asset.Asset>() on organization.ID equals asset.OwnerID
                                        join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on asset.DeviceID equals terminalEquipment.ID
                                        where organization.ID == filter.TreeSettings.FiltrationObjectID && asset.OwnerClassID == filter.TreeSettings.FiltrationObjectClassID
                                        select new QueryBuildingListItem()
                                        {
                                            ActiveDeviceImobjId = terminalEquipment.IMObjID
                                        };

                            break;
                        case UserTreeSettings.FiltrationFieldEnum.Utilizer:
                            var org = from organization in _softwareInstallationDataContext.Set<Organization>()
                                      join assetOrg in _softwareInstallationDataContext.Set<Asset.Asset>() on organization.ID equals assetOrg.UtilizerID
                                      join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on assetOrg.DeviceID equals activeDevice.ID
                                         into activeDeviceLeftGroup
                                      from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                      join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on assetOrg.DeviceID equals terminalEquipment.ID
                                          into terminalEquipmentLeftGroup
                                      from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                      where organization.ID == filter.TreeSettings.FiltrationObjectID
                                      select new QueryBuildingListItem()
                                      {
                                          ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                          TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID,
                                      };
                            var subdiv = from organization in _softwareInstallationDataContext.Set<Organization>()
                                      join division in _softwareInstallationDataContext.Set<Subdivision>() on organization.ID equals division.Organization.ID
                                      join assetOrg in _softwareInstallationDataContext.Set<Asset.Asset>() on division.ID equals assetOrg.UtilizerID
                                      join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on assetOrg.DeviceID equals activeDevice.ID
                                         into activeDeviceLeftGroup
                                      from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                      join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on assetOrg.DeviceID equals terminalEquipment.ID
                                          into terminalEquipmentLeftGroup
                                      from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                      where organization.ID == filter.TreeSettings.FiltrationObjectID
                                      select new QueryBuildingListItem()
                                      {
                                          ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                          TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID,
                                      };


                            queryList = (from organization in _softwareInstallationDataContext.Set<Organization>()
                                             join division in _softwareInstallationDataContext.Set<Subdivision>() on organization.ID equals division.Organization.ID
                                             join user in _softwareInstallationDataContext.Set<User>() on division.ID equals user.Subdivision.ID
                                        join assetOrg in _softwareInstallationDataContext.Set<Asset.Asset>() on user.IMObjID equals assetOrg.UtilizerID
                                         join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on assetOrg.DeviceID equals activeDevice.ID
                                            into activeDeviceLeftGroup
                                         from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                         join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on assetOrg.DeviceID equals terminalEquipment.ID
                                             into terminalEquipmentLeftGroup
                                         from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                         where organization.ID == filter.TreeSettings.FiltrationObjectID
                                        select new QueryBuildingListItem()
                                        {
                                            ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                            TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID
                                        })
                                        .Union(org.Union(subdiv))
                                        ;
                            break;
                        default:
                            queryList = Enumerable.Empty<QueryBuildingListItem>().AsQueryable();
                            break;
                    }
                    break;
                case UserTreeSettings.FiltrationTreeTypeEnum.Location:
                    queryList =
                     from organization in _softwareInstallationDataContext.Set<Organization>()
                     join building in _softwareInstallationDataContext.Set<Building>() on organization.ID equals building.OrganizationID
                     join floor in _softwareInstallationDataContext.Set<Floor>() on building.ID equals floor.Building.ID
                         into floorLeftGroup
                     from floorLeft in floorLeftGroup.DefaultIfEmpty()
                     join room in _softwareInstallationDataContext.Set<Room>() on floorLeft.ID equals room.Floor.ID
                       into roomLeftGroup
                     from roomLeft in roomLeftGroup.DefaultIfEmpty()
                     join rack in _softwareInstallationDataContext.Set<Rack>() on roomLeft.ID equals rack.Room.ID
                        into rackLeftGroup
                     from rackLeft in rackLeftGroup.DefaultIfEmpty()
                     join workplace in _softwareInstallationDataContext.Set<Workplace>() on roomLeft.ID equals workplace.Room.ID
                      into workplaceLeftGroup
                     from workplaceLeft in workplaceLeftGroup.DefaultIfEmpty()
                     join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on rackLeft.ID equals activeDevice.RackID
                       into activeDeviceLeftGroup
                     from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                     join terminalEquipment1 in _softwareInstallationDataContext.Set<TerminalDevice>() on roomLeft.ID equals terminalEquipment1.RoomID
                      into terminalEquipment1LeftGroup
                     from terminalEquipment1Left in terminalEquipment1LeftGroup.DefaultIfEmpty()
                     join terminalEquipment2 in _softwareInstallationDataContext.Set<TerminalDevice>() on workplaceLeft.ID equals terminalEquipment2.Workplace.ID
                      into terminalEquipment2LeftGroup
                     from terminalEquipment2Left in terminalEquipment2LeftGroup.DefaultIfEmpty()
                     where organization.ID == filter.TreeSettings.FiltrationObjectID
                     select new QueryBuildingListItem()
                     {
                         ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                         TerminalEquipment1ImobjId = terminalEquipment1Left.IMObjID,
                         TerminalEquipment2ImobjId = terminalEquipment2Left.IMObjID
                     };
                    break;
                default:
                    queryList = Enumerable.Empty<QueryBuildingListItem>().AsQueryable();
                    break;
            }

            var list = await queryList.ToListAsync(cancellationToken);

            var guidList = list.QueryBuildingListItemToListGuid();

            return query.Where(item => guidList.Contains(item.DeviceId));
        }
    }
}
