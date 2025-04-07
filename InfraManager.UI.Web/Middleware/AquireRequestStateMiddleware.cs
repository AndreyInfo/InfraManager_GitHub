using InfraManager.Core.Helpers;
using InfraManager.Core.Threading;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Middleware
{
    public class AquireRequestStateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHubContext<EventHub> _hubContext;

        public AquireRequestStateMiddleware(RequestDelegate next, IHubContext<EventHub> hubContext)
        {
            _next = next;
            _hubContext = hubContext;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUserLanguageChecker userLanguageChecker)
        {
            var path = httpContext.Request.Path.Value.ToLower();

            if (!path.Contains("/errors/")
                && !path.Contains("__browserlink/")
                && !path.Contains("configapi/"))
            {
                await userLanguageChecker.CheckAsync(httpContext);
                //
                //set CurrentUser.ID for current thread
                var authHelper = new AuthenticationHelper(httpContext, _hubContext);                
                var id = authHelper.CurrentUserID;
                var token = id.HasValue ? new Core.AuthenticationToken(id.Value, Core.AuthenticationType.ID) : null;
                httpContext.Items[ThreadContext.AuthenticationTokenSlot] = token;

                //set current connection for dataSource
                var connectionToken = httpContext.GetRequestConnectionID();

                if (connectionToken != null)
                    ThreadHelper.SetData(ThreadContext.ConnectionTokenSlot, connectionToken);
                //authHelper.TryExtendSession();
            }

            await _next(httpContext);
        }
    }
}
