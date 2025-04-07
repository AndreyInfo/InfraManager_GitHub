using InfraManager.BLL.Asset.dto;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InfraManager.Web.BLL.Assets;
using InfrastructureSegment = InfraManager.DAL.Asset.InfrastructureSegment;


namespace InfraManager.UI.Web.Controllers.BFF.Asset
{
    [Authorize]
    [ApiController]
    [Route("api/infrastructure")]
    public class InfrastructureSegmentController : ControllerBase
    {
        private readonly IBasicCatalogBLL<InfrastructureSegment, InfrastructureSegmentDTO, Guid, InfrastructureSegmentForTable> _basicCatalogBLL;
        public InfrastructureSegmentController(IBasicCatalogBLL<InfrastructureSegment, InfrastructureSegmentDTO, Guid, InfrastructureSegmentForTable> basicCatalogBLL)
        {
            _basicCatalogBLL = basicCatalogBLL;
        }


        #region Main CRUD
        [HttpPost("save")]
        public async Task<Guid> GetDataForTable([FromBody] InfrastructureSegmentDTO model)
        {
            return await _basicCatalogBLL.SaveOrUpdateAsync(model, HttpContext.RequestAborted);
        }

        [HttpDelete("remove")]
        public async Task<string[]> Remove([FromBody] DeleteModel<Guid>[] models)
        {
            return await _basicCatalogBLL.DeleteAsync(models, HttpContext.RequestAborted);
        }
        #endregion


        [HttpPost("table")]
        public async Task<InfrastructureSegmentDTO[]> GetDataForTable([FromBody] BaseFilter model)
        {
            return await _basicCatalogBLL.GetByFilterAsync(model, HttpContext.RequestAborted);
        }
    }
}
