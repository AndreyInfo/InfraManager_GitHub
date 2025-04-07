using InfraManager.BLL.Settings.TableFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.TableFilters
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FiltersController : ControllerBase
    {
        #region .ctor

        private readonly ITableFiltersBLL _service;

        public FiltersController(ITableFiltersBLL service)
        {
            _service = service;
        }

        #endregion

        #region Filter resource

        [HttpGet]
        public async Task<ActionResult<FilterDetails[]>> GetListAsync([FromQuery]string view, CancellationToken cancelationToken = default)
        {
            if (string.IsNullOrWhiteSpace(view))
            {
                return BadRequest("Parameter 'view' is missing.");
            }

            return await _service.GetListAsync(view, cancelationToken);
        }

        [HttpGet("{id}")]
        public Task<FilterDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.GetAsync(id, cancellationToken);
        }

        [HttpPost("{view}")]
        public async Task<ActionResult<FilterDetails>> PostAsync(
            string view, 
            [FromBody]FilterData model, 
            CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                return BadRequest();
            }

            return await _service.AddAsync(view, model, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FilterDetails>> UpdateAsync(
            Guid id,
            [FromBody]FilterData model,
            CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                return BadRequest();
            }

            return await _service.UpdateAsync(id, model, cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _service.DeleteAsync(id, cancellationToken);
        }

        #endregion

        #region CurrentFilter resource

        [HttpGet("/api/currentfilters/{view}")]
        public Task<CurrentFilterDetails> GetCurrentAsync(string view, CancellationToken cancellationToken = default)
        {
            return _service.GetCurrentAsync(view, cancellationToken);
        }

        [HttpPut("/api/currentfilters/{view}")]
        public async Task<IActionResult> SetCurrentAsync(string view, [FromBody]CurrentFilterData model, CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                return BadRequest();
            }

            await _service.SetCurrentAsync(view, model, cancellationToken);
            return Ok();
        }

        #endregion
    }
}
