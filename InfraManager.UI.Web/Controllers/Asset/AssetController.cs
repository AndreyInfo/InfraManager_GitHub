using InfraManager.BLL.Settings;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.Models.Asset;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.IM
{
    public class AssetController : BaseController
    {
        private readonly ISettingsBLL _settingsBLL;

        #region constructor
        public AssetController(ISettingsBLL settingsBLL)
        {
            _settingsBLL = settingsBLL;
        }
        #endregion

        #region method Table
#if Configuration
        public ActionResult Table()
        {
            Logger.Trace("AssetController.Table");

            ViewBag.Title = string.Format(Resources.TitleAssetList, Resources.MainLinkName);

            return View(
                new TableAvailabilityModel(
                    new SoftwareDistributionAvailabilityModel(false),
                    new SoftwareLicenceAvailabilityModel(false)));
        }

#endif
        #endregion

        #region method Settings
#if Settings

        public async Task<ActionResult> Settings()
        {
            Logger.Trace("AssetController.Settings");
            var user = base.CurrentUser;

            var isDistributionCenterTurnedOn = (await _settingsBLL.ConvertValueAsync(SystemSettings.DistributionCenterTurnedOn)) as bool?;

            var model = new SettingsAvailabilityModel(
                BLL.Assets.OrgStructure.OrgStructure.ModuleIsGranted(user.User),
                BLL.Assets.SoftwareDistributionCentres.SoftwareDistributionCentres.ModuleAvailable(user.User, isDistributionCenterTurnedOn)
                );

            //
            if (model.NoneAvailable)
                return Redirect(string.Format("~/Errors/Message?msg={0}", Resources.SettingsModuleNotAccessible));
            //
            ViewBag.Title = string.Format(Resources.TitleSettings, Resources.MainLinkName);
            return View(model);
        }

#endif
        #endregion

    }
}