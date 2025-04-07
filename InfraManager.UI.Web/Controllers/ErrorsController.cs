using InfraManager.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers
{
    //all methods names uses in Global.asax
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        #region method Default
        public ActionResult Default(Exception exception = null, int errorCode = 0)
        {
            Logger.Trace("ErrorsController.Default errorCode={0}, exception.Message={1}",
                errorCode,
                exception == null ? string.Empty : exception.Message);
            //
            ViewBag.Title = string.Format(Resources.TitleError, Resources.MainLinkName);
            ViewBag.LastErrorMessage = exception == null ? string.Empty : exception.ToString();
            ViewBag.ErrorCode = errorCode == 0 ? "?" : errorCode.ToString();
            ViewBag.IsDevelopment = false;
#if DEBUG
            ViewBag.IsDevelopment = true;
#endif
            //
            return View();
        }
        #endregion

        #region method PageNotFound
        public ActionResult PageNotFound()
        {
            Logger.Trace("ErrorsController.PageNotFound");
            //
            ViewBag.Title = string.Format(Resources.TitlePageNotFound, Resources.MainLinkName);
            //
            return View();
        }
        #endregion

        #region method BrowserNotSupported
        public ActionResult BrowserNotSupported()
        {
            Logger.Trace("ErrorsController.BrowserNotSupported");
            //
            return View();
        }
        #endregion

        #region method Message
        
        public ActionResult Message(string msg)
        {
            Logger.Trace("ErrorsController.Message message={0}", msg);
            //
            ViewBag.Title = string.Format(Resources.TitleMessage, Resources.MainLinkName);
            ViewBag.Message = msg;
            //
            return View();
        }
        #endregion
    }
}
