using IM.Core.Import.BLL.Interface;
using IM.Import.Services.Logger;
using InfraManager.DAL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace InfraManager.IM.ImportService.Controllers
{
    [ApiController]
    [Route("bff/[controller]")]
    [Authorize]
    public class LoggerController : Controller
    {
        private readonly IImportLogger _service;
        public LoggerController(IImportLogger service)
        {
            _service = service;
        }

        [HttpGet("titles/{id}")]
        public Task<List<TitleLog>> GetAllTitlesById(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.GetAllTitleLogsByTaskIdAsync(id, cancellationToken);
        }

        [HttpGet("{id}")]
        public Task<LogTask> GetLogById(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.GetLogByIdAsync(id, cancellationToken);
        }
    }
}
