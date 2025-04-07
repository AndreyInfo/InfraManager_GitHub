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

namespace InfraManager.UI.Web.Controllers.Api.DataEntity
{
    [Authorize]
    [ApiController]
    [Route("api/dataentity")]
    public class DataEntityController : ControllerBase
    {
        private readonly IDataEntityBLL _dataEntityBLL;

        public DataEntityController(IDataEntityBLL dataEntityBLL)
        {
            _dataEntityBLL = dataEntityBLL;
        }

        [HttpGet("{id}")]
        public async Task<DataEntityDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dataEntityBLL.DetailsAsync(id, cancellationToken);
        }
    }
}
