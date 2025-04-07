using IM.Core.DAL.PGSQL;
using InfraManager.BLL;
using InfraManager.BLL.Settings;
using InfraManager.Core.Extensions;
using InfraManager.Core.Logging;
using InfraManager.CrossPlatform.WebApi.Contracts.Auth;
using InfraManager.CrossPlatform.WebApi.Infrastructure;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using InfraManager.DAL.Microsoft.SqlServer;
using InfraManager.UI.Web;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using IM.Core.HttpInfrastructure;

namespace InfraManager.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public const string UserLoginKey = "userLogin";
        public const string UserHashKey = "userHash";
        public const string UserPasswordKey = "userPassword";
        public const string EmailKey = "email";
        public const string DefaultCultureCookie = "defaultCulture";
        public const string ConfigurationAuthenticationCookie = "authCookie";
        public const string ConextCultureItem = "currentCulture";

        private static object _locker = new object();

        public static void RedirectOnError(this HttpContext httpContext, string message)
        {
            httpContext.Response.Redirect("/Errors/Message?msg=" + Uri.EscapeDataString(message));
        }

        public static string GetUserLogin(this IQueryCollection query)
        {
            return query[UserLoginKey];
        }

        public static string GetUserLogin(this IFormCollection query)
        {
            return query[UserLoginKey];
        }

        public static string GetUserPassword(this IQueryCollection query)
        {
            return query[UserPasswordKey];
        }

        public static string GetUserPassword(this IFormCollection query)
        {
            return query[UserPasswordKey];
        }

        public static string GetUserHash(this IQueryCollection query)
        {
            return query[UserHashKey];
        }

        public static string GetUserHash(this IFormCollection query)
        {
            return query[UserHashKey];
        }

        public static string GetUserEmail(this IQueryCollection query)
        {
            return query[EmailKey];
        }

        public static string GetUserEmail(this IFormCollection query)
        {
            return query[EmailKey];
        }

        public static BLL.Users.User ValidateLoginOrRedirect(this HttpContext httpContext, string loginName)
        {
            Logger.Trace("AuthenticationHelper.ValidateLoginOrRedirect loginName={0}", loginName);
            //
            var user = AuthenticationHelper.FindUserByLogin(loginName);
            if (user == null)
                httpContext.RedirectOnError(Resources.UserLoginNotFound);
            else if (!user.WebAccessIsGranted)
                httpContext.RedirectOnError(Resources.UserNoRightsToWebAccess);
            //
            return user;
        }

        public static void ValidateLoginPasswordOrRedirect(this HttpContext httpContext, BLL.Users.User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            //
            if (!AuthenticationHelper.ValidateLoginPassword(user, password))
                httpContext.RedirectOnError(Resources.UserLoginPasswordNotFound);
        }

        public static string GetSupportedBrowserCultureName(this HttpContext httpContext)
        {
            try
            {
                //get from cookie
                string defaultCultureName = httpContext.GetDefaultCultureName();
                if (!string.IsNullOrWhiteSpace(defaultCultureName) &&
                    ResourcesArea.Global.AvailableLanguages.Any(x => x == defaultCultureName))
                {
                    Logger.Trace("AuthenticationHelper.GetSupportedBrowserCultureName: get culture from cookie '{0}'", defaultCultureName);
                    return defaultCultureName;
                }
                //
                //get from request
                var browserLanguages = httpContext.Request.Headers["Accept-Language"].ToArray();
                if (browserLanguages == null || browserLanguages.Length == 0)
                {
                    Logger.Trace("AuthenticationHelper.GetSupportedBrowserCultureName: browser languages empty.");
                    return BLL.Global.EN;
                }
                var userLanguages = browserLanguages.
                    Where(x => ResourcesArea.Global.AvailableLanguages.Any(y => x.Contains(y))).
                    Select(x =>
                    {
                        if(x.Contains(ResourcesArea.Global.RU))
                            return BLL.Global.RU;
                        else
                            return BLL.Global.EN;
                    });
                if (userLanguages.Count() == 0)
                {
                    Logger.Trace("AuthenticationHelper.GetSupportedBrowserCultureName: supported languages not found.");
                    return BLL.Global.EN;
                }
                //
                try
                {
                    var ci = new System.Globalization.CultureInfo(userLanguages.First());
                    Logger.Trace("AuthenticationHelper.GetSupportedBrowserCultureName: language was found '{0}'.", ci.Name);
                    return ci.Name;
                }
                catch (System.Globalization.CultureNotFoundException)
                {
                    Logger.Trace("AuthenticationHelper.GetSupportedBrowserCultureName: language not exists '{0}'.", userLanguages.First());
                    return BLL.Global.EN;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения языка пользователя из запроса (браузера)");
                return BLL.Global.EN;
            }
        }

        private static string GetDefaultCultureName(this HttpContext httpContext)
        {
            Logger.Trace("AuthenticationHelper.GetDefaultCultureName");

            return httpContext.Request.Cookies.ContainsKey(DefaultCultureCookie)
                ? httpContext.Request.Cookies[DefaultCultureCookie]
                : null;
        }

        public static void ValidateLoginHashOrRedirect(this HttpContext httpContext, BLL.Users.User user, string hash)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            //
            Logger.Trace("AuthenticationHelper.ValidateLoginHashOrRedirect userName={0}, hash={1}", user.FullName, hash);
            //
            string hash2 = CalculateHash(user);
            if (hash != hash2)
                httpContext.RedirectOnError(Resources.UserLoginHashNotFound);
        }

        private static string CalculateHash(BLL.Users.User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            //
            Logger.Trace("AuthenticationHelper.CalculateHash user={0}", user);
            //
            string hashData = string.Concat(user.Family, user.Name, user.Patronymic, user.Email);
            byte[] hashBytes = Encoding.GetEncoding(1251).GetBytes(hashData);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(hashBytes);
            //
            StringBuilder sb = new StringBuilder();
            foreach (byte @byte in hash)
                sb.Append(@byte.ToString("x2"));
            //
            return sb.ToString().ToLower();
        }

        public static BLL.Users.User ValidateEmailOrRedirect(this HttpContext httpContext, string email)
        {
            Logger.Trace("AuthenticationHelper.ValidateEmailOrRedirect email={0}", email);
            //
            BLL.Users.User user = null;
            try
            {
                user = BLL.Users.User.GetByEmail(email);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения пользователя по имейлу.");
            }
            if (user == null)
                httpContext.RedirectOnError(Resources.UserByEmailNotFound);
            if (!user.WebAccessIsGranted)
                httpContext.RedirectOnError(Resources.UserNoRightsToWebAccess);
            //
            return user;
        }

        public static bool IsAuthenticated(this HttpContext httpContext)
        {
            return httpContext.User?.Identity?.IsAuthenticated ?? false;
        }

        private class StaticHttpContextAccess : IHttpContextAccessor
        {
            public HttpContext HttpContext { get; set; }
        }

        public static string GetRequestConnectionID(this HttpContext context)
        {            
            return context.Connection.Id;
        }

        public static void SetDefaultCultureName(this HttpContext httpContext, string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
                throw new ArgumentException("cultureName");
            //
            Logger.Trace("AuthenticationHelper.SetDefaultCultureName cultureName={0}", cultureName);
            //            
            httpContext.Response.Cookies.Append(DefaultCultureCookie, cultureName);
        }



        public static void SetConfigurationAuthCookie(this HttpContext httpContext, string login, string password)
        {
            var machineKeyConfig = new MachineKeyConfig
            {
                DecryptionKey = "9D030F7ADA2D6EE15F16F23D23F8F94C1274E0DF8C202FE78C2E095FEBD9209A",               
                ValidationKey = "D05769FBD11B856B91D7040BE2C4E57318DFA20D43B932F0F38BB1317911337ED859E9802BB5D1597CC880D3B7C88BD3CAB997A26E193183AA6993CA3A7B64B3"
            };
            var machine = new MachineKey(machineKeyConfig);
            if (login == null)
                login = string.Empty;
            if (password == null)
                password = string.Empty;
            //
            string concatedLoginPasswordHash = string.Concat(login, BLL.Settings.WebSettings.GetPasswordHash(password));
            var data = Encoding.ASCII.GetBytes(concatedLoginPasswordHash);
            var sData = machine.Protect(data);
            httpContext.Response.Cookies.Append(ConfigurationAuthenticationCookie, Convert.ToBase64String(sData));
        }

        public static void OnUserSessionChanged(this HttpContext httpContext, IHubContext<EventHub> hubContext, Guid userID, string customUserAgentID = null)
        {
            var request = httpContext.Request;
            var ua = string.IsNullOrWhiteSpace(customUserAgentID)
                ? BLL.Users.Session.GetRequestUserAgent(request.Headers["REMOTE_HOST"], request.Headers["User-Agent"].ToString())
                : customUserAgentID;
            if (hubContext != null)
                EventHub.UserSessionChanged(hubContext, userID, ua);
        }
        
        public static string GetUserAgent(this HttpContext httpContext)
        {
            var headerValue = httpContext.Request.Headers["User-Agent"].ToString();
            return AnalyzeUserAgentHeader(headerValue);            
        }

        private const int UserAgent_MaxLength = 400;

        private static string AnalyzeUserAgentHeader(string userAgentHeader)
        {
            if (string.IsNullOrWhiteSpace(userAgentHeader))
            {
                return "WebService";
            }

            if (userAgentHeader.Contains("Firefox") && !userAgentHeader.Contains("Seamonkey"))
            {
                return "Firefox";
            }

            if (userAgentHeader.Contains("seamonkey"))
            {
                return "Seamonkey";
            }

            if (userAgentHeader.Contains("Edge"))
            {
                return "Edge";
            }

            if (userAgentHeader.Contains("Chrome") && !userAgentHeader.Contains("Chromium"))
            {
                return "Chrome";
            }

            if (userAgentHeader.Contains("Chromium"))
            {
                return "Chromium";
            }

            if (userAgentHeader.Contains("Safari") && (!userAgentHeader.Contains("Chrome") || !userAgentHeader.Contains("Chromium")))
            {
                return "Safari";
            }

            if (userAgentHeader.Contains("Opera") || userAgentHeader.Contains("OPR/"))
            {
                return "Opera";
            }

            if (userAgentHeader.Contains("; MSIE ") || userAgentHeader.Contains("Trident/7.0;"))
            {
                return "Internet Explorer";
            }
            //
            return userAgentHeader.Truncate(UserAgent_MaxLength);
        }

        public static string GetHost(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString();
        }

        public static string GetCurrentCulture(this HttpContext httpContext)
        {
            lock(_locker)
                return httpContext.Items[ConextCultureItem]?.ToString() ?? "";
        }
        public static void SetCurrentCulture(this HttpContext httpContext, string cultureName)
        {
            lock(_locker)
                httpContext.Items[ConextCultureItem] = cultureName;
        }
    }
}
