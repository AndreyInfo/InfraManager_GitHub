using InfraManager.BLL.Users;
using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.BLL.Authentication;
using InfraManager.UI.Web.Helpers;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.BLL.Settings;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.SD.ExpressionEditor;
using InfraManager.BLL.Settings;
using ColumnSettings = InfraManager.Web.BLL.Settings.ColumnSettings;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.Account
{
    public class AccountApiController : BaseApiController
    {
        #region constructors
        private readonly IUserBLL _users;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAppSettingsBLL _appSettings;

        public AccountApiController(IHubContext<EventHub> hubContext, 
            IUserBLL users, 
            ILogger<AccountApiController> logger, 
            IConfiguration configuration, 
            IAuthenticationService authenticationService,
            IAppSettingsBLL appSettings)
        {
            _hubContext = hubContext;
            _users = users;
            _logger = logger;
            _configuration = configuration;
            _authenticationService = authenticationService;
            _appSettings = appSettings;
        }
        #endregion

        #region method SignIn
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/SignIn", Name = "SignIn")]
        [AllowAnonymous]
        public async Task<SignInResult> SignIn([FromBodyOrForm] SignInData data, CancellationToken cancellationToken = default)
        {
            //нужно проверять наличие свободной сессии
            if (!ModelState.IsValid)
                return new SignInResult(false, string.Empty);
            //
            if (data == null || string.IsNullOrWhiteSpace(data.LoginName))
                return new SignInResult(false, string.Empty);
            //
            Logger.Trace("AccountApiController.SignIn login={0}", data.LoginName);
            //
            UserDetailsModel user = null;
            var settings = await _appSettings.GetConfigurationAsync(false, cancellationToken);

            try
            {
                //TODO придумать логику получше а не if
                if (settings.WebSettings.LdapAuthentication)
                {
                    if (_authenticationService.IsValid(data.LoginName, data.GetPasswordDecrypted()))
                    {
                       user = await _users.GetSignInUserAsync(data.LoginName, string.Empty, false, cancellationToken);
                    }
                }
                else if (settings.WebSettings.LoginAuthentication)
                {
                    user = await _users.GetSignInUserAsync(data.LoginName, string.Empty, false, cancellationToken);
                }
                else if(settings.WebSettings.LoginPasswordAuthentication)
                {
                    user = await _users.GetSignInUserAsync(data.LoginName, data.GetPasswordDecrypted(), true,
                        cancellationToken);
                }
            }
            catch (Exception error) 
            {
                _logger.LogError(error, $"Failed to sign in user {data.LoginName}");
                return new SignInResult(false, string.Empty);
            }

            if (user == null)
            {
                return new SignInResult(false, string.Empty);
            }    
            //
            if (user.WebAccessIsGranted)
            {
                await new AuthenticationHelper(HttpContext, _hubContext).SignInAsync(user);
                return new SignInResult(true, string.Empty);
            }

            return new SignInResult(
                false,
                string.Format(
                    "Errors/Message?msg={0}",
                    Uri.EscapeDataString(Resources.UserNoRightsToWebAccess)));
        }          
        #endregion

        #region method SignOut
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/SignOut", Name = "SignOut")]
        public bool SignOut()
        {
            if (!ModelState.IsValid)
                return false;            
            //            
            AuthHelper.SignOut();
            return true;
        }
        #endregion

        #region method GetCurrentUser
        [HttpGet]
        [Route("accountApi/GetCurrentUser", Name = "GetCurrentUser")]
        public BLL.Users.User GetCurrentUser()
        {
            var user = base.CurrentUser;
            if (user == null)
            {
                AuthHelper.SignOut();
                return null;
            }
            //
            Logger.Trace("AccountApiController.GetCurrentUser userID={0}, userName={1}", user.Id, user.UserName);
            //
            var retval = user.User;
            return retval;
        }
        #endregion

        #region method RestorePassword
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/RestorePassword", Name = "RestorePassword")]
        [AllowAnonymous]
        public string RestorePassword([FromForm]string value)
        {
            string msg;
            if (AuthHelper.RestorePassword(value, out msg) && base.CurrentUser != null)
                AuthHelper.SignOut();
            //
            return msg;
        }
        #endregion

        #region method ResetUserSettings
        [HttpPost]
        [Route("accountApi/ResetUserSettings", Name = "ResetUserSettings")]
        [Obsolete("POST null to api/userSettings instead")]
        public bool ResetUserSettings()
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("AccountApiController.ResetUserSettings userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                //настройка столбцов списка, текущего фильтра в списках, размера и положения окон
                UserSettings.Delete(user.User.ID);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка очистки настроек пользователя.");
                return false;
            }
        }
        #endregion


        #region method GetUserLanguage (not use)
        [HttpGet]
        [Route("accountApi/GetUserLanguage", Name = "GetUserLanguage")]
        public string GetUserLanguage()
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("AccountApiController.GetUserLanguage userID={0}, userName={1}", user.Id, user.UserName);
            //
            var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture);
            return settings == null ? null : settings.CultureName;
        }
        #endregion

        #region method SetUserLanguage
        [HttpPost]
        [Route("accountApi/SetUserLanguage", Name = "SetUserLanguage")]
        [AllowAnonymous]//for login page only
        [Obsolete("Post to api/userSettings instead. For login page - no need to do a server call to set public cookie.")]
        public bool SetUserLanguage([FromForm]string cultureName)
        {
            var user = base.CurrentUser;
            if (string.IsNullOrWhiteSpace(cultureName))
                return false;
            else if (!ResourcesArea.Global.AvailableLanguages.Any(x => x == cultureName))
            {
                Logger.Trace("AccountApiController.SetUserLanguage cultureName={0} failed (culture not supported)", cultureName);
                return false;
            }
            //
            HttpContext.SetDefaultCultureName(cultureName);
            if (user == null)//from login page, for example
                return true;
            //
            Logger.Trace("AccountApiController.SetUserLanguage userID={0}, userName={1}, cultureName={2}", user.Id, user.UserName, cultureName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, cultureName, dataSource);
                    if (settings == null)
                        return false;
                    //
                    if (settings.CultureName == cultureName)
                        return true;
                    //
                    settings.CultureName = cultureName;
                    settings.Save(dataSource);
                }
                //
                return true;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return false;
            }
        }
        #endregion

        #region method GetUserSettings
        [HttpGet]
        [Obsolete("Use api/userSettings")]
        [Route("accountApi/GetUserSettings", Name = "GetUserSettings")]
        public DTL.Settings.UserSettings GetUserSettings()
        {
            var user = base.CurrentUser;

            //
            Logger.Trace("AccountApiController.GetUserSettings userID={0}, userName={1}", user.Id, user.UserName);
            //
            var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture);
            return settings == null ? null : settings.DTL;
        }
        #endregion

        #region method GetUserSettings
        [HttpPost]
        [Obsolete("Use api/userSettings")]
        [Route("accountApi/SetUserSettings", Name = "SetUserSettings")]
        public RequestResponceType SetUserSettings([FromBodyOrForm] DTL.Settings.UserSettings value)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.GlobalError;
            if (value == null)
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("AccountApiController.SetUserSettings userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return RequestResponceType.BadParamsError;
                    //
                    settings.DTL = value;
                    settings.Save(dataSource);
                }
                //
                return RequestResponceType.Success;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return RequestResponceType.ValidationError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method SetViewName
        public sealed class SetViewNameInputModel
        {
            public string ViewName { get; set; }
            public BLL.Modules Module { get; set; }
        }
        [HttpPost]
        [Obsolete("Use api/userSettings")]
        [Route("accountApi/SetViewName", Name = "SetViewName")]
        public RequestResponceType SetViewName(SetViewNameInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null || string.IsNullOrWhiteSpace(model.ViewName))
                return RequestResponceType.NullParamsError;
            else if (!user.ActionIsGranted(model.Module, model.ViewName))
            {
                Logger.Warning("AccountApiController.SetViewName userID={0}, userName={1}, module={3}, viewName={2} failed (user is client)", user.Id, user.UserName, model.ViewName, model.Module);
                return RequestResponceType.AccessError;
            }
            //
            Logger.Trace("AccountApiController.SetViewName userID={0}, userName={1}, module={3}, viewName={2}", user.Id, user.UserName, model.ViewName, model.Module);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return RequestResponceType.NullParamsError;
                    //
                    if (model.Module == BLL.Modules.SD)
                    {
                        if (settings.ViewNameSD == model.ViewName)
                            return RequestResponceType.Success;
                        //
                        settings.ViewNameSD = model.ViewName;
                    }
                    //
                    if (model.Module == BLL.Modules.Asset)
                    {
                        if (settings.ViewNameAsset == model.ViewName)
                            return RequestResponceType.Success;
                        //
                        settings.ViewNameAsset = model.ViewName;
                    }
                    //
                    if (model.Module == BLL.Modules.Finance)
                    {
                        if (settings.ViewNameFinance == model.ViewName)
                            return RequestResponceType.Success;
                        else if (model.ViewName == BLL.Finance.Budget.FinanceBudgetRowForTable.VIEW_NAME && !BLL.Global.IsBudgetEnabled)
                            return RequestResponceType.BadParamsError;
                        //
                        settings.ViewNameFinance = model.ViewName;
                    }
                    settings.Save(dataSource);
                    return RequestResponceType.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return RequestResponceType.GlobalError;
            }
        }

        #endregion

        #region method SetTreeParams
        [HttpPost]
        [Route("accountApi/SetTreeParams", Name = "SetTreeParams")]
        public RequestResponceType SetTreeParams([FromBodyOrForm] DTL.Settings.UserTreeSettings model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("AccountApiController.SetTreeParams userID={0}, userName={1}, treeType={2}, selectedID={3}, selectedClassID={4}, selectedName={5}, field={6}", user.Id, user.UserName, model.FiltrationTreeType, model.FiltrationObjectID, model.FiltrationObjectClassID, model.FiltrationObjectName, model.FiltrationField);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return RequestResponceType.AccessError;
                    //
                    settings.AssetFiltrationObjectClassID = model.FiltrationObjectClassID;
                    settings.AssetFiltrationObjectID = model.FiltrationObjectID;
                    settings.AssetFiltrationObjectName = model.FiltrationObjectName;
                    settings.AssetFiltrationTreeType = model.FiltrationTreeType;
                    settings.AssetFiltrationField = model.FiltrationField;
                    //
                    settings.Save(dataSource);
                }
                //
                return RequestResponceType.Success;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return RequestResponceType.ValidationError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method ResetTreeParams
        [HttpPost]
        [Route("accountApi/ResetTreeParams", Name = "ResetTreeParams")]
        public RequestResponceType ResetTreeParams()
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("AccountApiController.ResetTreeParams userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return RequestResponceType.AccessError;
                    //
                    settings.DTLTree = null;
                    //
                    settings.Save(dataSource);
                }
                //
                return RequestResponceType.Success;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return RequestResponceType.ValidationError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method SetIncomingCallProcessing
        [HttpPost]
        [Route("accountApi/SetIncomingCallProcessing", Name = "SetIncomingCallProcessing")]
        [AllowAnonymous]
        public bool SetIncomingCallProcessing([FromForm]bool incomingCallProcessing)
        {
            var user = base.CurrentUser;
            if (user == null)
                return true;
            //
            Logger.Trace("AccountApiController.SetIncomingCallProcessing userID={0}, userName={1}, incomingCallProcessing={2}", user.Id, user.UserName, incomingCallProcessing);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return false;
                    //
                    if (settings.IncomingCallProcessing == incomingCallProcessing)
                        return true;
                    //
                    settings.IncomingCallProcessing = incomingCallProcessing;
                    settings.Save(dataSource);
                }
                //
                return true;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return false;
            }
        }
        #endregion

        #region method SetFinanceBudget
        [HttpPost]
        [Route("accountApi/SetFinanceBudget", Name = "SetFinanceBudget")]
        [AllowAnonymous]
        public bool SetFinanceBudget([FromForm]Guid? financeBudgetID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return true;
            //
            Logger.Trace("AccountApiController.SetFinanceBudget userID={0}, userName={1}, financeBudgetID={2}", user.Id, user.UserName, financeBudgetID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return false;
                    //
                    if (settings.FinanceBudgetID == financeBudgetID)
                        return true;
                    //
                    settings.FinanceBudgetID = financeBudgetID;
                    settings.Save(dataSource);
                }
                //
                return true;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return false;
            }
        }
        #endregion

        #region method SetUseCompactMenuOnly
        [HttpPost]
        [Route("accountApi/SetUseCompactMenuOnly", Name = "SetUseCompactMenuOnly")]
        [AllowAnonymous]
        public bool SetUseCompactMenuOnly([FromForm]bool useCompactMenuOnlyValue)
        {
            var user = base.CurrentUser;
            if (user == null)
                return true;
            //
            Logger.Trace("AccountApiController.SetUseCompactMenuOnly userID={0}, userName={1}, useCompactMenuOnlyValue={2}", user.Id, user.UserName, useCompactMenuOnlyValue);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return false;
                    //
                    if (settings.UseCompactMenuOnly == useCompactMenuOnlyValue)
                        return true;
                    //
                    settings.UseCompactMenuOnly = useCompactMenuOnlyValue;
                    settings.Save(dataSource);
                }
                //
                return true;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return false;
            }
        }
        #endregion

        #region method SetTimeSheetFilter
        [HttpPost]
        [Route("accountApi/SetTimeSheetFilter", Name = "SetTimeSheetFilter")]
        [AllowAnonymous]
        public bool SetTimeSheetFilter([FromForm]byte timeSheetFilter)
        {
            var user = base.CurrentUser;
            if (user == null)
                return true;
            //
            Logger.Trace("AccountApiController.SetTimeSheetFilter userID={0}, userName={1}, timeSheetFilter={2}", user.Id, user.UserName, timeSheetFilter);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = UserSettings.TryGetOrCreate(user.User, user.DefaultUserCulture, dataSource);
                    if (settings == null)
                        return false;
                    //
                    if (settings.TimeSheetFilter == timeSheetFilter)
                        return true;
                    //
                    settings.TimeSheetFilter = timeSheetFilter;
                    settings.Save(dataSource);
                }
                //
                return true;
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек пользователя.");
                return false;
            }
        }
        #endregion

        #region method SetCurrentFilter
        [HttpPost]
        [Obsolete("Use /api/currentfilters/{view} instead")]
        [Route("accountApi/setCurrentFilter", Name = "SetCurrentFilter")]
        public RequestResponceType SetCurrentFilter([FromQuery]string viewName, [FromQuery]string filterIDString, [FromQuery]bool isTemp, [FromQuery]bool withFinishedWorkflow, [FromQuery]string afterModifiedMilliseconds, [FromQuery] BLL.Modules module)
        {
            var user = base.CurrentUser;
            Guid? filterID = null;
            if (!string.IsNullOrWhiteSpace(filterIDString))
            {
                Guid tmpID;
                if (Guid.TryParse(filterIDString, out tmpID))
                    filterID = tmpID;
            }
            DateTime? afterUtcDateModified = BLL.Helpers.JSDateTimeHelper.FromJSMilliseconds(afterModifiedMilliseconds);
            //
            if (user == null || string.IsNullOrWhiteSpace(viewName))
                return RequestResponceType.NullParamsError;
            else if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("AccountApiController.SetCurrentFilter userID={0}, userName={1}, viewName={2}, filterId={3}, isTemp={4}, withFinishedWorkflow={5}, afterUtcModified={6}, module={7} failed (user is client)", user.Id, user.UserName, viewName, filterIDString, isTemp, withFinishedWorkflow, afterUtcDateModified.HasValue ? afterUtcDateModified.Value.ToString() : string.Empty, module);
                return RequestResponceType.AccessError;
            }
            //
            Logger.Trace("AccountApiController.SetCurrentFilter userID={0}, userName={1}, viewName={2}, filterId={3}, isTemp={4}, withFinishedWorkflow={5}, afterUtcModified={6}, module={7}", user.Id, user.UserName, viewName, filterIDString, isTemp, withFinishedWorkflow, afterUtcDateModified.HasValue ? afterUtcDateModified.Value.ToString() : string.Empty, module);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    FilterSettings.DeleteTempSettings(user.User.ID, viewName, dataSource);
                    //
                    var settings = new FilterSettings(user.User.ID, filterID, viewName, isTemp, withFinishedWorkflow, afterUtcDateModified);
                    settings.Save(dataSource);
                }
                return RequestResponceType.Success;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения текущего фильтра пользователя.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method SetFiltersGeneralSettings
        [HttpPost]
        [Route("accountApi/SetFiltersGeneralSettings", Name = "SetFiltersGeneralSettings")]
        public RequestResponceType SetFiltersGeneralSettings([FromQuery] string viewName, [FromQuery] BLL.Modules module, [FromQuery] string filterIDString, [FromQuery] bool filterOther, [FromQuery] Guid? newUserID)
        {
            var user = base.CurrentUser;
            Guid? filterID = null;
            if (!string.IsNullOrWhiteSpace(filterIDString))
            {
                Guid tmpID;
                if (Guid.TryParse(filterIDString, out tmpID))
                    filterID = tmpID;
            }
            DateTime? afterUtcDateModified = DateTime.UtcNow;
            //
            if (user == null || string.IsNullOrWhiteSpace(viewName))
                return RequestResponceType.NullParamsError;
            else if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("AccountApiController.SetFiltersGeneralSettings userID={0}, userName={1}, viewName={2}, filterId={3}, afterUtcModified={4}, module={5} failed (user is client)", user.Id, user.UserName, viewName, filterIDString, afterUtcDateModified.HasValue ? afterUtcDateModified.Value.ToString() : string.Empty, module);
                return RequestResponceType.AccessError;
            }
            //
            Logger.Trace("AccountApiController.SetCurrentFilter userID={0}, userName={1}, viewName={2}, filterId={3}, afterUtcModified={4}, module={5}", user.Id, user.UserName, viewName, filterIDString, afterUtcDateModified.HasValue ? afterUtcDateModified.Value.ToString() : string.Empty, module);
            BLL.Tables.Filters.Filter filter = null;
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var changeUser = false;
                    filter = BLL.Tables.Filters.Filter.Get((Guid)filterID, user.User.ID, dataSource);
                    if (newUserID != null && filter.UserID != newUserID)
                    {
                        filter.UserID = newUserID;
                        filter.Save();
                        changeUser = true;
                    }
                    //
                    filter.SetFiltersGeneralSettings(user.User.ID, filterOther, changeUser, dataSource);
                }
                return RequestResponceType.Success;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения текущего фильтра пользователя.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method GetCurrentFilter
        [HttpGet]
        [Obsolete("Use /api/currentfilters/{view} instead")]
        [Route("accountApi/getCurrentFilter", Name = "GetCurrentFilter")]
        public FilterData GetCurrentFilter([FromQuery] string viewName, [FromQuery] BLL.Modules module)
        {
            var user = base.CurrentUser;
            if (user == null || string.IsNullOrWhiteSpace(viewName))
                return new FilterData(RequestResponceType.NullParamsError);
            else if (!user.ActionIsGranted(module, viewName))
            {
                Logger.Warning("AccountApiController.GetCurrentFilter userID={0}, userName={1}, viewName={2} failed (user is client)", user.Id, user.UserName, viewName);
                return new FilterData(RequestResponceType.AccessError);
            }
            //
            Logger.Trace("AccountApiController.GetCurrentFilter userID={0}, userName={1}, viewName={2}", user.Id, user.UserName, viewName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = FilterSettings.Get(user.User.ID, viewName, dataSource);
                    //
                    while (settings != null && settings.Temp)
                    {
                        FilterSettings.DeleteTempSettings(user.User.ID, viewName, dataSource);
                        settings = FilterSettings.Get(user.User.ID, viewName, dataSource);
                    }
                    //
                    if (settings != null)
                    {
                        if (settings.FilterID.HasValue && !settings.Temp)
                        {
                            var filter = BLL.Tables.Filters.Filter.Get(settings.FilterID.Value, user.User.ID, dataSource);
                            if (filter == null)
                            {
                                filter = BLL.Tables.Filters.Filter.GetDefaultFilter(viewName, user.User.ID, dataSource);
                                settings = new FilterSettings(user.User.ID, filter.ID, viewName, false, false, null);
                                settings.Save(dataSource);
                                return new FilterData(RequestResponceType.Success, filter, settings.WithFinishedWorkflow, settings.AfterUtcDateModified);
                            }
                            else return new FilterData(RequestResponceType.Success, filter, settings.WithFinishedWorkflow, settings.AfterUtcDateModified);
                        }
                        else
                        {
                            var filter = BLL.Tables.Filters.Filter.GetDefaultFilter(viewName, user.User.ID, dataSource);
                            if (filter != null)
                            {
                                settings = new FilterSettings(user.User.ID, filter.ID, viewName, false, settings.WithFinishedWorkflow, settings.AfterUtcDateModified);
                                settings.Save(dataSource);
                                return new FilterData(RequestResponceType.Success, filter, settings.WithFinishedWorkflow, settings.AfterUtcDateModified);
                            }
                            else return new FilterData(RequestResponceType.FiltrationError, null, settings.WithFinishedWorkflow, settings.AfterUtcDateModified);
                        }
                    }
                    else
                    {
                        var filter = BLL.Tables.Filters.Filter.GetDefaultFilter(viewName, user.User.ID, dataSource);
                        settings = new FilterSettings(user.User.ID, filter.ID, viewName, false, false, null);
                        settings.Save(dataSource);
                        return new FilterData(RequestResponceType.Success, filter, settings.WithFinishedWorkflow, settings.AfterUtcDateModified);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения текущего фильтра пользователя.");
                return new FilterData(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetColumnSettingsList
        [HttpGet]
        [Route("accountApi/GetColumnSettingsList", Name = "GetColumnSettingsList")]
        public List<DTL.Settings.ColumnSettings> GetColumnSettingsList([FromQuery] string listName)
        {
            return GetColumnSettingsListInner(listName, "AccountApiController.GetColumnSettingsList");
        }
        #endregion

        #region method SetColumnSettingsList
        [HttpPost]
        [Route("accountApi/SetColumnSettingsList", Name = "SetColumnSettingsList")]
        public bool SetColumnSettingsList([FromBodyOrForm]List<DTL.Settings.ColumnSettings> columnsDTL)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("AccountApiController.SetColumnSettingsList userID={0}, userName={1}", user.Id, user.UserName);
            if (columnsDTL == null || columnsDTL.Count == 0 || columnsDTL[0].UserID.ToString() != user.Id)
                return false;
            //
            var first = columnsDTL[0];
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = ColumnSettings.TryGetOrCreate(user.User, first.ListName, dataSource);
                    if (settings == null)
                        return false;
                    //
                    settings.ColumnsDTL = columnsDTL;
                    settings.Save(dataSource);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек столбцов.");
                return false;
            }
        }
        #endregion

        #region method GetSplitterSettings
        [HttpGet]
        [Obsolete("Use api/splitterSettings/{name} instead")]
        [Route("accountApi/GetSplitterSettings", Name = "GetSplitterSettings")]
        public DTL.Settings.SplitterSettings GetSplitterSettings(string splitterName)
        {
            var user = base.CurrentUser;

            //
            Logger.Trace("AccountApiController.GetSplitterSettings userID={0}, userName={1}, splitterName={2}", user.Id, user.UserName, splitterName);
            //
            var settings = SplitterSettings.TryGetOrCreate(user.User, splitterName);
            return settings == null ? null : settings.DTL;
        }
        #endregion

        #region method SetSplitterSettings
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/SetSplitterSettings", Name = "SetSplitterSettings")]
        [Obsolete("Use api/splitterSettings/{name} instead")]
        public RequestResponceType SetSplitterSettings(string splitterName, int distance)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            if (string.IsNullOrWhiteSpace(splitterName))
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("AccountApiController.SetSplitterSettings userID={0}, userName={1}, splitterName={2}, distance={3}", user.Id, user.UserName, splitterName, distance);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = SplitterSettings.TryGetOrCreate(user.User, splitterName, dataSource);
                    if (settings == null)
                        return RequestResponceType.BadParamsError;
                    //
                    settings.Distance = distance;
                    settings.Save(dataSource);
                }
                return RequestResponceType.Success;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек сплиттера.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method GetCustomControl
        [HttpGet]
        [Route("accountApi/GetCustomControl", Name = "GetCustomControl")]
        [Obsolete("GET api/{WebApiResource}/{id}/customControls/my/")]
        public bool? GetCustomControl([FromQuery]Guid objectID)
        {
            return false;
            var user = base.CurrentUser;

            //
            Logger.Trace("AccountApiController.GetCustomControl userID={0}, userName={1}, objectID={2}", user.Id, user.UserName, objectID);
            //
            try
            {
                var retval = user.User.GetCustomControl(objectID);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
        #endregion

        #region method SetCustomControl
        [HttpPost]
        [Route("accountApi/SetCustomControl", Name = "SetCustomControl")]
        [Obsolete("PUT to api/{WebApiResource}/{id}/customcontrols/")]
        [AllowAnonymous]
        public bool SetCustomControl([FromQuery]Guid objectID, [FromQuery]int objectClassID, [FromQuery]bool value)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("AccountApiController.SetCustomControl userID={0}, userName={1}, objectID={2}, objectClassID={3}, value={4}", user.Id, user.UserName, objectID, objectClassID, value);
            //
            try
            {
                user.User.SetCustomControl(objectID, objectClassID, value);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        #endregion


        #region method GetFormSettings
        [HttpGet]
        [Route("accountApi/GetFormSettings", Name = "GetFormSettings")]
        [Obsolete("Use api/formsettings/{name} instead")]
        public DTL.Settings.FormSettings GetFormSettings([FromQuery] string formName)
        {
            var user = base.CurrentUser;

            //
            Logger.Trace("AccountApiController.GetFormSettings userID={0}, userName={1}, formName={2}", user.Id, user.UserName, formName);
            //
            var settings = FormSettings.TryGetOrCreate(user.User, formName);
            return settings == null ? null : settings.DTL;
        }
        #endregion

        #region method SetFormSettings
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/SetFormSettings", Name = "SetFormSettings")]
        [Obsolete("Use api/formsettings/{name} instead")]
        public bool SetFormSettings(DTL.Settings.FormSettings formSettingsDTL)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("AccountApiController.SetUserSettings userID={0}, userName={1}, formName={2}", user.Id, user.UserName, formSettingsDTL == null ? string.Empty : formSettingsDTL.FormName);

            if (formSettingsDTL == null || string.IsNullOrWhiteSpace(formSettingsDTL.FormName))
                return false;
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var settings = FormSettings.TryGetOrCreate(user.User, formSettingsDTL.FormName, dataSource);
                    if (settings == null)
                        return false;
                    //
                    settings.DTL = formSettingsDTL;
                    settings.Save(dataSource);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения настроек форм.");
                return false;
            }
        }
        #endregion


        #region method OperationIsGranted (not use)
        [HttpGet]
        [Route("accountApi/OperationIsGranted", Name = "OperationIsGranted")]
        public bool OperationIsGranted(int id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("AccountApiController.OperationIsGranted userID={0}, userName={1}, operationID={2}", user.Id, user.UserName, id);
            //
            var retval = user.User.OperationIsGranted(id);
            return retval;
        }
        #endregion


        #region method CallToUser
        [HttpGet]
        [Route("accountApi/CallToUserFromMyWorkplace", Name = "CallToUserFromMyWorkplace")]
        public bool CallToUserFromMyWorkplace(Guid userID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            Logger.Trace("AccountApiController.CallToUserFromMyWorkplace userID={0}, userName={1}, toUserID={2}", user.Id, user.UserName, userID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var currentUserInfo = BLL.Users.UserInfo.Get(user.User.ID, dataSource);
                    var userInfo = BLL.Users.UserInfo.Get(userID, dataSource);
                    //
                    return BLL.TelephonyWrapper.CallTo(currentUserInfo, userInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка начала звонка пользователю со стационарного телефона рабочего места пользователя.");
                return false;
            }
        }
        #endregion

        #region method GetListForObject
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/GetListForObject", Name = "AccountGetListForObject")]
        public ResultData<List<BaseForTable>> GetListForObject([FromForm] TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            if (!user.User.HasAdminRole)
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.AccessError);

            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.AdminTools);
        }
        #endregion

        #region method KillUserSession
        public sealed class KillUserSessionParameterIn
        {
            public List<KillUserSessionItem> List { get; set; }
        }
        public sealed class KillUserSessionItem
        {
            public Guid UserID { get; set; }
            public string UserAgent { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/KillUserSession", Name = "KillUserSession")]
        public RequestResponceType KillUserSession(KillUserSessionParameterIn param)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            if (param == null || param.List == null)
                return RequestResponceType.BadParamsError;
            //
            Logger.Trace("AccountApiController.KillUserSession userID={0}, userName={1}, param.List.Count={2}", user.Id, user.UserName, param.List.Count);
            //
            bool success = true;
            try
            {
                foreach (var item in param.List)
                {
                    var result = BLL.Users.Session.Kill(item.UserID, item.UserAgent, DateTime.UtcNow, user.User.ID);
                    if (result)
                        HttpContext.OnUserSessionChanged(_hubContext, item.UserID, item.UserAgent);
                    success &= result;
                }
                //
                if (success)
                    return RequestResponceType.Success;
                else
                    return RequestResponceType.OperationError;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сброса сессии пользователя.");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method GetSessionInfo
        [HttpGet]
        [Route("accountApi/GetSessionInfo", Name = "GetSessionInfo")]
        public ResultData<DTL.Users.SessionInfo> GetSessionInfo()
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultData<DTL.Users.SessionInfo>.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("AccountApiController.GetSessionInfo userID={0}, userName={1}", user.Id, user.UserName);
            //
            try
            {
                var retval = BLL.Users.Session.GetSessionInfo();
                return ResultData<DTL.Users.SessionInfo>.Create(RequestResponceType.Success, retval);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информации о сессиях.");
                return ResultData<DTL.Users.SessionInfo>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion


        #region method GetPersonalLicenceList
        public sealed class GetPersonalLicenceListIn
        {
            public Guid[] IDList { get; set; }
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/GetPersonalLicenceList", Name = "GetPersonalLicenceList")]
        public ResultData<List<BLL.Users.PersonalLicence>> GetPersonalLicenceList([FromForm] GetPersonalLicenceListIn model)
        {
            var user = base.CurrentUser;
            if (!user.User.HasAdminRole)
                return ResultData<List<BLL.Users.PersonalLicence>>.Create(RequestResponceType.AccessError);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var list = BLL.Users.PersonalLicence.GetList(model?.IDList, dataSource);
                    return ResultData<List<BLL.Users.PersonalLicence>>.Create(RequestResponceType.Success, list);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultData<List<BLL.Users.PersonalLicence>>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method InsertPersonalLicence
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/insertPersonalLicence", Name = "InsertPersonalLicence")]
        public ResultData<BLL.Users.PersonalLicence> InsertPersonalLicence([FromQuery] Guid userID)
        {
            var user = base.CurrentUser;
            if (!user.User.HasAdminRole)
                return ResultData<BLL.Users.PersonalLicence>.Create(RequestResponceType.AccessError);
            //
            try
            {
                var retval = BLL.Users.PersonalLicence.Insert(userID);
                return ResultData<BLL.Users.PersonalLicence>.Create(retval == null ? RequestResponceType.OperationError : RequestResponceType.Success, retval);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultData<BLL.Users.PersonalLicence>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method DeletePersonalLicence
        public sealed class DeletePersonalLicenceIn
        {
            public Guid[] IDList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/deletePersonalLicence", Name = "DeletePersonalLicence")]
        public RequestResponceType DeletePersonalLicence([FromForm] DeletePersonalLicenceIn model)
        {
            var user = base.CurrentUser;
            if (!user.User.HasAdminRole)
                return RequestResponceType.AccessError;
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    foreach (var userID in model.IDList)
                    {
                        var retval = BLL.Users.PersonalLicence.Delete(userID, user.User.ID, dataSource);
                        if (!retval)
                        {
                            dataSource.RollbackTransaction();
                            return RequestResponceType.ConcurrencyError;
                        }
                    }
                    dataSource.CommitTransaction();
                    //
                    return RequestResponceType.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return RequestResponceType.GlobalError;
            }
        }
        #endregion


        #region helper class SignInData
        //для входа в систему
        public class SignInData
        {
            private readonly int __key = 13;
            public SignInData()
            {
                this.LoginName = string.Empty;
                this.PasswordEncrypted = string.Empty;
            }

            public string LoginName { get; set; }
            public string PasswordEncrypted { get; set; }

            public string GetPasswordDecrypted()
            {
                if (string.IsNullOrEmpty(PasswordEncrypted))
                    return null;
                //
                var retval = string.Empty;
                for (int i = 0; i < PasswordEncrypted.Length; i++)
                    retval += (char)(PasswordEncrypted[i] ^ __key);
                return retval;
            }

            public string GetLoginWithoutDomain()
            {
                if(String.IsNullOrWhiteSpace(LoginName))
                {
                    return null;
                }

                int pos = LoginName.IndexOf('\\');

                return pos != -1 ? LoginName.Substring(pos + 1) : LoginName;
            }
        }
        #endregion

        #region helper class SignInRresult
        //для входа в систему
        public class SignInResult
        {
            public SignInResult(bool success, string rerirectUrl)
            {
                this.Success = success;
                this.RedirectUrl = rerirectUrl;
            }

            public string RedirectUrl { get; private set; }
            public bool Success { get; private set; }
        }
        #endregion

        #region helper class FilterData
        public sealed class FilterData
        {
            public FilterData(RequestResponceType type)
            {
                this.Filter = null;
                this.WithFinishedWorkflow = false;
                this.AfterUtcModified = null;
                this.Result = type;
            }

            public FilterData(RequestResponceType type, BLL.Tables.Filters.Filter filter, bool withFinishedWorkflow, DateTime? afterUtcModified)
            {
                this.Filter = filter;
                this.WithFinishedWorkflow = withFinishedWorkflow;
                this.AfterUtcModified = afterUtcModified;
                this.Result = type;
            }

            public BLL.Tables.Filters.Filter Filter { get; set; }
            public bool WithFinishedWorkflow { get; set; }
            public DateTime? AfterUtcModified { get; set; }
            //
            public RequestResponceType Result { get; set; }
        }
        #endregion


        public sealed class GetDeputyObjByIDIncoming
        {
            public Guid ModelID { get; set; }
        }
        public sealed class GetDeputyObjByIDOut
        {
            public Deputy Model { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("accountApi/GetDeputyByID", Name = "GetDeputyByID")]
        public GetDeputyObjByIDOut GetDeputyByID([FromQuery] GetDeputyObjByIDIncoming model)
        {
            var user = base.CurrentUser;
            Logger.Trace("AccountApiController.GetByID UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Deputy.Get(model.ModelID, dataSource);
                    //
                    return new GetDeputyObjByIDOut() { Model = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetDeputyObjByIDOut() { Model = null, Result = RequestResponceType.GlobalError };
            }
        }

        public sealed class GetDeputyCategoryResult
        {
            public Guid? ID { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("accountApi/SaveDeputy", Name = "SaveDeputyCategory")]
        public GetDeputyCategoryResult SaveDeputyCategory([FromBodyOrForm] Web.DTL.Assets.SaveDeputy model)
        {
            var user = base.CurrentUser;
            Logger.Trace("AccountApiController.SaveDeputy UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                model.UtcDataDeputyWith = model.UtcDataDeputyWithDT.ParseAsDate() ??  DateTime.Parse(model.UtcDataDeputyWithDT);
                model.UtcDataDeputyBy = model.UtcDataDeputyByDT.ParseAsDate() ?? DateTime.Parse(model.UtcDataDeputyByDT);
                using (var dataSource = DataSource.GetDataSource())
                {
                    Guid ID = Deputy.Save(model, user.User.ID, dataSource);
                    //
                    return new GetDeputyCategoryResult() { ID = ID, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetDeputyCategoryResult() { ID = null, Result = RequestResponceType.GlobalError };
            }
        }
        public sealed class ObjectID
        {
            public Guid ID { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("accountApi/RemoveDeputy", Name = "RemoveDeputy")]
        public ResultWithMessage RemoveDeputy([FromBodyOrForm] ObjectID model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("AccountApiController.RemoveDeputy userID={0}, userName={1}}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    Deputy.Remove(model.ID, user.User.ID, dataSource);
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError, ex.Message);
            }
        }
    }
}
