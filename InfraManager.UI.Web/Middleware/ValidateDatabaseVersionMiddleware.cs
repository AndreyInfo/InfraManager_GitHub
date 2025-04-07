using InfraManager.Core.Logging;
using InfraManager.ResourcesArea;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Middleware
{
    public class ValidateDatabaseVersionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateDatabaseVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value.ToLower();

            if (!path.Contains("/errors/") 
                && !path.Contains("__browserlink/") 
                && !path.Contains("/api/configuration/"))
            {
                Logger.Trace("AuthenticationHelper.ValidateDatabaseVersionOrRedirect");
                //
                string message = null;
                string currentDBVersion = null;
                try
                {
                    currentDBVersion = InfraManager.Web.BLL.Global.GetDatabaseVersion();
                    Logger.Trace($"AuthenticationHelper.ValidateDatabaseVersionOrRedirect: {nameof(currentDBVersion)}:{currentDBVersion}");
                }
                catch (Exception e)
                {
                    Logger.Trace($"{e.Message}");
                    if(e.InnerException!=null)
                        Logger.Trace($"{e.InnerException.Message}: {e.InnerException}");
                    message = Resources.DBNotAvailable;
                }
                //
                if (currentDBVersion == null)
                    message = Resources.DBNotExists;
                else if (currentDBVersion == string.Empty)
                    message = Resources.DBInvalid;
                else if (currentDBVersion != Core.Global.CompatibleDBVersion)
                    message = Resources.DBVersionNotCompatible;
                //
                if (message != null)
                {
                    httpContext.RedirectOnError(message);
                    return;
                }
                else
                    InfraManager.Web.BLL.Helpers.ImageHelper.MakeIconsCache();
            }

            await _next(httpContext);
        }
    }
}
