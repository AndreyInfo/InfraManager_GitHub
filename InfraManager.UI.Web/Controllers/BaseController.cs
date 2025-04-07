using InfraManager.UI.Web.Helpers;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InfraManager.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private AuthenticationHelper _authHelper;
        private IHubContext<EventHub> _hubContext;
        public AuthenticationHelper AuthHelper => _authHelper ?? (_authHelper = new AuthenticationHelper(HttpContext, _hubContext));

        protected ApplicationUser CurrentUser => AuthHelper.CurrentUser;
    }
}
