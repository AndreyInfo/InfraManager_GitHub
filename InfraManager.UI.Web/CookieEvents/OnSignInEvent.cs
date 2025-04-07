using System;
using System.Linq;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;
using InfraManager.BLL.Sessions;
using InfraManager.BLL.Settings;
using InfraManager.Core.Extensions;
using InfraManager.DAL.Sessions;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace InfraManager.UI.Web.CookieEvents;

public class OnSignInEvent : CookieAuthenticationEvents
{
    private readonly ISystemSessionBLL _sessionBLL;
    private readonly ILogger<OnSignInEvent> _logger;
    private readonly IHubContext<EventHub> _hubContext;
    private readonly IMemoryCache _cache;
    private readonly ISettingsBLL _settings;
    private readonly IConvertSettingValue<int> _intConverter;
    private const int defaultCacheTime = 30;
    private LocationProvider _locationProvider;
    
    private AuthenticationHelper _authenticationHelper { get; set; }

    public OnSignInEvent(
        ISystemSessionBLL sessionBLL,
        ILogger<OnSignInEvent> logger,
        IHubContext<EventHub> hubContext,
        IMemoryCache cache,
        ISettingsBLL settings, 
        IConvertSettingValue<int> intConverter,
        LocationProvider locationProvider)
    {
        _sessionBLL = sessionBLL;
        _logger = logger;
        _hubContext = hubContext;
        _cache = cache;
        _settings = settings;
        _intConverter = intConverter;
        _locationProvider = locationProvider;
    }

    private string GetUserAgentWithIP(HttpContext httpContext)
    {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        var ip = httpContext.Connection.RemoteIpAddress;
        
        return $"{ip} ({ProcessUserAgentString(userAgent)})";
    }
    
    private string ProcessUserAgentString(string userAgent)
    {
        if (userAgent is null or "")
            return "WebService";
        if (userAgent.Contains("Firefox") && !userAgent.Contains("Seamonkey"))
            return "Firefox";
        if (userAgent.Contains("seamonkey"))
            return "Seamonkey";
        if (userAgent.Contains("Edge"))
            return "Edge";
        if (userAgent.Contains("Chrome") && !userAgent.Contains("Chromium"))
            return "Chrome";
        if (userAgent.Contains("Chromium"))
            return "Chromium";
        if (userAgent.Contains("Safari") && (!userAgent.Contains("Chrome") || !userAgent.Contains("Chromium")))
            return "Safari";
        if (userAgent.Contains("Opera") || userAgent.Contains("OPR/"))
            return "Opera";
        if (userAgent.Contains("; MSIE ") || userAgent.Contains("Trident/7.0;"))
            return "Internet Explorer";
        //
        return userAgent.Truncate(400);
    }

    private SessionLocationType GetLocationType(HttpContext httpContext)
    {
        var locationFrom = httpContext.Request.Headers["LocationFrom"].ToString();
        return _locationProvider.GetLocation(locationFrom);
    }
    
    public override async Task SigningIn(CookieSigningInContext context)
    {
        try
        {
            var httpContext = context.HttpContext;

            var securityStamp = context.Principal?.Claims.FirstOrDefault(x => x.Type == "SecurityStamp")?.Value;
            var userID = context.Principal?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            var userAgent = GetUserAgentWithIP(context.HttpContext);

            await _sessionBLL.CreateOrRestoreAsync(new Guid(userID), securityStamp, userAgent,
                GetLocationType(httpContext), httpContext.RequestAborted);
        }
        catch (Exception e)
        {
            _authenticationHelper = new AuthenticationHelper(context.HttpContext, _hubContext);
            
            _logger.LogError(e, "Error while creating or restoring user session");
            _authenticationHelper.SignOut();
        }
    }

    public override async Task SigningOut(CookieSigningOutContext context)
    {
        var userID = context.HttpContext.GetUserId();  

        var userAgent = GetUserAgentWithIP(context.HttpContext);

        await _sessionBLL.AbortAsync(userID, userAgent, ApplicationUser.GenerateNewSecurityStamp(),
            context.HttpContext.RequestAborted);
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userID = context.Principal?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        var userAgent = GetUserAgentWithIP(context.HttpContext);
        var securityStamp = context.Principal?.Claims.FirstOrDefault(x => x.Type == "SecurityStamp")?.Value;

        if (_cache.TryGetValue(securityStamp, out bool extended))
        {
            return;
        }
        
        try
        {
            var cacheSessionCheckTime = _intConverter.Convert(await _settings.GetValueAsync(
                SystemSettings.CacheSessionCheckTime,
                context.HttpContext.RequestAborted));

            if (cacheSessionCheckTime <= 0)
            {
                cacheSessionCheckTime = defaultCacheTime;
            }

            await _sessionBLL.ExtendAsync(new Guid(userID), userAgent, securityStamp,
                context.HttpContext.RequestAborted);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheSessionCheckTime));

            _cache.Set(securityStamp, true, cacheEntryOptions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cant validate Principal or extend active session");
            context.RejectPrincipal();
            _logger.LogInformation($"Principals was rejected for user with ID = {userID}");
        } 
    }
}