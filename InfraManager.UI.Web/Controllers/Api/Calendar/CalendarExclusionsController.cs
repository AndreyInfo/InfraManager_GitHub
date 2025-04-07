using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Calendar;
using System.Threading;
using InfraManager.BLL.Calendar.CalendarExclusions;

namespace InfraManager.UI.Web.Controllers.Api.Calendar
{
    [Authorize]
    [ApiController]
    [Route("api/CalendarExclusions")]
    public class CalendarExclusionsController : ControllerBase
    {
        private readonly ICalendarExclusionBLL _calendarExclusionBLL;

        public CalendarExclusionsController(ICalendarExclusionBLL calendarExclusionBLL)
        {
            _calendarExclusionBLL = calendarExclusionBLL;
        }

        [HttpGet("{exclusionId}")]
        public async Task<CalendarExclusionDetails> GetByIdAsync([FromRoute] Guid exclusionId)
        {
            var result = await _calendarExclusionBLL.GetByIDAsync(exclusionId, HttpContext.RequestAborted);

            return result;
        }

        [HttpPost]
        public async Task<CalendarExclusionDetails> AddAsync([FromBody] CalendarExclusionInsertDetails calendarExclusion, CancellationToken cancellationToken)
        {
            return await _calendarExclusionBLL.AddAsync(calendarExclusion, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<CalendarExclusionDetails> UpdateAsync([FromBody] CalendarExclusionDetails calendarExclusion, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return await _calendarExclusionBLL.UpdateAsync(calendarExclusion, id, cancellationToken);
        }

        [HttpDelete]
        public async Task<List<string>> BulkDeleteAsync([FromBody] DeleteModel<Guid>[] deleteModels)
        {
            var result = await _calendarExclusionBLL.DeleteAsync(deleteModels, HttpContext.RequestAborted);

            return result;
        }

        //TODO передлать на Get, соглосовать с фротном
        [HttpPost("byFilter")]
        public async Task<CalendarExclusionDetails[]> GetTableForService([FromBody] BaseFilterWithClassIDAndID<Guid> filter)
        {
            var result = await _calendarExclusionBLL.GetByFilterAsync(filter, HttpContext.RequestAborted);

            return result;
        }
    }
}
