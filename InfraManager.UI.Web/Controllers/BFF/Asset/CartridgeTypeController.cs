using InfraManager.BLL.Asset.dto;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Asset;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraManager.BLL.Asset.ForTable;

namespace InfraManager.UI.Web.Controllers.BFF.Asset
{
    [Authorize]
    [ApiController]
    [Route("api/cartridge/type")] // TODO отрефакторить
    public class CartridgeTypeController : ControllerBase
    {
        private readonly IBasicCatalogBLL<CartridgeType, CartridgeTypeDTO, Guid, CartridgeTypeForTable>
            _basicCatalogBLL;

        public CartridgeTypeController(
            IBasicCatalogBLL<CartridgeType, CartridgeTypeDTO, Guid, CartridgeTypeForTable> basicCatalogBLL)
        {
            _basicCatalogBLL = basicCatalogBLL;
        }

        [HttpGet("item")]
        public async Task<CartridgeTypeDTO> GetByIdAsync(Guid id)
        {
            return await _basicCatalogBLL.GetByIDAsync(id, HttpContext.RequestAborted);
        }

        [HttpPost("list")]
        public async Task<CartridgeTypeDTO[]> Get([FromBody] BaseFilter filter)
        {
            return await _basicCatalogBLL.GetByFilterAsync(filter, HttpContext.RequestAborted);
        }


        [HttpPost("save")]
        public async Task<Guid> GetByIdAsync([FromBody] CartridgeTypeDTO model)
        {
            return await _basicCatalogBLL.SaveOrUpdateAsync(model, HttpContext.RequestAborted);
        }



        [HttpDelete("remove")]
        public async Task<string[]> GetByIdAsync([FromBody] List<DeleteModel<Guid>> models)
        {
            return await _basicCatalogBLL.DeleteAsync(models, HttpContext.RequestAborted);
        }

    }
}
