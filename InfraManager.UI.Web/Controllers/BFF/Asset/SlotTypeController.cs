using InfraManager.BLL.Asset.dto;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraManager.Web.BLL.Assets;
using SlotType = InfraManager.DAL.Asset.SlotType;

namespace InfraManager.UI.Web.Controllers.BFF.Asset
{
    [Authorize]
    [ApiController]
    [Route("api/slot/type")]
    public class SlotTypeController : ControllerBase
    {
        private readonly IBasicCatalogBLL<SlotType, SlotTypeDTO, int, SlotTypeForTable> _basicCatalogBLL;

        public SlotTypeController(IBasicCatalogBLL<SlotType, SlotTypeDTO, int, SlotTypeForTable> basicCatalogBLL)
        {
            _basicCatalogBLL = basicCatalogBLL;
        }

        [HttpGet("item")]
        public async Task<SlotTypeDTO> GetByIDAsync(int id)
        {
            return await _basicCatalogBLL.GetByIDAsync(id, HttpContext.RequestAborted);
        }

        [HttpPost("list")]
        public async Task<SlotTypeDTO[]> Get([FromBody] BaseFilter filter)
        {
            return await _basicCatalogBLL.GetByFilterAsync(filter, HttpContext.RequestAborted);
        }


        [HttpPost("save")]
        public async Task<int> GetByIdAsync([FromBody] SlotTypeDTO model)
        {
            return await _basicCatalogBLL.SaveOrUpdateAsync(model, HttpContext.RequestAborted);
        }



        [HttpDelete("remove")]
        public async Task<string[]> GetByIdAsync([FromBody] List<DeleteModel<int>> models)
        {
            return await _basicCatalogBLL.DeleteAsync(models, HttpContext.RequestAborted);
        }
    }
}
