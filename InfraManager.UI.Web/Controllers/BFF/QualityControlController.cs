using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.BLL.ServiceDesk.Quality;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF
{

    [Route("api/{controller}")]
    [ApiController]
    [Authorize]
    public class QualityControlController : ControllerBase
    {
        private readonly IQualityControlBLL _qualityControlBLL;

        public QualityControlController(IQualityControlBLL qualityControlBLL)
        {
            _qualityControlBLL = qualityControlBLL;
        }

        [HttpPost]
        public async Task<QualityControlDetails> PostAsync([FromBody] QualityControlData model, CancellationToken cancellationToken = default)
        {
            return await _qualityControlBLL.AddAsync(model, cancellationToken);
        }

        [HttpGet("last-by-call")]
        public async Task<DateTime?> GetLastVyCallAsync([FromQuery] Guid callID, CancellationToken cancellationToken = default)
        {
            return await _qualityControlBLL.GetLastByCallAsync(callID, cancellationToken);
        }
    }
}
