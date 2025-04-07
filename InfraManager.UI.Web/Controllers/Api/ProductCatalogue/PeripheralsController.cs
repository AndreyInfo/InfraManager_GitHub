using InfraManager.BLL.ProductCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue
{
    [Authorize]
    [ApiController]
    [Route("api/peripherals")]
    public class PeripheralsController : ControllerBase
    {
        private readonly IPeripheralBLL _peripherals;

        public PeripheralsController(IPeripheralBLL peripherals)
        {
            _peripherals = peripherals;
        }

        [HttpGet("{id}")]
        public async Task<PeripheralDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _peripherals.DetailsAsync(id, cancellationToken);
        }
    }
}
