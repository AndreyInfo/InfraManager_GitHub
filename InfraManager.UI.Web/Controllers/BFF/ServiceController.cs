using Inframanager.BLL;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Authorize]
    [ApiController]
    [Route("api/service")]
    public class ServiceController : ControllerBase
    {
        private readonly IBasicCatalogBLL<Service, ServiceDetails, Guid, Service> _serviceCatalogBLL;
        private readonly IServiceBLL _serviceBLL;
        private readonly IEnumBLL<ServiceType> _serviceTypes;
        private readonly IEnumBLL<CatalogItemState> _catalogItemStates;
        private readonly ISupportLineBLL _supportLineBLL;

        public ServiceController(IBasicCatalogBLL<Service, ServiceDetails, Guid, Service> serviceCatalogBLL,
            IServiceBLL serviceBLL,
            IEnumBLL<ServiceType> serviceTypes,
            IEnumBLL<CatalogItemState> catalogItemStates, 
            ISupportLineBLL supportLineBLL)
        {
            _serviceCatalogBLL = serviceCatalogBLL;
            _serviceBLL = serviceBLL;
            _serviceTypes = serviceTypes;
            _catalogItemStates = catalogItemStates;
            _supportLineBLL = supportLineBLL;
        }

        [HttpGet("item")]
        public async Task<ServiceDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken) 
            => await _serviceBLL.GetByIDAsync(id, cancellationToken);


        [HttpGet("list")]
        public async Task<ServiceDetails[]> GetListAsync(CancellationToken cancellationToken = default) 
            => await _serviceCatalogBLL.GetListAsync(null, cancellationToken);
        

        [HttpGet]
        public async Task<PortfolioServiceItemTable[]> GetListAsync([FromQuery] ServiceFilter filter, CancellationToken cancellationToken = default) => 
            await _serviceBLL.GetServicesForTableAsync(filter, cancellationToken);

        [HttpPost]
        public async Task<Guid?> PostAsync([FromBody] ServiceData model, CancellationToken cancellationToken) 
            => await _serviceBLL.AddAsync(model, cancellationToken);
        

        [HttpPut("{id}")]
        public async Task<Guid> PutAsync([FromBody] ServiceData model, Guid id, CancellationToken cancellationToken) 
            => await _serviceBLL.UpdateAsync(model, id ,cancellationToken);
        

        [HttpDelete("{id:guid}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken) 
            => await _serviceCatalogBLL.RemoveAsync(id, cancellationToken);
        

        [HttpGet("list/bycategory")]
        public async Task<ServiceDetails[]> GetListByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _serviceBLL.GetListByCategoryIDAsync(categoryId, cancellationToken);
        }


        [HttpGet("type/list")]
        public async Task<LookupItem<ServiceType>[]> GetTypes(CancellationToken cancellationToken)
        {
            return await _serviceTypes.GetAllAsync(cancellationToken);
        }


        [HttpGet("state/list")]
        public async Task<LookupItem<CatalogItemState>[]> GetStates(CancellationToken cancellationToken)
        {
            return await _catalogItemStates.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}/responsible")]
        public async Task<SupportLineResponsibleDetails[]> GetResponsibleAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await _supportLineBLL.GetResponsibleObjectLineAsync(id, ObjectClass.Service, cancellationToken);
        }
    }
}
