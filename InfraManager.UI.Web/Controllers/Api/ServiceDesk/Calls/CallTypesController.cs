using AutoMapper;
using InfraManager.BLL.ServiceDesk.Calls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.UI.Web.Models.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CallTypesController : ControllerBase
    {
        private readonly ICallTypeBLL _service;
        private readonly IMapper _mapper;

        public CallTypesController(ICallTypeBLL service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<CallTypeListItemModel[]> ListAsync([FromQuery] bool? visibleInWeb, [FromQuery] ClientPageFilter<CallType> page, CancellationToken cancellationToken = default)
        {
            var callTypes = await _service.GetDetailsPageAsync(
                new CallTypeListFilter
                {
                    VisibleInWeb = visibleInWeb,
                    OrderByProperty = page.OrderByProperty ?? nameof(CallType.ID),
                    Ascending = page.Ascending,
                    Take = page.Take,
                    Skip = page.Skip
                },
                cancellationToken);
            return callTypes.Select(dt => _mapper.Map<CallTypeListItemModel>(dt)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<CallTypeListItemModel> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var callType = await _service.DetailsAsync(id, cancellationToken);
            return _mapper.Map<CallTypeListItemModel>(callType);
        }

        [HttpPost]
        public async Task<CallTypeDetails> PostAsync(CallTypeData data, CancellationToken cancellationToken = default)
        {
            return await _service.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<CallTypeDetails> PutAsync([FromRoute] Guid id, CallTypeData data, CancellationToken cancellationToken = default)
        {
            return await _service.UpdateAsync(id, data, cancellationToken);
        }

        [HttpGet("image/{id}")]
        public async Task<FileResult> GetImageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var bytes = await _service.GetImageBytesAsync(id, cancellationToken);
            return File(bytes, "image/png", $"{id}.png");
        }
    }
}
