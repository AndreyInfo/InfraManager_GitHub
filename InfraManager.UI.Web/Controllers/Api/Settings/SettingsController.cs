using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.Settings;
using InfraManager.WebApi.Contracts.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Settings;

[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ISettingsBLL _settingsBLL;

    public SettingsController(ISettingsBLL settingsBLL)
    {
        _settingsBLL = settingsBLL;
    }

    [Obsolete("GET api/settings/webserveraddress")]
    [HttpGet("server/address")]
    public async Task<string> GetServerAddressAsync(CancellationToken cancellationToken = default)
    {
        var address = await _settingsBLL.ConvertValueAsync(SystemSettings.WebServerAddress, cancellationToken) as string;
        
        return !string.IsNullOrEmpty(address) ? address: HttpContext.Request.Host.Value;
    }

    [HttpGet("{setting}")]
    public async Task<SettingDetails> GetAsync(SystemSettings setting, CancellationToken cancellationToken = default)
    {
        return await _settingsBLL.GetAsync(setting, cancellationToken);
    }

    [HttpPut("{setting}")]
    public async Task<SettingDetails> PutAsync(SystemSettings setting, [FromBody]SettingData data, CancellationToken cancellationToken = default)
    {
        return await _settingsBLL.SetAsync(setting, data, cancellationToken);
    }
}
