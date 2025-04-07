using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Calls;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.Calls.DTO;
using InfraManager.Web.Controllers;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.ServiceDesk;
using InfraManager.IM.BusinessLayer.ConfigurationData;
using InfraManager.BLL.DataEntities.DTO;
using InfraManager.BLL.DataEntities;
using InfraManager.BLL.ConfigurationUnit.DTO;
using InfraManager.BLL.ConfigurationUnit;

namespace InfraManager.UI.Web.Controllers.Api.ConfigurationUnit
{
    [Authorize]
    [ApiController]
    [Route("api/ConfigurationUnit")]
    public class ConfigurationUnitController : ControllerBase
    {
        private readonly IConfigurationUnitBLL _configurationUnitBLL;

        public ConfigurationUnitController(IConfigurationUnitBLL ConfigurationUnitBLL)
        {
            _configurationUnitBLL = ConfigurationUnitBLL;
        }

        [HttpGet("{id}")]
        public async Task<ConfigurationUnitDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _configurationUnitBLL.DetailsAsync(id, cancellationToken);
        }
    }
}
