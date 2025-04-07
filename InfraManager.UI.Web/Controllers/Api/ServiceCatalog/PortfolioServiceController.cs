using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue;
using InfraManager.BLL.ServiceCatalogue;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    [Authorize]
    [ApiController]
    [Route("api/portfolio/service")]
    public class PortfolioServiceController : ControllerBase
    {

        private readonly IPortfolioServiceBLL _portfolioServiceBLL; 

        public PortfolioServiceController(IPortfolioServiceBLL portfolioServiceBLL)
        {
            _portfolioServiceBLL = portfolioServiceBLL;
        }


        [HttpPost("tree")]
        public async Task<PortfolioServicesItem[]> GetList([FromBody] PortfolioServiceFilter filter, CancellationToken cancellationToken = default)
        {
            return await _portfolioServiceBLL.GetTreeAsync(filter, cancellationToken);
        }

        [HttpGet("path")]
        public PortfolioServicesItem[] GetPath(ObjectClass classID, Guid id)
        {
            return _portfolioServiceBLL.GetPath(classID, id);
        }



        [HttpPost("customer")]
        public ServiceCustomerDetails GetCustomer(ObjectClass classID, Guid id)
        {
            return _portfolioServiceBLL.GetCustomer(classID, id);
        }


        [HttpGet("not/customers")]
        public ServiceCustomerDetails[] GetNotCustomers([FromQuery] ObjectClass[] classIDs, [FromQuery] Guid[] ids, string search = "")
        {
            return _portfolioServiceBLL.GetNotCustomer(classIDs, ids, search);
        }

        [HttpPost("Infrastructure")]
        public async Task<Guid> InsertInfrastructureAsync([FromBody] ServiceReferenceModel model, CancellationToken cancellationToken = default)
        {
            return await _portfolioServiceBLL.AddInfrastructureAsync(model, cancellationToken);
        }

        [HttpGet("Infrastructure/{serviceID}")]
        public async Task<PortfolioServiceInfrastructureItem[]> GetInfrastructureAsync([FromRoute] Guid serviceID, CancellationToken cancellationToken = default)
        {
            return await _portfolioServiceBLL.GetInfrastructureAsync(serviceID, cancellationToken);
        }
    }
}