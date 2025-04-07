using InfraManager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Users;
using InfraManager.UI.Web.Models;
using InfraManager.Web.BLL;
using InfraManager.Web.Controllers.Config;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;

namespace InfraManager.UI.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IUserBLL _users;
        private readonly IUserAccessBLL _userAccess;
        private readonly IWebUserSettingsBLL _settings;
        private readonly IHubContext<EventHub> _hubContext;
        private readonly IAppSettingsBLL _appSettingsBLL;

        public AccountsController(
            IUserBLL users, 
            IUserAccessBLL userAccess, 
            IWebUserSettingsBLL settings, 
            IHubContext<EventHub> hubContext,
            IAppSettingsBLL appSettingsBLL)
        {
            _users = users;
            _userAccess = userAccess;
            _settings = settings;
            _hubContext = hubContext;
            _appSettingsBLL = appSettingsBLL;
        }

        // TODO: [CONDITIONAL_COMPILATION]
        [HttpGet("my")]
        [AllowAnonymous]
        public async Task<AccountModel> GetCurrent(CancellationToken cancellationToken = default)
        {
            if (!HttpContext.IsAuthenticated())
            {
                return null;
            }

            var userID = HttpContext.GetUserId();

            var settings = await _settings.GetAsync(userID);
            var user = await _users.DetailsAsync(userID, cancellationToken);
            var grantedOperations = await _userAccess.GrantedOperationsAsync(userID);

            var timesheetManagementEnabled = false;

            if (Core.Global.TimeManagementEnabled)
            {
                timesheetManagementEnabled = await _userAccess.UserHasOperationAsync(userID, OperationID.Project_Properties)
                    || await _userAccess.UserHasOperationAsync(userID, OperationID.SD_General_Executor)
                    || await _userAccess.UserHasOperationAsync(userID, OperationID.SD_General_Owner);
            }

            var systemSettings = await _appSettingsBLL.GetConfigurationAsync(false, cancellationToken);
            
            return new AccountModel
            {
                UserID = userID.ToString(),
                UserFullName = user.FullName,
                UserPositionName = user.PositionName,
                IsReSignAvailable = systemSettings.WebSettings.LoginPasswordAuthentication,
                UserAgent = $"{HttpContext.GetHost()} ({HttpContext.GetUserAgent()})",
                CultureName = settings.CultureName,
                HasAdminRole = await _userAccess.HasAdminRoleAsync(userID),
                HasRoles = await _userAccess.HasRolesAsync(userID),
                GrantedOperations = grantedOperations.Cast<int>().ToArray(),
                IncomingCallProcessing = TelephonyWrapper.IsReady ? settings.IncomingCallProcessing : null,
                TimeManagementEnabled = timesheetManagementEnabled,
                BudgetEnabled = Global.IsBudgetEnabled,
                WebMobileEnabled = Global.IsWebMobileEnabled,
                AssetFiltrationObjectClassID = settings.AssetFiltrationObjectClassID ?? default,
                AssetFiltrationField = settings.AssetFiltrationField,
                AssetFiltrationObjectID = settings.AssetFiltrationObjectID,
                AssetFiltrationObjectName = settings.AssetFiltrationObjectName,
                AssetFiltrationTreeType = settings.AssetFiltrationTreeType ?? default,
                UseCompactMenuOnly = settings.UseCompactMenuOnly ?? false,
                FinanceBudgetID = settings.FinanceBudgetID,
                ListView_CompactMode = settings.ListView.CompactMode,
                ListView_GridLines = settings.ListView.GridLines,
                ListView_Multicolor = settings.ListView.Multicolor, 
                ViewNameAsset = settings.ViewNameAsset,
                ViewNameSD = settings.ViewNameSD,
                ViewNameFinance = settings.ViewNameFinance,
                IsDemo = Global.IsDemo
            };
        }
    }
}
