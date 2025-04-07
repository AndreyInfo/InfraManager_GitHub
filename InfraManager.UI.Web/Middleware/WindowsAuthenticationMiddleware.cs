using InfraManager.UI.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Middleware
{
    public class WindowsAuthenticationMiddleware
    {
        public const int WINDOWS_AUTO_LOGON_STATUS_CODE = 418;

        private readonly RequestDelegate _next;

        public WindowsAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!AuthenticationHelper.IsAutoLogonEnabled(httpContext) ||
                httpContext.User.Identity.IsAuthenticated)
            {
                await _next(httpContext);
                return;
            }

            //
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.StatusCode = 401;
            }
            else
                httpContext.Response.Redirect("Account/WindowsAuthenticate");
        }
    }
}
