using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.Installation
{
    [ObjectClassMapping(ObjectClass.Workplace)]
    internal class WorkplaceSoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _softwareInstallationDataContext;

        /// <summary>
        /// Инициализирует экземпляр <see cref="WorkplaceSoftwareInstallationListQuery"/>.
        /// </summary>
        /// <param name="softwareInstallationDataContext">DataContext для работы со инсталляциями</param>
        public WorkplaceSoftwareInstallationListQuery(CrossPlatformDbContext softwareInstallationDataContext)
        {
            _softwareInstallationDataContext = softwareInstallationDataContext ?? throw new ArgumentNullException(nameof(softwareInstallationDataContext));
        }

        /// <inheritdoc />
        public async Task<IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _softwareInstallationDataContext.Set<ViewSoftwareInstallation>();

            var queryList =
                      from workplace in _softwareInstallationDataContext.Set<Workplace>()
                      join terminalEquipment2 in _softwareInstallationDataContext.Set<TerminalDevice>() on workplace.ID equals terminalEquipment2.Workplace.ID
                      where workplace.IMObjID == filter.TreeSettings.FiltrationObjectID
                      select new QueryBuildingListItem()
                      {
                          ActiveDeviceImobjId = null,
                          TerminalEquipment1ImobjId = null,
                          TerminalEquipment2ImobjId = terminalEquipment2.IMObjID
                      };

            var list = await queryList.ToListAsync(cancellationToken);

            var guidList = list.QueryBuildingListItemToListGuid();

            return query.Where(item => guidList.Contains(item.DeviceId));
        }
    }
}
