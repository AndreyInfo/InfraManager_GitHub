using AutoMapper;
using InfraManager.BLL.ServiceCatalogue;
using InfraManager.WebApi.Contracts.Models.ServiceCatalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Scripting.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceCatalog
{
    [Route("api/[controller]")]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceBLL _service;
        private readonly IMapper _mapper;

        public ServicesController(IServiceBLL service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private ServiceDetailsModel ToModel(ServiceDetails details) => _mapper.Map<ServiceDetailsModel>(details);

        [HttpGet]
        public async Task<ServiceDetailsModel[]> GetAsync([FromQuery] ServiceListFilter filterBy, CancellationToken cancellationToken = default) =>
            (await _service.GetDetailsPageAsync(filterBy, cancellationToken)).Select(ToModel).ToArray();

        [HttpGet("{id}")]
        public async Task<ServiceDetailsModel> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
            ToModel(await _service.DetailsAsync(id, cancellationToken));
    }
}
