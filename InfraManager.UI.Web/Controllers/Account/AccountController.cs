using System.Threading;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;
using InfraManager.BLL.Settings;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.Account
{
    public class AccountController : BaseController
    {
        #region constructor

        private readonly IAppSettingsBLL _configuration;

        public AccountController(IAppSettingsBLL configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region method Authenticate
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate(string returnUrl)
        {//сюда попадаем, если не аутентифицированы, после Global.asax
            Logger.Trace("AccountController.Authenticate returnURL={0}", returnUrl);
            //            
            var config = await _configuration.GetConfigurationAsync(false, CancellationToken.None);
            //
            ViewBag.LoginPasswordAuthenticationEnabled = config.WebSettings.LoginPasswordAuthentication;
            ViewBag.Title = string.Format(Resources.TitleAuthenticate, Resources.MainLinkName);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        #endregion

        #region method WindowsAuthenticate
        [AllowAnonymous]
        [HttpPost]
        public ActionResult WindowsAuthenticate()//name used in WindowsAuthenticationHandler
        {
            Logger.Trace("AccountController.WindowsAuthenticate");
            //
            if (HttpContext.User.Identity == null || !HttpContext.User.Identity.IsAuthenticated)
                return Json(false);//for ajax success
            //
            var loginName = HttpContext.User.Identity.Name;
            if (string.IsNullOrWhiteSpace(loginName))
                return Json(false);//for ajax success
            //
            var user = AuthHelper.UserManager.FindAsync(new UserLoginInfo("Windows", loginName, loginName)).Result;
            if (user == null || !user.User.WebAccessIsGranted)
                return Json(false);//for ajax success
            //
            AuthHelper.SignInAsync(user).Wait();
            return Json(true);//for ajax success
        }
        #endregion

        #region method Profile
        public ActionResult ProfileSettings()
        {
            Logger.Trace("AccountController.ProfileSettings");
            //            
            ViewBag.Title = string.Format(Resources.TitleProfile, Resources.MainLinkName);
            ViewBag.UserID = HttpContext.GetUserId();
            return View();
        }
        #endregion

        #region method AdminTools
        public ActionResult AdminTools()
        {
            Logger.Trace("AccountController.AdminTools");
            var user = base.CurrentUser;
            //
            if (!user.User.HasAdminRole)
                return Redirect(string.Format("~/Errors/Message?msg={0}", Resources.UserNoRightsToWebAccess));
            //            
            ViewBag.Title = string.Format(Resources.TitleAdminTools, Resources.MainLinkName);
            return View();
        }
        #endregion
    }
}