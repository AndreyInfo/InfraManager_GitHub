using System;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCsvConfiguration
{
    internal class ProductCatalogImportCSVConfigurationBLL :
        StandardBLL<Guid, ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationDetails,
            ProductCatalogImportCSVConfigurationOutputDetails, ProductCatalogImportCSVConfigurationFilter>,
        IProductCatalogImportCSVConfigurationBLL, ISelfRegisteredService<IProductCatalogImportCSVConfigurationBLL>
    {
        public ProductCatalogImportCSVConfigurationBLL(IRepository<ProductCatalogImportCSVConfiguration> repository,
            ILogger<ProductCatalogImportCSVConfigurationBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ProductCatalogImportCSVConfigurationOutputDetails, ProductCatalogImportCSVConfiguration>
                detailsBuilder,
            IInsertEntityBLL<ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationDetails>
                insertEntityBLL,
            IModifyEntityBLL<Guid, ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationDetails,
                    ProductCatalogImportCSVConfigurationOutputDetails>
                modifyEntityBLL,
            IRemoveEntityBLL<Guid, ProductCatalogImportCSVConfiguration> removeEntityBLL,
            IGetEntityBLL<Guid, ProductCatalogImportCSVConfiguration, ProductCatalogImportCSVConfigurationOutputDetails>
                detailsBLL,
            IGetEntityArrayBLL<Guid, ProductCatalogImportCSVConfiguration,
                    ProductCatalogImportCSVConfigurationOutputDetails, ProductCatalogImportCSVConfigurationFilter>
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