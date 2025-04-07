using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InfraManager.BLL.Catalog;
using InfraManager.BLL.CrudWeb;
using InfraManager.DAL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.Calendar.CalendarWeekends;

namespace InfraManager.UI.Web.Controllers.Api.Calendar;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalendarWeekendsController : ControllerBase
{
    private readonly IBasicCatalogBLL<CalendarWeekend, CalendarWeekendDetails, Guid,CalendarWeekend> _basicCatalogBLL;
    private readonly ICalendarWeekenedBLL _calendarWeekenedBLL;
    public CalendarWeekendsController(IBasicCatalogBLL<CalendarWeekend, CalendarWeekendDetails, Guid,CalendarWeekend> basicCatalogBLL
                                      , ICalendarWeekenedBLL calendarWeekenedBLL)
    {
        _basicCatalogBLL = basicCatalogBLL;
        _calendarWeekenedBLL = calendarWeekenedBLL;
    }

    [HttpGet("{id}")]
    public async Task<CalendarWeekendDetails> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        return await _basicCatalogBLL.GetByIDAsync(id, cancellationToken);
    }

    [HttpGet]
    public async Task<CalendarWeekendDetails[]> GetAsync([FromQuery] BaseFilter filter, CancellationToken cancellationToken)
    {
        return await _basicCatalogBLL.GetByFilterAsync(filter, cancellationToken);
    }


    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CalendarWeekendDetails model, CancellationToken cancellationToken)
    {
        return await _basicCatalogBLL.InsertAsync(model, cancellationToken);
    }

    [HttpPut]
    public async Task<Guid> PutAsync([FromBody] CalendarWeekendDetails model, CancellationToken cancellationToken)
    {
        return await _basicCatalogBLL.UpdateAsync(model.ID, model, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _calendarWeekenedBLL.DeleteAsync(id, cancellationToken);
    }
}
