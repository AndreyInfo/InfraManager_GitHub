using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings
{
    internal class ProductCatalogImportSettingTypesBLL :
        BaseEntityBLL<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes,
            ProductCatalogImportSettingTypesDetails, ProductCatalogImportSettingTypesOutputDetails,
            ProductCatalogImportSettingTypesFilter>,
        IProductCatalogImportSettingTypesBLL, ISelfRegisteredService<IProductCatalogImportSettingTypesBLL>
    {
        public ProductCatalogImportSettingTypesBLL(IRepository<ProductCatalogImportSettingTypes> entities,
            IMapper mapper,
            IFilterEntity<ProductCatalogImportSettingTypes, ProductCatalogImportSettingTypesFilter> filterEntity,
            IFinderQuery<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<ProductCatalogImportSettingTypes, ProductCatalogImportSettingTypesOutputDetails> outputBuilder,
            IUpdateQuery<ProductCatalogImportSettingTypesDetails, ProductCatalogImportSettingTypes> updateQuery,
            IInsertQuery<ProductCatalogImportSettingTypesDetails, ProductCatalogImportSettingTypes> insertQuery,
            IRemoveQuery<ProductCatalogImportSettingsType, ProductCatalogImportSettingTypes> removeQuery) : base(
            entities,
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