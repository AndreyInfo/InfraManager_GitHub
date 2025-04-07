using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogCsvConfiguration;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;

namespace IM.Core.Import.BLL.Import.SettingsBLL.ImportSettings.ProductCatalogCsvConfiguration
{
    internal class ProductCatalogImportCSVConfigurationBLL : BaseEntityBLL<Guid, ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationDetails,
            ProductCatalogImportCSVConfigurationOutputDetails, ProductCatalogImportCSVConfigurationFilter>,
        IProductCatalogImportCSVConfigurationBLL, ISelfRegisteredService<IProductCatalogImportCSVConfigurationBLL>
    {
        public ProductCatalogImportCSVConfigurationBLL(IRepository<ProductCatalogImportCSVConfiguration> entities,
            IMapper mapper,
            IFilterEntity<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationFilter>
                filterEntity,
            IFinderQuery<Guid, ProductCatalogImportCSVConfiguration> finder,
            IUnitOfWork unitOfWork,
            IBuildModel<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationOutputDetails>
                outputBuilder,
            IUpdateQuery<ProductCatalogImportCSVConfigurationDetails, ProductCatalogImportCSVConfiguration> updateQuery,
            IInsertQuery<ProductCatalogImportCSVConfigurationDetails, ProductCatalogImportCSVConfiguration> insertQuery,
            IRemoveQuery<Guid, ProductCatalogImportCSVConfiguration> removeQuery) : base(entities,
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