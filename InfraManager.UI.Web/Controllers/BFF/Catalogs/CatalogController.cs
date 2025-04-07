using InfraManager.Core.Logging;
using Resources = InfraManager.ResourcesArea.Resources;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.Catalogs
{
    public class CatalogController : BaseController
    {

        #region constructor
        public CatalogController()
        { }
        #endregion

        #region method Table
        public ActionResult Table()
        {
            Logger.Trace("CatalogController.Table");
            var user = base.CurrentUser;
            //
            if (!user.User.HasRoles)
                return Redirect(string.Format("~/Errors/Message?msg={0}", Resources.UserNoRightsToWebAccess));
            //
            ViewBag.Title = string.Format(Resources.TitleCatalog, Resources.MainLinkName);
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }
        #endregion
    }
}