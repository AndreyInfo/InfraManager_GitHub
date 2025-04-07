using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL;
using System.Threading;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Obsolete("Выпилить")]
    [Authorize]
    [ApiController]
    [Route("api/service/category")]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly ILookupBLL<ServiceCategoryItem, ServiceCategoryDetails, ServiceCategoryData, Guid> _serviceCategoryCatalogBLL;
        public ServiceCategoryController(ILookupBLL<ServiceCategoryItem, ServiceCategoryDetails, ServiceCategoryData, Guid> basicCatalogBLL)
        {
            _serviceCategoryCatalogBLL = basicCatalogBLL;
        }


        [HttpGet("item")]
        public async Task<ServiceCategoryDetails> GetById(Guid id)
        {
            var result = await _serviceCategoryCatalogBLL.FindAsync(id, HttpContext.RequestAborted);

            return result;
        }

        [HttpGet("list")]
        public async Task<ServiceCategoryItem[]> GetList()
        {
            var result = await _serviceCategoryCatalogBLL.ListAsync(HttpContext.RequestAborted);

            return result;
        }

        [HttpPost("save")]
        public async Task<Guid> SaveOrUpdateAsync([FromBody] ServiceCategoryData model)
        {
            var resultData = new ServiceCategoryDetails();
            if (model.ID == default)
            {
                model.ID = Guid.NewGuid();
                resultData = await _serviceCategoryCatalogBLL.AddAsync(model, HttpContext.RequestAborted);
            }
            else
            {
                resultData = await _serviceCategoryCatalogBLL.UpdateAsync(model.ID, model, HttpContext.RequestAborted);
            }

            return resultData.ID;
        }

        [HttpDelete("{id}")]
        public async Task RemoveAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _serviceCategoryCatalogBLL.DeleteAsync(id, cancellationToken);
        }
    }
}
