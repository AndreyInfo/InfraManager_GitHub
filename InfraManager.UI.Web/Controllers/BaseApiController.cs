using InfraManager.Core.Logging;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.BLL.Settings;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace InfraManager.Web.Controllers
{
    [Authorize]
    [Obsolete]
    public abstract class BaseApiController : ControllerBase
    {
        #region properties

        protected IHubContext<EventHub> _hubContext;
        private AuthenticationHelper _authHelper;
        protected AuthenticationHelper AuthHelper => _authHelper ?? (_authHelper = new AuthenticationHelper(HttpContext, _hubContext));

        protected ApplicationUser CurrentUser
        {
            get
            {
                return AuthHelper.CurrentUser;
            }
        }

        #endregion


        protected List<DTL.Settings.ColumnSettings> GetColumnSettingsListInner(string listName, string caller)
        {
            var user = CurrentUser;
            if (string.IsNullOrWhiteSpace(listName))
                return null;
            //
            //Logger.Trace($"{caller} userID={0}, userName={1}, listName={2}", user.Id, user.UserName, listName);
            //
            try
            {
                var settings = ColumnSettings.TryGetOrCreate(user.User, listName);
                return settings == null ? null : settings.ColumnsDTL;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения настроек столбцов.");
                return null;
            }
        }

    }
}