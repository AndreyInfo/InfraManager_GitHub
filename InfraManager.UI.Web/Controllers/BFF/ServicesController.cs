using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.UI.Web.Models.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Bff.ServiceCatalog
{
    [Route("bff/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceBLL _service;

        public ServicesController(IServiceBLL service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ServiceDetailsModel> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var model = await _service.FindAsync(id, cancellationToken);

            return model;
        }
    }
}
