using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.WebApi.Contracts.OrganizationStructure;
using InfraManager.Core.Extensions;
using System.Linq;
using Inframanager.BLL;
using InfraManager.BLL.OrganizationStructure.JobTitles;

namespace InfraManager.UI.Web.Controllers.BFF.OrganizationStructure;

[Authorize]
[ApiController]
[Route("bff/[controller]")]
[Obsolete("Use api/jobtitles instead")]
public class PositionsController : ControllerBase
{
    private readonly IJobTitleBLL _positionBLL;
    public PositionsController(IJobTitleBLL positionBLL)
    {
        _positionBLL = positionBLL;
    }

    [HttpGet("byid")]
    public Task<JobTitleDetails> GetByIDAsync([FromQuery] int id, CancellationToken cancellationToken = default) =>
        _positionBLL.DetailsAsync(id, cancellationToken);

    [HttpGet("list")]
    public async Task<JobTitleDetails[]> GetListAsync([FromQuery] string search, CancellationToken cancellationToken) =>
        await _positionBLL.GetDetailsArrayAsync(new JobTitleListFilter { Name = search }, cancellationToken);

    /// <summary>
    /// Получение должности по ID
    /// </summary>
    /// <param name="id">ImObjID Должности</param>
    /// <param name="cancellationToken"></param>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var jobTitles = await _positionBLL.GetDetailsArrayAsync(new JobTitleListFilter { IMObjID = id }, cancellationToken);

        return jobTitles.Any() ? Ok(jobTitles.First()) : NotFound();
    }
    
    /// <summary>
    /// Получение списка должностей по BaseFilter
    /// </summary>
    /// <param name="filter">BaseFilter для поиска должностей</param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    public async Task<JobTitleDetails[]> GetTableAsync([FromQuery]PositionsFilter filter, CancellationToken cancellationToken) => 
        await _positionBLL.GetDetailsPageAsync(
            new JobTitleListFilter
            {
                Name = filter.SearchString
            }, 
            new ClientPageFilter<JobTitle>
            {
                OrderByProperty = filter.OrderBy ?? nameof(JobTitle.ID),
                Skip = filter.StartRecordIndex,
                Take = filter.CountRecords,
                Ascending = true
            },
            cancellationToken);
    
    /// <summary>
    /// Добавление должности в базу
    /// </summary>
    /// <param name="positionDetails">DTO Должности</param>
    /// <param name="cancellationToken"></param>
    [HttpPost]
    public async Task AddAsync([FromBody]JobTitleDetails data, CancellationToken cancellationToken) =>
        await _positionBLL.AddAsync(data, cancellationToken);
    
    /// <summary>
    /// Обновление должности
    /// </summary>
    /// <param name="positionDetails">DTO Должности</param>
    /// <param name="cancellationToken"></param>
    [HttpPut]
    public async Task UpdateAsync([FromBody]JobTitleDetails positionDetails, CancellationToken cancellationToken) =>
        await _positionBLL.UpdateAsync(positionDetails.ID, positionDetails, cancellationToken);

    /// <summary>
    /// Удаление должности по ID
    /// </summary>
    /// <param name="id">ImObjID Должности</param>
    /// <param name="cancellationToken"></param>
    [HttpDelete("{id:guid}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var jobTitle = await _positionBLL.GetDetailsArrayAsync(new JobTitleListFilter { IMObjID = id }, cancellationToken);
        await _positionBLL.DeleteAsync(jobTitle.FirstOrDefault()?.ID ?? default, cancellationToken);
    }    
}
