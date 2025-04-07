using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.Installation
{
    [ObjectClassMapping(ObjectClass.Rack)]
    internal class RackSoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _softwareInstallationDataContext;

        /// <summary>
        /// Инициализирует экземпляр <see cref="RoomSoftwareInstallationListQuery"/>.
        /// </summary>
        /// <param name="softwareInstallationDataContext">DataContext для работы со инсталляциями</param>
        public RackSoftwareInstallationListQuery(CrossPlatformDbContext softwareInstallationDataContext)
        {
            _softwareInstallationDataContext = softwareInstallationDataContext ?? throw new ArgumentNullException(nameof(softwareInstallationDataContext));
        }

        /// <inheritdoc />
        public async Task<IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _softwareInstallationDataContext.Set<ViewSoftwareInstallation>();

            var queryList =
                     from rack in _softwareInstallationDataContext.Set<Rack>()
                     join activeDevice in _softwareInstallationDataContext.Set<NetworkDevice>() on rack.ID equals activeDevice.RackID
                     where rack.IMObjID == filter.TreeSettings.FiltrationObjectID
                     select activeDevice.IMObjID;

            var list = await queryList.ToArrayAsync(cancellationToken);

            return query.Where(item => list.Contains(item.DeviceId));
        }
    }
}
