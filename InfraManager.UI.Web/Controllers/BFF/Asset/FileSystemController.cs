using InfraManager.BLL.Asset.dto;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfraManager.Web.BLL.Assets;
using FileSystem = InfraManager.DAL.Asset.FileSystem;

namespace InfraManager.UI.Web.Controllers.BFF.Asset
{
    [Authorize]
    [ApiController]
    [Route("api/file/system")]
    public class FileSystemController : ControllerBase
    {
        private readonly IBasicCatalogBLL<FileSystem, FileSystemDTO, Guid, FileSystemForTable> _basicCatalogBLL;

        public FileSystemController(IBasicCatalogBLL<FileSystem, FileSystemDTO, Guid, FileSystemForTable> basicCatalogBLL)
        {
            _basicCatalogBLL = basicCatalogBLL;
        }

        [HttpGet("item")]
        public async Task<FileSystemDTO> GetByIDAsync(Guid id)
        {
            return await _basicCatalogBLL.GetByIDAsync(id, HttpContext.RequestAborted);
        }

        [HttpPost("list")]
        public async Task<FileSystemDTO[]> Get([FromBody] BaseFilter filter)
        {
            return await _basicCatalogBLL.GetByFilterAsync(filter, HttpContext.RequestAborted);
        }


        [HttpPost("save")]
        public async Task<Guid> GetByIdAsync([FromBody] FileSystemDTO model)
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
