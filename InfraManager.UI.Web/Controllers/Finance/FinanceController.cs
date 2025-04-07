using InfraManager.Core.Logging;
using InfraManager.UI.Web.Models.Asset;
using InfraManager.Web.Controllers.IM;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.Finance
{
    public class FinanceController : BaseController
    {
        #region constructor

        public FinanceController()
        {
        }

        #endregion

        #region method Table

#if Purchase
        public ActionResult Table()
        {
            Logger.Trace("FinanceController.Table");
            var user = base.CurrentUser;
            //
            if (!user.User.HasRoles)
                return Redirect(string.Format("~/Errors/Message?msg={0}", Resources.UserNoRightsToWebAccess));
            //
            ViewBag.Title = string.Format(Resources.TitleFinance, Resources.MainLinkName);

            var model = CreateTableAvailabilityModel(user);
            return View(model);
        }
#endif
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
    }
}