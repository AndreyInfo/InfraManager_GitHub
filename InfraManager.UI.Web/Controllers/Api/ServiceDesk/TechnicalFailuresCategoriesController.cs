using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Route("api/[controller]")]
[Obsolete("Use api/technicalFailureCategories instead")]
[ApiController]
[Authorize]
public class TechnicalFailuresCategoriesController : ControllerBase
{
   private readonly ITechnicalFailureCategoryBLL _service;

   public TechnicalFailuresCategoriesController(ITechnicalFailureCategoryBLL service)
   {
      _service = service;
   }
   
   [HttpGet("{id}")]
   public async Task<TechnicalFailureCategoryDetails> GetAsync([FromRoute] int id,
      CancellationToken cancellationToken = default)
   {
      return await _service.DetailsAsync(id, cancellationToken);
   }

   [HttpGet]
   public async Task<TechnicalFailureCategoryDetails[]> ListAsync([FromQuery] TechnicalFailureCategoryFilter filter,
      CancellationToken cancellationToken = default)
   {
      return await _service.GetDetailsArrayAsync(filter, cancellationToken);
   }

   [HttpPost]
   public async Task<TechnicalFailureCategoryDetails> PostAsync([FromBody] TechnicalFailureCategoryData data,
      CancellationToken cancellationToken = default)
   {
      return await _service.AddAsync(data, cancellationToken);
   }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }

   [HttpPut("{id}")]
   public async Task<TechnicalFailureCategoryDetails> PutAsync([FromRoute] int id,
      TechnicalFailureCategoryData data, CancellationToken cancellationToken = default)
   {
      return await _service.UpdateAsync(id, data, cancellationToken);
   }
}