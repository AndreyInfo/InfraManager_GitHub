using System;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogImportSettings
{
    internal class ProductCatalogImportSettingBLL :
        StandardBLL<Guid, ProductCatalogImportSetting, ProductCatalogImportSettingDetails,
            ProductCatalogImportSettingOutputDetails, ProductCatalogImportSettingFilter>,
        IProductCatalogImportSettingBLL, ISelfRegisteredService<IProductCatalogImportSettingBLL>
    {
        public ProductCatalogImportSettingBLL(IRepository<ProductCatalogImportSetting> repository,
            ILogger<ProductCatalogImportSettingBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ProductCatalogImportSettingOutputDetails, ProductCatalogImportSetting> detailsBuilder,
            IInsertEntityBLL<ProductCatalogImportSetting, ProductCatalogImportSettingDetails> insertEntityBLL,
            IModifyEntityBLL<Guid, ProductCatalogImportSetting, ProductCatalogImportSettingDetails,
                    ProductCatalogImportSettingOutputDetails>
                modifyEntityBLL,
            IRemoveEntityBLL<Guid, ProductCatalogImportSetting> removeEntityBLL,
            IGetEntityBLL<Guid, ProductCatalogImportSetting, ProductCatalogImportSettingOutputDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, ProductCatalogImportSetting, ProductCatalogImportSettingOutputDetails,
                    ProductCatalogImportSettingFilter>
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
    }
}