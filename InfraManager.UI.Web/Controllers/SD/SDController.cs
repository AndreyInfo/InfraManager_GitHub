using InfraManager.Core.Logging;
using InfraManager.UI.Web.Models.Asset;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.SD
{
    public sealed class SDController : BaseController
    {
        #region constructor
        public SDController()
        { }
        #endregion

        #region method Table

        [Authorize]
        public ActionResult Table()
        {
            Logger.Trace("SDController.Table");
            //
            /*var user = base.CurrentUser;
            var model = CreateTableAvailabilityModel(user);*/
            ViewBag.Title = string.Format(Resources.TitleMyWorkplace, Resources.MainLinkName);
            return View(
                new TableAvailabilityModel(
                    new SoftwareDistributionAvailabilityModel(false), 
                    new SoftwareLicenceAvailabilityModel(false)));
        }
        
        private static TableAvailabilityModel CreateTableAvailabilityModel(ApplicationUser user)
        {
            var softwareDistributionAvailabilityModel =
                new SoftwareDistributionAvailabilityModel(
                    BLL.Assets.SoftwareDistributionCentres.SoftwareDistributionCentres.ModuleAvailable(user.User));

            var softwareLicenceAvailabilityModel =
                new SoftwareLicenceAvailabilityModel(BLL.Assets.Licence.SoftwareLicence.ModuleAvailable(user.User));

            var model = new TableAvailabilityModel(softwareDistributionAvailabilityModel,
                softwareLicenceAvailabilityModel);

            return model;
        }
        #endregion

        #region method ServiceCatalogue
        public ActionResult ServiceCatalogue()
        {
            Logger.Trace("SDController.ServiceCatalogue");
            // 
            ViewBag.Title = string.Format(Resources.TitleServiceCatalogue, Resources.MainLinkName);
            return View();
        }
        #endregion

        #region method KBArticleSearch
        public ActionResult KBArticleSearch()
        {
            Logger.Trace("SDController.KBArticleSearch");
            // 
            ViewBag.Title = string.Format(Resources.TitleKBASearch, Resources.MainLinkName);
            return View();
        }
        #endregion

        #region method DashboardSearch
        public ActionResult DashboardSearch()
        {
            Logger.Trace("SDController.DashboardSearch");
            var user = base.CurrentUser;
            //
            if (!user.User.HasRoles)
                return Redirect(string.Format("~/Errors/Message?msg={0}", Resources.DashboardModuleNotAccessibleInClientMode));
            //
            ViewBag.Title = string.Format(Resources.TitleDashboardSearch, Resources.MainLinkName);
            return View();
        }
        #endregion

        #region method TimeManagement
#if TimeManagement
        public ActionResult TimeManagement()
        {
            Logger.Trace("SDController.TimeManagement");
            var user = base.CurrentUser;
            //
            if (!BLL.TimeManagement.TimeManagement.ModuleIsGranted(user.User))
                return Redirect(string.Format("~/Errors/Message?msg={0}", Resources.TimeManagementModuleNotAccessible));
            //
            ViewBag.Title = string.Format(Resources.TitleTimeManagement, Resources.MainLinkName);
            return View();
        }
#endif
        #endregion
    }
}