using InfraManager.BLL.Settings;
using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.DAL;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.Web.Controllers.Config
{
    public class ConfigApiController : ControllerBase
    {
        private readonly IHubContext<EventHub> _hubContext;
        private readonly IAppSettingsBLL _appSettingsBLL;
        
        public ConfigApiController(IHubContext<EventHub> hubContext,
         IAppSettingsBLL appSettingsBLL)
        {
            _appSettingsBLL = appSettingsBLL;
        }

        #region method ClearServerLog
        [HttpPost]
        //[ConfigAuthorize]
        [AcceptVerbs("POST")]
        [Route("configApi/ClearServerLog", Name = "ClearServerLog")]
        public bool ClearServerLog()
        {
            Logger.Trace("ConfigApiController.ClearServerLog");
            //
            string webLog = string.Empty;
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web.log");
                if (System.IO.File.Exists(path))
                {
                    var attr = System.IO.File.GetAttributes(path);
                    if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        attr = attr & ~FileAttributes.ReadOnly;
                        System.IO.File.SetAttributes(path, attr);
                    }
                    System.IO.File.WriteAllText(path, string.Empty);
                }
                //
                return true;
            }
            catch { }
            //
            return false;
        }
        #endregion

        #region method SignOut
        [HttpPost]
        //[ConfigAuthorize]
        [AcceptVerbs("POST")]
        [Route("configApi/SignOut", Name = "ConfigSignOut")]
        public bool SignOut()
        {
            Logger.Trace("ConfigApiController.SignOut");
            //
            try
            {
                HttpContext.Response.Cookies.Delete("authCookie");
                return true;
            }
            catch { }
            //
            return false;
        }
        #endregion

        #region method IsHardToChooseButtonVisibile
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("configApi/IsHardToChooseButtonVisibile", Name = "IsHardToChooseButtonVisibile")]
        public async Task<bool> IsHardToChooseButtonVisibile()
        {
            Logger.Trace("ConfigApiController.IsHardToChooseButtonVisibile");
            //
            var config = await _appSettingsBLL.GetConfigurationAsync(false, CancellationToken.None);
            return config.WebSettings.HardToChooseButtonVisible;
        }
        #endregion

        #region method IsHardToChooseButtonVisibile
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("configApi/IsNotAvailableServiceBySlaVisible", Name = "IsNotAvailableServiceBySlaVisible")]
        public async Task<bool> IsNotAvailableServiceBySlaVisible()
        {
            Logger.Trace("ConfigApiController.IsNotAvailableServiceBySlaVisible");
            //
            var config = await _appSettingsBLL.GetConfigurationAsync(false, CancellationToken.None);
            return config.WebSettings.VisibleNotAvailableServiceBySla;
        }
        #endregion

        #region method IsHardToChooseButtonVisibile
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("configApi/IsHidePlaceOfServiceVisible", Name = "IsHidePlaceOfServiceVisible")]
        public bool IsHidePlaceOfServiceVisible()
        {
            return true;
            Logger.Trace("ConfigApiController.IsHidePlaceOfServiceVisible");
            //
            return InfraManager.IM.BusinessLayer.Settings.Setting.Get(InfraManager.IM.BusinessLayer.Settings.SettingType.CallHidePlaceOfService).GetBoolean();
        }
        #endregion

        #region method WorkOrderRegistration_UseTypeSelectionDialog
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("configApi/WorkOrderRegistration_UseTypeSelectionDialog", Name = "WorkOrderRegistration_UseTypeSelectionDialog")]
        public async Task<bool> WorkOrderRegistration_UseTypeSelectionDialog()
        {
            Logger.Trace("ConfigApiController.WorkOrderRegistration_UseTypeSelectionDialog");
            //
            var config = await _appSettingsBLL.GetConfigurationAsync(false, CancellationToken.None);
            return config.WebSettings.WorkOrderRegistrationUseTypeSelectionDialog;
        }
        #endregion

        #region method GetImagePath
        [HttpGet]
        [Route("configApi/GetImagePath", Name = "GetImagePath")]
        public async Task<string> GetImagePath(string name)
        {
            Logger.Trace("ConfigApiController.GetImagePath name={0}", name);
            //
            var settings = await _appSettingsBLL.GetConfigurationAsync(false, CancellationToken.None);
            switch (name)
            {
                case "browser":
                    return GetNotNullOrEmptyString(settings.WebSettings.ImagePathBrowser, "/images/favicon.ico");
                case "menu":
                    return GetNotNullOrEmptyString(settings.WebSettings.ImagePathMenu, "/images/logo.png");
                case "login":
                    return GetNotNullOrEmptyString(settings.WebSettings.ImagePathLogin, "/images/logo_big.png");
                default:
                    {
                        Logger.Trace("ConfigApiController.GetImagePath name={0} not supported.", name);
                        return null;
                    }
            }
        }

        private string GetNotNullOrEmptyString(string @string, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(@string))
            {
                return defaultValue;
            }
            
            return @string;
        }
        
        #endregion

        #region method IsReSignAvailable
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("configApi/isReSignAvailable", Name = "IsReSignAvailable")]
        public async Task<bool> IsReSignAvailable()
        {
            Logger.Trace("ConfigApiController.IsReSignAvailable");
            //
            var settings = await _appSettingsBLL.GetConfigurationAsync(false, CancellationToken.None);
            return settings.WebSettings.LoginPasswordAuthentication;
        }
        #endregion

        #region method ManhoursSettings
        public sealed class ManhoursSettingsOutModel
        {
            public BLL.SD.Manhours.ManhoursShowMode ShowMode { get; set; }
            public bool AllowInClosed { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("configApi/manhoursSettings", Name = "ManhoursSettings")]
        public ManhoursSettingsOutModel ManhoursShowMode()
        {
            Logger.Trace("ConfigApiController.ManhoursSettings");
            //
            using (var dataSource = DataSource.GetDataSource())
            {
                var retval = new ManhoursSettingsOutModel();
                retval.ShowMode = BLL.SD.Manhours.ManhoursWork.GetManhoursShowMode(dataSource);
                retval.AllowInClosed = BLL.SD.Manhours.ManhoursWork.GetManhoursAllowInClosed(dataSource);
                //
                return retval;
            }
        }
        #endregion
    }
}