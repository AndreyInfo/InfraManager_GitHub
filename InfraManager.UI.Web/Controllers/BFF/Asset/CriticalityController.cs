using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Asset.dto;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.Controllers;
using Criticality = InfraManager.DAL.Asset.Criticality;
using System.Threading;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers.BFF.Asset
{
    [Authorize]
    [ApiController]
    [Route("api/criticality")]
    [Obsolete("Use api/criticalities instead.")]
    public class CriticalityController : ControllerBase
    {
        private readonly IBasicCatalogBLL<Criticality, CriticalityDTO, Guid, CriticalityForTable> _criticalityCatalogBLL;

        public CriticalityController(IBasicCatalogBLL<Criticality, CriticalityDTO, Guid, CriticalityForTable> basicCatalogBLL)
        {
            _criticalityCatalogBLL = basicCatalogBLL;
        }

        [HttpGet("item")]
        public async Task<CriticalityDTO> GetByID(Guid id, CancellationToken token)
        {
            return await _criticalityCatalogBLL.GetByIDAsync(id, token);
        }

        [HttpPost("list")]
        public async Task<CriticalityDTO[]> GetListAsync([FromBody] BaseFilter filter, CancellationToken token)
        {
            return await _criticalityCatalogBLL.GetByFilterAsync(filter, token);
        }

        [HttpPost("save")]
        public async Task<Guid> SaveOrUpdateAsync([FromBody] CriticalityDTO model, CancellationToken token)
        {
            return await _criticalityCatalogBLL.SaveOrUpdateAsync(model, token);
        }

        [HttpDelete("remove")]
        public async Task<string[]> RemoveAsync([FromBody] DeleteModel<Guid>[] model, CancellationToken token)
        {
            return await _criticalityCatalogBLL.DeleteAsync(model, token);
        }

    }
}
