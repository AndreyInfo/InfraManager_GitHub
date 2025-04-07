using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager.UI.Web.Models.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceItemsController : ControllerBase
    {
        private readonly IServiceItemBLL _service;
        private readonly IMapper _mapper;
        private readonly ISupportLineBLL _supportLineBLL;

        public ServiceItemsController(IServiceItemBLL service, IMapper mapper, ISupportLineBLL supportLineBLL)
        {
            _service = service;
            _mapper = mapper;
            _supportLineBLL = supportLineBLL;
        }

        [Obsolete("Выпилить или в bff")]
        [HttpGet("callSummary/{id}")]
        public async Task<ServiceItemViewModel> GetByCallSummaryIDAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var model = await _service.DetailsByCallSummaryIDAsync(id, cancellationToken);

            return _mapper.Map<ServiceItemViewModel>(model);
        }

        [HttpGet("{id}")]
        public async Task<ServiceItemViewModel> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var model = await _service.DetailsAsync(id, cancellationToken);

            return _mapper.Map<ServiceItemViewModel>(model);
        }

        [Obsolete("Выпилить или в bff")]
        [HttpGet("{id}/responsible")]
        public async Task<SupportLineResponsibleDetails[]> GetResponsibleAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await _supportLineBLL.GetResponsibleObjectLineAsync(id, ObjectClass.ServiceItem, cancellationToken);
        }
    }
}