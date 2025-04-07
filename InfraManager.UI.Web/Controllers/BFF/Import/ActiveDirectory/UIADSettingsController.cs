using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager.BLL.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.BFF.Import
{
    [Route("api/ActiveDirectory/UIADSetting")]
    [ApiController]
    [Authorize]
    public class UIADSettingsController : ControllerBase
    {
        private readonly IUIADSettingsBLL _bll;

        public UIADSettingsController(IUIADSettingsBLL bll)
        {
            _bll = bll;
        }


        [HttpGet("{id}")]
        public Task<UIADSettingsOutputDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _bll.DetailsAsync(id, cancellationToken);
        }

        [HttpGet]
        public Task<UIADSettingsOutputDetails[]> GetAsync([FromQuery] UIADSettingsFilter filter,
            CancellationToken cancellationToken = default)
        {
            return _bll.GetDetailsArrayAsync(filter, cancellationToken);
        }

        [HttpPost]
        public Task<UIADSettingsOutputDetails> PostAsync([FromBody] UIADSettingsDetails data,
            CancellationToken cancellationToken = default)
        {
            return _bll.AddAsync(data, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<UIADSettingsOutputDetails> PutAsync(Guid id, [FromBody] UIADSettingsDetails data,
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