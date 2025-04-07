using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.Calendar.CalendarHolidays;

namespace InfraManager.UI.Web.Controllers.Api.Calendar;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CalendarHolidayItemsController : ControllerBase
{
    private readonly ICalendarHolidayItemBLL _calendarHolidayItemBLL;

    public CalendarHolidayItemsController(ICalendarHolidayItemBLL calendarHolidayItemBLL)
    {
        _calendarHolidayItemBLL = calendarHolidayItemBLL;
    }


    [HttpGet("{id}")]
    public async Task<CalendarHolidayItemDetails> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _calendarHolidayItemBLL.GetByIDAsync(id, cancellationToken);
    }

    
    [HttpGet]
    public async Task<CalendarHolidayItemDetails[]> GetListAsync([FromQuery] string search, [FromQuery] Guid calendarHolidayID, CancellationToken cancellationToken)
    {
        return await _calendarHolidayItemBLL.GetListAsync(calendarHolidayID, search, cancellationToken);
    }

    [HttpPost]
    public async Task<Guid> AddAsync([FromBody] CalendarHolidayItemDetails model, CancellationToken cancellationToken)
    {
        return await _calendarHolidayItemBLL.AddAsync(model, cancellationToken);
    }

    [HttpPut]
    public async Task<Guid> UpdateAsync([FromBody] CalendarHolidayItemDetails model, CancellationToken cancellationToken)
    {
        return await _calendarHolidayItemBLL.UpdateAsync(model, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] DeleteModel<Guid>[] models, CancellationToken cancellationToken)
    {
        await _calendarHolidayItemBLL.DeleteAsync(models, cancellationToken);
    }
}
