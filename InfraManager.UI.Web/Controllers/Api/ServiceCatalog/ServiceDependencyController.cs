using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceDependencies;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Authorize]
    [ApiController]
    [Route("api/service/dependency")]
    public class ServiceDependencyController : ControllerBase
    {
        private readonly IServiceDependencyBLL _serviceDependencyBLL;

        public ServiceDependencyController(IServiceDependencyBLL serviceDependencyBLL)
        {
            _serviceDependencyBLL = serviceDependencyBLL;
        }


        [HttpPost("list")]
        public async Task<ServiceDetails[]> GetListByParentId([FromBody] BaseFilter model, [FromQuery] Guid? parentID, CancellationToken cancellationToken)
        {
            return await _serviceDependencyBLL.GetTableAsync(model, parentID, cancellationToken);
        }


        [HttpPost("save")]
        public async Task<bool>GetListByParentId([FromBody] ServiceDependencyModel model, CancellationToken cancellationToken)
        {
            return await _serviceDependencyBLL.AddAsync(model.ParentId, model.ChildId, cancellationToken);
        }

        [HttpDelete("remove")]
        public async Task<Guid[]> DeleteAsync ([FromBody] ServiceDependencyModel[] models, CancellationToken cancellationToken)
        {
            return await _serviceDependencyBLL.DeleteAsync(models, cancellationToken);
        }
    }
}
