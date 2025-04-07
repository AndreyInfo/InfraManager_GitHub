using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [Route("api/ActiveDirectory/UIADPath")]
    [ApiController]
    [Authorize]
    public class UIADPathsController : ControllerBase
    {
        private readonly IUIADPathsBLL _bll;

        public UIADPathsController(IUIADPathsBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{id}")]
        public Task<UIADPathsOutputDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _bll.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIADPathsOutputDetails[]> GetAsync([FromQuery] UIADPathsFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIADPathsOutputDetails> PostAsync([FromBody] UIADPathsDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<UIADPathsOutputDetails> PutAsync(Guid id, [FromBody] UIADPathsDetails data,
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