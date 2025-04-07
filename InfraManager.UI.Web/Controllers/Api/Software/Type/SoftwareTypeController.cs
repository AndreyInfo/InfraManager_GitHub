using AutoMapper;
using InfraManager.BLL.SoftwareType;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Software;

namespace InfraManager.UI.Web.Controllers.Api.Software.Type
{
    [Route("{controller}/{action}")]
    [ApiController]
    [Authorize]
    public class SoftwareTypeController : ControllerBase
    {
        private readonly ISoftwareTypeBLL _softwareTypeDataProvider;
        private readonly IMapper _mapper;

        public SoftwareTypeController(
            ISoftwareTypeBLL softwareTypeDataProvider,
            IMapper mapper
            )
        {
            _softwareTypeDataProvider = softwareTypeDataProvider ?? throw new ArgumentNullException(nameof(softwareTypeDataProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<SoftwareTypeResponse> GetSoftwareType(Guid id, CancellationToken cancellationToken = default)
        {
            var data = await _softwareTypeDataProvider.GetSoftwareType(id, cancellationToken);
            return _mapper.Map<SoftwareTypeResponse>(data);
        }

        [HttpPost]
        public async Task<bool> SaveSoftwareType([FromBody] SoftwareType model,
            CancellationToken cancellationToken = default)
        {
            return await _softwareTypeDataProvider.SaveAsync(model, cancellationToken);
        }

        [HttpPost]
        public async Task RemoveSoftwareType([FromBody] SoftwareType model, CancellationToken cancellationToken = default)
        {
            await _softwareTypeDataProvider.RemoveAsync(model, cancellationToken);
        }
    } 
}