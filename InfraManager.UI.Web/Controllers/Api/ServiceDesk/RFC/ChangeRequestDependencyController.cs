using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.RFC
{
    [Route("api/ChangeRequests")]
    [ApiController]
    [Authorize]
    public class ChangeRequestDependencyController : ControllerBase
    {
        #region .ctor

        private readonly IDependencyBLL<ProblemDependency> _dependencyBLL;
        private readonly IMapper _mapper;

        public ChangeRequestDependencyController(IDependencyBLL<ProblemDependency> dependencyBLL, IMapper mapper)
        {
            _dependencyBLL = dependencyBLL;
            _mapper = mapper;
        }

        #endregion

        #region api/changerequests

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