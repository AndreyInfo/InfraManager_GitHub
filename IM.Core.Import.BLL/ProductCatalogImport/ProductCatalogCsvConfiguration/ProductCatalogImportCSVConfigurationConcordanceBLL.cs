using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ModelSettings;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings
{
    internal class ProductCatalogImportCSVConfigurationConcordanceBLL :
        BaseEntityBLL<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance,
            ProductCatalogImportCSVConfigurationConcordanceDetails,
            ProductCatalogImportCSVConfigurationConcordanceOutputDetails,
            ProductCatalogImportCSVConfigurationConcordanceFilter>,
        IProductCatalogImportCSVConfigurationConcordanceBLL,
        ISelfRegisteredService<IProductCatalogImportCSVConfigurationConcordanceBLL>
    {
        public ProductCatalogImportCSVConfigurationConcordanceBLL(
            IRepository<ProductCatalogImportCSVConfigurationConcordance> entities,
            IMapper mapper,
            IFilterEntity<ProductCatalogImportCSVConfigurationConcordance,
                ProductCatalogImportCSVConfigurationConcordanceFilter> filterEntity,
            IFinderQuery<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<ProductCatalogImportCSVConfigurationConcordance,
                ProductCatalogImportCSVConfigurationConcordanceOutputDetails> outputBuilder,
            IUpdateQuery<ProductCatalogImportCSVConfigurationConcordanceDetails,
                ProductCatalogImportCSVConfigurationConcordance> updateQuery,
            IInsertQuery<ProductCatalogImportCSVConfigurationConcordanceDetails,
                ProductCatalogImportCSVConfigurationConcordance> insertQuery,
            IRemoveQuery<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance>
                removeQuery) : base(entities,
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