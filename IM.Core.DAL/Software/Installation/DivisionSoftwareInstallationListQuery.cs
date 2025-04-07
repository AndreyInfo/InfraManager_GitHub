using InfraManager.DAL.Asset;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.Installation
{
    [ObjectClassMapping(ObjectClass.Division)]
    internal class DivisionSoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _softwareInstallationDataContext;

        /// <summary>
        /// Инициализирует экземпляр <see cref="DivisionSoftwareInstallationListQuery"/>.
        /// </summary>
        /// <param name="softwareInstallationDataContext">DataContext для работы со инсталляциями</param>
        public DivisionSoftwareInstallationListQuery(CrossPlatformDbContext softwareInstallationDataContext)
        {
            _softwareInstallationDataContext = softwareInstallationDataContext ?? throw new ArgumentNullException(nameof(softwareInstallationDataContext));
        }

        /// <inheritdoc />
        public async Task<IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _softwareInstallationDataContext.Set<ViewSoftwareInstallation>();

            IQueryable<QueryBuildingListItem> queryList = null;
            switch (filter.TreeSettings.FiltrationTreeType)
            {
                case UserTreeSettings.FiltrationTreeTypeEnum.OrgStructure:
                    switch (filter.TreeSettings.FiltrationField)
                    {
                        case UserTreeSettings.FiltrationFieldEnum.MOL:
                            queryList = from division in _softwareInstallationDataContext.Set<Subdivision>()
                                        join user in _softwareInstallationDataContext.Set<User>() on division.ID equals user.Subdivision.ID
                                        join asset in _softwareInstallationDataContext.Set<Asset.Asset>() on user.ID equals asset.UserID
                                        join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on asset.DeviceID equals activeDevice.ID
                                           into activeDeviceLeftGroup
                                        from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                        join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on asset.DeviceID equals terminalEquipment.ID
                                            into terminalEquipmentLeftGroup
                                        from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                        where division.ID == filter.TreeSettings.FiltrationObjectID
                                        select new QueryBuildingListItem()
                                        {
                                            ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                            TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID
                                        };
                            break;
                        case UserTreeSettings.FiltrationFieldEnum.Owner:
                            queryList = from division in _softwareInstallationDataContext.Set<Subdivision>()
                                        join user in _softwareInstallationDataContext.Set<User>() on division.ID equals user.Subdivision.ID
                                        join asset in _softwareInstallationDataContext.Set<Asset.Asset>() on user.IMObjID equals asset.OwnerID
                                        join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on asset.DeviceID equals activeDevice.ID
                                           into activeDeviceLeftGroup
                                        from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                        join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on asset.DeviceID equals terminalEquipment.ID
                                            into terminalEquipmentLeftGroup
                                        from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                        where division.ID == filter.TreeSettings.FiltrationObjectID
                                        select new QueryBuildingListItem()
                                        {
                                            ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                            TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID
                                        };
                            break;
                        case UserTreeSettings.FiltrationFieldEnum.Utilizer:
                            queryList = from division in _softwareInstallationDataContext.Set<Subdivision>()
                                        join user in _softwareInstallationDataContext.Set<User>() on division.ID equals user.Subdivision.ID
                                        join asset in _softwareInstallationDataContext.Set<Asset.Asset>() on user.IMObjID equals asset.UtilizerID
                                        join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on asset.DeviceID equals activeDevice.ID
                                           into activeDeviceLeftGroup
                                        from activeDeviceLeft in activeDeviceLeftGroup.DefaultIfEmpty()
                                        join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on asset.DeviceID equals terminalEquipment.ID
                                            into terminalEquipmentLeftGroup
                                        from terminalEquipmentLeft in terminalEquipmentLeftGroup.DefaultIfEmpty()
                                        where division.ID == filter.TreeSettings.FiltrationObjectID
                                        select new QueryBuildingListItem()
                                        {
                                            ActiveDeviceImobjId = activeDeviceLeft.IMObjID,
                                            TerminalEquipment1ImobjId = terminalEquipmentLeft.IMObjID
                                        };
                            break;
                    }
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
