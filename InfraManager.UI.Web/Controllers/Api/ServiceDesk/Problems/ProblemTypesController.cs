using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL.ServiceDesk.Problems;
using System.Threading;
using AutoMapper;
using InfraManager.UI.Web.Models;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class ProblemTypesController : ControllerBase
    {
        private readonly IProblemTypesBLL _problemTypesBLL;
        private readonly IMapper _mapper;

        public ProblemTypesController(
            IProblemTypesBLL problemTypesBLL,
            IMapper mapper)
        {
            _problemTypesBLL = problemTypesBLL;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ProblemTypeDetails[]> ListAsync(CancellationToken cancellationToken = default)
        {
            return _mapper.Map<ProblemTypeDetails[]>(
                await _problemTypesBLL.GetAllDetailsArrayAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ProblemTypeDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _problemTypesBLL.GetByIdAsync(id, cancellationToken);
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var content = await _problemTypesBLL.GetImageAsync(id, cancellationToken);
            return File(content, "image/png"); // TODO: store files on disk (cache) is possible
        }

        [HttpPost]
        public async Task<ProblemTypeDetails> AddAsync([FromBody] ProblemTypeData model,
            CancellationToken cancellationToken = default)
        {
            return await _problemTypesBLL.AddAsync(model, cancellationToken);
        }
        
        [HttpPut("{id}")]
        public async Task<ProblemTypeDetails> UpdateAsync([FromBody] ProblemTypeData model, [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _problemTypesBLL.UpdateAsync(id, model, cancellationToken);
        }
        
        [HttpDelete]
        [Route("/api/problem/type/item/remove")]
        public async Task DeleteAsync(Guid problemTypeID, CancellationToken cancellationToken = default)
        {
            await _problemTypesBLL.DeleteAsync(problemTypeID, cancellationToken);
        }
        
        #region Obsolete

        [HttpGet]
        [Route("/api/problem/type/item")]
        [Obsolete("Use api/problemtypes/{id} instead")]
        public async Task<ProblemTypeDetails> GetProblemTypeByIdAsync(Guid problemTypeID,
            CancellationToken cancellationToken = default)
        {
            return await _problemTypesBLL.GetByIdAsync(problemTypeID, cancellationToken);
        }


        [HttpGet]
        [Route("/api/problem/type/item/path")]
        public async Task<ProblemTypeDetails[]> GetPathInTreeIdAsync(Guid problemTypeID,
            CancellationToken cancellationToken = default)
        {
            return await _problemTypesBLL.GetPathByIdAsync(problemTypeID, cancellationToken);
        }
        
        [HttpPost]
        [Route("/api/problem/type/item/tree")]
        public async Task<ProblemTypeDetails[]> GetTreeByIdAsync([FromBody] ProblemTypeTreeModel model,
            [FromQuery] List<Guid> filterId, CancellationToken cancellationToken)
        {
            filterId ??= new List<Guid>();

            return await _problemTypesBLL.GetTreeByIdAsync(model.ProblemTypeID, filterId, cancellationToken);
        }

        #endregion
    }
}