using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProblemReferencesController : ControllerBase
{
    private readonly IProblemReferenceBLL _service;
    private readonly IMapper _mapper;

    public ProblemReferencesController(IProblemReferenceBLL service,
        IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [Obsolete]
    [HttpGet("{objectID}/{objectClassID}")]
    public async Task<ProblemListItemModel[]> Get([FromQuery] BaseFilter filter, [FromRoute] Guid objectID,
        [FromRoute] ObjectClass objectClassID, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetReferencedProblemsAsync(filter, objectID, objectClassID, cancellationToken);

        return _mapper.Map<ProblemListItemModel[]>(result);
    }
}