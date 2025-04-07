using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Software.Installation
{
    [ObjectClassMapping(ObjectClass.ProductCatalogCategory)]
    internal class ProductCatalogCategorySoftwareInstallationListQuery : ISoftwareInstalationListQuery
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _softwareInstallationDataContext;

        /// <summary>
        /// Инициализирует экземпляр <see cref="ProductCatalogCategorySoftwareInstallationListQuery"/>.
        /// </summary>
        /// <param name="softwareInstallationDataContext">DataContext для работы со инсталляциями</param>
        public ProductCatalogCategorySoftwareInstallationListQuery(CrossPlatformDbContext softwareInstallationDataContext)
        {
            _softwareInstallationDataContext = softwareInstallationDataContext ?? throw new ArgumentNullException(nameof(softwareInstallationDataContext));
        }

        /// <inheritdoc />
        public async Task<IQueryable<ViewSoftwareInstallation>> QueryAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _softwareInstallationDataContext.Set<ViewSoftwareInstallation>();

            var queryList =                      
                        from productCatalogType in _softwareInstallationDataContext.Set<ProductCatalogType>()
                        join productCatalogCategory in _softwareInstallationDataContext.Set<ProductCatalogCategory>() on productCatalogType.ProductCatalogCategoryID equals productCatalogCategory.ID
                        join terminalEquipmentType in _softwareInstallationDataContext.Set<TerminalDeviceModel>() on productCatalogType.IMObjID equals terminalEquipmentType.ProductCatalogTypeID
                        join terminalEquipment in _softwareInstallationDataContext.Set<TerminalDevice>() on terminalEquipmentType.ID equals terminalEquipment.TypeID
                        where productCatalogCategory.ID == filter.TreeSettings.FiltrationObjectID
                        select terminalEquipment.IMObjID;

            var guidList = await queryList.ToListAsync(cancellationToken);          

            return query.Where(item => guidList.Contains(item.DeviceId));
        }
    }
}
