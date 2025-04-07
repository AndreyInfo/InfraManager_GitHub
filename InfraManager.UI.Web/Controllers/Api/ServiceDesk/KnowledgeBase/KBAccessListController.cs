using InfraManager.BLL.ServiceDesk;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.BLL.KnowledgeBase;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.KnowledgeBase
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KBAccessListController : BaseApiController
    {
        private readonly IKnowledgeBaseAccessBLL _knowledgeBaseAccessBLL;
        public KBAccessListController(IKnowledgeBaseAccessBLL knowledgeBaseAccessBLL)
        {
            _knowledgeBaseAccessBLL = knowledgeBaseAccessBLL;
        }

        [HttpGet]
        public async Task<KBArticleAccessListModel[]> GetListAsync([FromQuery] KBArticleAccessListFilter filter, CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseAccessBLL.GetListAsync(filter, cancellationToken);
        }

        //sdApi/RemoveAccessObject
        [HttpDelete]
        public async Task<OkResult> DeleteAsync([FromBody] KBArticleAccessListModel model, CancellationToken cancellationToken = default)
        {
            await _knowledgeBaseAccessBLL.DeleteAsync(model, cancellationToken);
            return Ok();
        }

        //sdApi/AddAdmittedUsers
        //sdApi/AddAdmittedQueue
        //sdApi/AddAdmittedSubDivision
        //sdApi/AddAdmittedOrganization
        [HttpPost]
        public async Task<OkResult> PostAsync([FromBody] KBArticleAccessListModel model, CancellationToken cancellationToken = default)
        {
            await _knowledgeBaseAccessBLL.AddAsync(model, cancellationToken);
            return Ok();
        }
    }
}
