using AutoMapper;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.WebApi.Contracts.OrganizationStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.OrganizationStructure
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeputiesController : ControllerBase
    {
        private readonly IDeputyUserBLL _deputyService;
        private readonly IMapper _mapper;

        public DeputiesController(IDeputyUserBLL deputyService, IMapper mapper)
        {
            _deputyService = deputyService;

            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<BaseDeputyUserListItem[]> GetAsync([FromQuery] DeputyUserListFilter filterBy, CancellationToken cancellationToken = default)
        {
            var models = await _deputyService.GetDetailsPageAsync(filterBy, filterBy, cancellationToken);

            return filterBy.DeputyMode == DeputyMode.Deputy 
                ? models.Select(x => _mapper.Map<DeputyUserListItem>(x)).ToArray()
                : models.Select(x => _mapper.Map<IDeputyForUserListItem>(x)).ToArray();
        }

        [HttpGet("{id}")]
        public async Task<DeputyUserDetailsModel> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var model = await _deputyService.DetailsAsync(id, cancellationToken);

            return _mapper.Map<DeputyUserDetailsModel>(model);
        }

        [HttpPost()]
        public async Task<DeputyUserDetailsModel> PostAsync([FromBody] DeputyUserData data, CancellationToken cancellationToken = default)
        {
            var newModel = await _deputyService.AddAsync(data, cancellationToken);

            return _mapper.Map<DeputyUserDetailsModel>(newModel);
        }

        [HttpPut("{id}")]
        public async Task<DeputyUserDetailsModel> PutAsync([FromRoute] Guid Id, [FromBody] DeputyUserData data, CancellationToken cancellationToken = default)
        {
            var model = await _deputyService.UpdateAsync(Id, data, cancellationToken);

            return _mapper.Map<DeputyUserDetailsModel>(model);
        }

        [HttpDelete("{id}")]
        public async Task DeleteDeputy([FromRoute] Guid Id, CancellationToken cancellationToken = default)
        {
            await _deputyService.DeleteAsync(Id, cancellationToken);
        }
    }
}