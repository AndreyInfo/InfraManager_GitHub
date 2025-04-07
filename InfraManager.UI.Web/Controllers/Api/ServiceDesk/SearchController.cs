using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Search;
using InfraManager.Services.SearchService;
using InfraManager.UI.Web.Models.Search;
using InfraManager.UI.Web.Services.Search;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly ServiceDeskSearchService _searchService;
    private readonly IMapper _mapper;
    
    public SearchController(ServiceDeskSearchService searchService,
        IMapper mapper)
    {
        _searchService = searchService;
        _mapper = mapper;
    }
 
    [HttpGet]
    public async Task<ListItemModel[]> GetAsync([FromQuery] SearchParametersModel searchParams,
        CancellationToken cancellationToken)
    {
        var result = await _searchService.SearchAsync(new SearchFilter
        {
            Ascending = searchParams.Ascending,
            Classes = searchParams.Classes,
            Skip = searchParams.Skip,
            Take = searchParams.Take,
            Text = searchParams.Text,
            OrderBy = searchParams.OrderBy,
            SearchFinished = searchParams.SearchFinished,
            SearchMode = searchParams.SearchMode
        }, cancellationToken);
        
        return _mapper.Map<ListItemModel[]>(result);
    }
}