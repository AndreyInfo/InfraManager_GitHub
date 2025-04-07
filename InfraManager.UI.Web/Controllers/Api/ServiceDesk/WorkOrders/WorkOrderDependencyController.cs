using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders
{
    [Route("api/workorders")]
    [ApiController]
    [Authorize]
    public class WorkOrderDependencyController : ControllerBase
    {
        #region .ctor

        private readonly IDependencyBLL<ProblemDependency> _dependencyBLL;
        private readonly IMapper _mapper;

        public WorkOrderDependencyController(IDependencyBLL<ProblemDependency> dependencyBLL, IMapper mapper)
        {
            _dependencyBLL = dependencyBLL;
            _mapper = mapper;
        }

        #endregion

        #region api/calls

        [HttpGet("{id}/dependencies")]
        public async Task<DependencyDetails[]> DependenciesAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var dependencies = await _dependencyBLL.GetDependenciesAsync(id, cancellationToken);
            return _mapper.Map<DependencyDetails[]>(dependencies);
        }


        #endregion
    }
}