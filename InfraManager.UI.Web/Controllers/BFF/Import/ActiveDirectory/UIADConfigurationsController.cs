using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [Route("api/ActiveDirectory/UIADConfiguration")]
    [ApiController]
    [Authorize]
    public class UIADConfigurationsController : ControllerBase
    {
        private readonly IUIADConfigurationsBLL _bll;

        public UIADConfigurationsController(IUIADConfigurationsBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{id}")]
        public Task<UIADConfigurationsOutputDetails> GetASync(Guid id, CancellationToken cancellationToken = default)
        {
            return _bll.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIADConfigurationsOutputDetails[]> GetAsync([FromQuery] UIADConfigurationsFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIADConfigurationsOutputDetails> PostAsync([FromBody] UIADConfigurationsDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<UIADConfigurationsOutputDetails> PutAsync(Guid id, [FromBody] UIADConfigurationsDetails data,
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