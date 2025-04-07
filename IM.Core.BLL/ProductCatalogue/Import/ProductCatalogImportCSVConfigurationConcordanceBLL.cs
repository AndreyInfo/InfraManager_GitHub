using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.Import
{
    internal class ProductCatalogImportCSVConfigurationConcordanceBLL :
        StandardBLL<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance,
            ProductCatalogImportCSVConfigurationConcordanceDetails,
            ProductCatalogImportCSVConfigurationConcordanceOutputDetails,
            ProductCatalogImportCSVConfigurationConcordanceFilter>,
        IProductCatalogImportCSVConfigurationConcordanceBLL,
        ISelfRegisteredService<IProductCatalogImportCSVConfigurationConcordanceBLL>
    {
        public ProductCatalogImportCSVConfigurationConcordanceBLL(
            IRepository<ProductCatalogImportCSVConfigurationConcordance> repository,
            ILogger<ProductCatalogImportCSVConfigurationConcordanceBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ProductCatalogImportCSVConfigurationConcordanceOutputDetails,
                ProductCatalogImportCSVConfigurationConcordance> detailsBuilder,
            IInsertEntityBLL<ProductCatalogImportCSVConfigurationConcordance,
                ProductCatalogImportCSVConfigurationConcordanceDetails> insertEntityBLL,
            IModifyEntityBLL<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance,
                    ProductCatalogImportCSVConfigurationConcordanceDetails,
                    ProductCatalogImportCSVConfigurationConcordanceOutputDetails>
                modifyEntityBLL,
            IRemoveEntityBLL<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance>
                removeEntityBLL,
            IGetEntityBLL<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance,
                ProductCatalogImportCSVConfigurationConcordanceOutputDetails> detailsBLL,
            IGetEntityArrayBLL<ProductCatalogImportCSVConcordanceKey, ProductCatalogImportCSVConfigurationConcordance,
                    ProductCatalogImportCSVConfigurationConcordanceOutputDetails,
                    ProductCatalogImportCSVConfigurationConcordanceFilter>
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