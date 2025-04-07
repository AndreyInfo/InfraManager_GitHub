using InfraManager.BLL;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Users;
using InfraManager.DAL;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Helpers
{
    internal class UserLanguageChecker : IUserLanguageChecker
    {
        private readonly IServiceProvider _serviceProvider;

        public UserLanguageChecker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task CheckAsync(HttpContext httpContext)
        {
            string cultureName = httpContext.GetSupportedBrowserCultureName();
            bool userIdentified = false;
            var auth = new AuthenticationHelper(httpContext, null);

            if (httpContext.IsAuthenticated())
            {
                // TODO: Костыль, чтобы поддержать авторизацию от имени системного пользователя от других сервисов (решения покаа нет)             
                if (auth.CurrentUserID == User.SystemUserGlobalIdentifier)
                {
                    userIdentified = true;
                }
                else
                {
                    try
                    {
                        var user = await _serviceProvider
                            .GetService<IUserBLL>()
                            .DetailsAsync(auth.CurrentUserID.Value);
                        userIdentified = true;
                    }
                    catch (ObjectNotFoundException)
                    {
                        auth.SignOut();
                    }
                }
            }

            if (userIdentified)
            {
                var userSettings = _serviceProvider.GetRequiredService<IWebUserSettingsBLL>();
                var settings = await userSettings.GetAsync(auth.CurrentUserID.Value);

                cultureName = settings?.IsDefault == false
                    ? settings.CultureName
                    : (cultureName ?? settings?.CultureName);
            }

            httpContext.SetCurrentCulture(cultureName);

            if (cultureName != CultureInfo.CurrentUICulture.Name)
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(cultureName);
            }
        }
    }
}
