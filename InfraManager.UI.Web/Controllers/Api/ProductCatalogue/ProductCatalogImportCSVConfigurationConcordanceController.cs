using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue
{
    [Route("api/ProductCatalogue/ProductCatalogImportCSVConfigurationConcordance")]
    [ApiController]
    [Authorize]
    public class ProductCatalogImportCSVConfigurationConcordanceController : ControllerBase
    {
        private readonly IProductCatalogImportCSVConfigurationConcordanceBLL _bll;

        public ProductCatalogImportCSVConfigurationConcordanceController(
            IProductCatalogImportCSVConfigurationConcordanceBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{id}/{field}")]
        public Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails> Get(
            Guid id, string field, CancellationToken cancellationToken = default)
        {
            return _bll.DetailsAsync(new ProductCatalogImportCSVConcordanceKey(id, field), cancellationToken);
        }

        [HttpGet]
        public Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails[]> Get(
            [FromQuery] ProductCatalogImportCSVConfigurationConcordanceFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails> Post(
            [FromBody] ProductCatalogImportCSVConfigurationConcordanceDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}/{field}")]
        public Task<ProductCatalogImportCSVConfigurationConcordanceOutputDetails> Put(
            Guid id, string field,
            [FromBody] ProductCatalogImportCSVConfigurationConcordanceDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.UpdateAsync(new ProductCatalogImportCSVConcordanceKey(id,field), data, cancellationToken);
        }

        [HttpDelete("{id}/{field}")]
        public Task Delete(Guid id, string field, CancellationToken cancellationToken = default)
        {
            return _bll.DeleteAsync(new ProductCatalogImportCSVConcordanceKey(id, field), cancellationToken);
        }
    }
}