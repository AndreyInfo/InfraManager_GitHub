using InfraManager.BLL.Asset;
using InfraManager.BLL.OrganizationStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.GroupQueue
{
    [Authorize]
    [ApiController]
    [Route("api/group/queue/")]
    public class GroupQueueController : Controller
    {
        private readonly IGroupBLL _groupBLL;
        private readonly IGroupQueueBLL _groupQueueBLL;
        public GroupQueueController(IGroupBLL groupBLL,
                                    IGroupQueueBLL groupQueueBLL)
        {
            _groupBLL = groupBLL;
            _groupQueueBLL = groupQueueBLL;
        }


        [HttpGet("{id:guid}")]
        public Task<GroupDetails> GetByIdAsync(Guid id, CancellationToken cancellationToken) 
            => _groupBLL.DetailsAsync(id, cancellationToken);

        [HttpGet("table")]
        public async Task<GroupDetails[]> GetTableAsync([FromQuery] GroupFilter filter, CancellationToken cancellationToken = default)
        {
            return await _groupQueueBLL.GetListAsync(filter, cancellationToken);
        }

        [HttpGet("list")]
        public async Task<GroupDetails[]> GetGroupsListAsync([FromQuery] string searchName, CancellationToken cancellationToken = default)
        {
            return await _groupBLL.GetListAsync(searchName, cancellationToken);
        }

        #region Вынести в отдельный контроллер
        [HttpGet("performers/list")]
        public async Task<GroupQueueUserDetails[]> GetPerformersByQueueIdAsync([FromQuery] string search,
                                                                                           Guid queueId, bool isPerformers = true
                                                                                           , CancellationToken cancellationToken = default)
        {
            return await _groupQueueBLL.GetPerformersAsync(queueId, isPerformers, search, cancellationToken);

        }

        [HttpGet("performers/item")]
        [Obsolete("Use api/users/id instead")]
        public async Task<GroupQueueUserDetails> GetPerformersByQueueIdAsync(Guid userId, Guid queueId, CancellationToken cancellationToken)
        {
            return await _groupQueueBLL.GetPerformerByIdAsync(userId, queueId, cancellationToken);
        }
        #endregion
    }
}
