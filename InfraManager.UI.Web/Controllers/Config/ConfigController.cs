using InfraManager.BLL.Settings;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace InfraManager.Web.Controllers
{
    public class ConfigController : Controller
    {
        #region consts
        private long LOG_SIZE = 10 * 1024;
        private const string REDIRECT_AFTER_LOGIN_POST = "redirectAfterLoginPost";
        #endregion

        #region constructor

        private readonly IServiceProvider _serviceProvider;
        private readonly IUserLanguageChecker _userLanguageChecker;
        private readonly IWebUserSettingsBLL _webUserSettingsBLL;
        private readonly IConfiguration _configuration;

        public ConfigController(
            IServiceProvider serviceProvider,
            IUserLanguageChecker userLanguageChecker, IWebUserSettingsBLL webUserSettings,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _userLanguageChecker = userLanguageChecker;
            _webUserSettingsBLL = webUserSettings;
            _configuration = configuration;
        }
        #endregion

        #region method SetLanguage
        public async Task <ActionResult> SetLanguage(string cultureName)
        {
            Logger.Trace("ConfigController.Login post cultureName={0}", cultureName ?? string.Empty);
            //
            WebUserSettings webUser = new WebUserSettings();
            webUser.CultureName = cultureName;
            var auth = new AuthenticationHelper(HttpContext, null);
            if (ResourcesArea.Global.AvailableLanguages.Any(x => x == cultureName))
            {
                var userSettings = _serviceProvider.GetRequiredService<IWebUserSettingsBLL>();
                var settingsU = await userSettings.GetAsync(auth.CurrentUserID.Value);
                settingsU.IsDefault = true;
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(Int32.Parse(_configuration["CultureCookieTTL"]))
                    });
                await _webUserSettingsBLL.SetAsync(HttpContext.GetUserId(), webUser);
                await _userLanguageChecker.CheckAsync(HttpContext);
            }
            //
            return Redirect(Request.Headers["Referer"].ToString());
        }
        #endregion
    }
}
