using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.Catalog;
using InfraManager.DAL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.Calendar.CalendarHolidays;

namespace InfraManager.UI.Web.Controllers.Api.Calendar;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalendarHolidaysController : ControllerBase
{
    private readonly IBasicCatalogBLL<CalendarHoliday, CalendarHolidayTableDetails, Guid, CalendarHoliday> _catalogBLL;
    private readonly ICalendarHolidayBLL _calendarHolidayBLL;

    public CalendarHolidaysController(
        IBasicCatalogBLL<CalendarHoliday, CalendarHolidayTableDetails, Guid, CalendarHoliday> catalogBLL,
        ICalendarHolidayBLL calendarHolidayBLL)
    {
        _catalogBLL = catalogBLL;
        _calendarHolidayBLL = calendarHolidayBLL;
    }

    [HttpGet("{id}")]
    public async Task<CalendarHolidayDetails> GetByIDAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _calendarHolidayBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<CalendarHolidayTableDetails[]> GetTableAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _catalogBLL.GetByFilterAsync(filter, cancellationToken);
    }

    
    [HttpPost]
    public async Task<Guid> AddAsync([FromBody] CalendarHolidayInsertDetails model, CancellationToken cancellationToken)
    {
        return await _calendarHolidayBLL.AddAsync(model, cancellationToken);
    }
    
    [HttpPut("{id}")]
    public async Task<Guid> UpdateAsync([FromBody] CalendarHolidayDetails model, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _calendarHolidayBLL.UpdateAsync(model, id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _calendarHolidayBLL.DeleteAsync(id, cancellationToken);
    }

}
