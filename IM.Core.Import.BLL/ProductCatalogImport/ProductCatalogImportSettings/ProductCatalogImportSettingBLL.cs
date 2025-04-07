using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogImportSettings
{
    internal class ProductCatalogImportSettingBLL : BaseEntityBLL<Guid, ProductCatalogImportSetting, ProductCatalogImportSettingDetails,
            ProductCatalogImportSettingOutputDetails, ProductCatalogImportSettingFilter>,
        IProductCatalogImportSettingBLL, ISelfRegisteredService<IProductCatalogImportSettingBLL>
    {
        public ProductCatalogImportSettingBLL(IRepository<ProductCatalogImportSetting> entities,
            IMapper mapper,
            IFilterEntity<ProductCatalogImportSetting, ProductCatalogImportSettingFilter> filterEntity,
            IFinderQuery<Guid, ProductCatalogImportSetting> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<ProductCatalogImportSetting, ProductCatalogImportSettingOutputDetails> outputBuilder,
            IUpdateQuery<ProductCatalogImportSettingDetails, ProductCatalogImportSetting> updateQuery,
            IInsertQuery<ProductCatalogImportSettingDetails, ProductCatalogImportSetting> insertQuery,
            IRemoveQuery<Guid, ProductCatalogImportSetting> removeQuery) : base(entities,
            mapper,
            filterEntity,
            finder,
            unitOfWork,
            outputBuilder,
            updateQuery,
            insertQuery,
            removeQuery)
        {
        }
    }
}