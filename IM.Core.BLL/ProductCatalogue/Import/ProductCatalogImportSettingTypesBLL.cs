using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.Import
{
    internal class ProductCatalogImportSettingTypesBLL :
        StandardBLL<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes,
            ProductCatalogImportSettingTypesDetails, ProductCatalogImportSettingTypesOutputDetails,
            ProductCatalogImportSettingTypesFilter>,
        IProductCatalogImportSettingTypesBLL, ISelfRegisteredService<IProductCatalogImportSettingTypesBLL>
    {
        public ProductCatalogImportSettingTypesBLL(IRepository<ProductCatalogImportSettingTypes> repository,
            ILogger<ProductCatalogImportSettingTypesBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ProductCatalogImportSettingTypesOutputDetails, ProductCatalogImportSettingTypes>
                detailsBuilder,
            IInsertEntityBLL<ProductCatalogImportSettingTypes, ProductCatalogImportSettingTypesDetails> insertEntityBLL,
            IModifyEntityBLL<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes,
                    ProductCatalogImportSettingTypesDetails, ProductCatalogImportSettingTypesOutputDetails>
                modifyEntityBLL,
            IRemoveEntityBLL<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes> removeEntityBLL,
            IGetEntityBLL<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes,
                ProductCatalogImportSettingTypesOutputDetails> detailsBLL,
            IGetEntityArrayBLL<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes,
                    ProductCatalogImportSettingTypesOutputDetails, ProductCatalogImportSettingTypesFilter>
                detailsArrayBLL) : base(repository,
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
        }

        public Task<ProductCatalogImportSettingTypesOutputDetails> AddAsync(ProductCatalogImportSettingsType data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}