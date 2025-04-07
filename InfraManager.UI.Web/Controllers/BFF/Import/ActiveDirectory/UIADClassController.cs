using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [Route("api/ActiveDirectory/UIADClass")]
    [ApiController]
    [Authorize]
    public class UIADClassController : ControllerBase
    {
        private readonly IUIADClassBLL _bll;

        public UIADClassController(IUIADClassBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{id}")]
        public Task<UIADClassOutputDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _bll.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIADClassOutputDetails[]> GetAsync([FromQuery] UIADClassFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIADClassOutputDetails> PostAsync([FromBody] UIADClassDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<UIADClassOutputDetails> PutAsync(Guid id, [FromBody] UIADClassDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.UpdateAsync(id, data, cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _bll.DeleteAsync(id, cancellationToken);
        }
    }
}