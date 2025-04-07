using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    [Route("api/calls")]
    [ApiController]
    [Authorize]
    public class CallDependencyController : ControllerBase
    {
        #region .ctor

        private readonly IDependencyBLL<CallDependency> _dependencyBLL;
        private readonly IMapper _mapper;

        public CallDependencyController(IDependencyBLL<CallDependency> dependencyBLL, IMapper mapper)
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