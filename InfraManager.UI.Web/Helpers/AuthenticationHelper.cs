using InfraManager.BLL.Users;
using InfraManager.Core.Caching;
using InfraManager.Core.Helpers;
using InfraManager.Core.Logging;
using InfraManager.DAL.Users;
using InfraManager.ResourcesArea;
using InfraManager.Web.BLL.Users;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;

namespace InfraManager.UI.Web.Helpers
{
    public class AuthenticationHelper
    {
        private ClaimsIdentity _identity;
        private ApplicationUser _currentUser;
        private readonly HttpContext _httpContext;
        private readonly IHubContext<EventHub> _hubContext;

        public AuthenticationHelper(HttpContext httpContext, IHubContext<EventHub> hubContext)
        {
            _httpContext = httpContext;
            _hubContext = hubContext;
            UserManager = new CustomUserManager(httpContext);

            if (httpContext?.User?.Identity != null)
            {
                _identity = (ClaimsIdentity)httpContext.User.Identity;
            }
        }

        public async Task SignInAsync(User userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            await SignInAsync(new ApplicationUser(_httpContext, userDto));
        }

        public async Task SignInAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            //
            Logger.Trace("AuthenticationHelper.SignInAsync userID={0}, userName={1}", user.Id, user.UserName);

            var securityClaimValue = ApplicationUser.GenerateNewSecurityStamp();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimTypes.Locality, user.DefaultUserCulture),
                ClaimsIdentityExtensions.CreateIdClaim(user.Id),
                new Claim(ApplicationUser.SecurityClaimName, securityClaimValue)
            };
            _identity = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(_identity));
            SetAutoLogon(true);

            Logger.Trace("CookieAuthenticationProvider.OnResponseSignIn userID={0}", user.Id);
            //
            try
            {
                Session.CreateOrRestore(
                    new Guid(user.Id),
                    DateTime.UtcNow,
                    securityClaimValue,
                    _httpContext.Request.Host.ToString(),
                    _httpContext.Request.Headers["User-Agent"].ToString());
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                SignOut();
                Logger.Error(ex, "Ошибка создания сессии пользователя.");
            }
            finally
            {
                _httpContext.OnUserSessionChanged(_hubContext, new Guid(user.Id));
            }
        }

        public async Task SignInAsync(UserDetailsModel user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));            

            var securityClaimValue = ApplicationUser.GenerateNewSecurityStamp();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.LoginName),
                ClaimsIdentityExtensions.CreateIdClaim(user.ID.ToString()),
                new Claim(ApplicationUser.SecurityClaimName, securityClaimValue)
            };

            SetAutoLogon(true);

            _identity = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(_identity));
        }

        public void SignOut()
        {
            if (_identity != null)
            {
                _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
                var userId = _identity.GetUserId();
                _identity = null;
                SetAutoLogon(false);
            }
        }

        public const string EnableWindowsAutoLogonCookie = "enableWindowsAutoLogon";

        private void SetAutoLogon(bool enabled)
        {
            Logger.Trace("AuthenticationHelper.SetAutoLogon enabled={0}", enabled);
                //            
            _httpContext.Response.Cookies.Append(EnableWindowsAutoLogonCookie, enabled ? "1" : "0");
        }

        public static bool IsAutoLogonEnabled(HttpContext httpContext)
        {
            Logger.Trace("AuthenticationHelper.IsAutoLogonEnabled check");

            var cookie = httpContext.Request.Cookies[EnableWindowsAutoLogonCookie];

            return !string.IsNullOrWhiteSpace(cookie) && cookie == "1";
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    lock (this)
                        if (_currentUser == null)
                        {
                            if (_identity == null)
                            {
                                Logger.Trace("AuthenticationHelper.CurrentUser: userIdentity is null");
                                throw new HttpException(System.Net.HttpStatusCode.Unauthorized);
                            }
                            //
                            string userIDStr = _identity.GetUserId();
                            if (string.IsNullOrWhiteSpace(userIDStr))
                            {
                                Logger.Trace("AuthenticationHelper.CurrentUser: userID is empty");
                                throw new HttpException(System.Net.HttpStatusCode.Unauthorized);
                            }
                            //
                            Logger.Trace("AuthenticationHelper.CurrentUser userID={0}, userName={1}",
                                userIDStr,
                                GetUserName());
                            //            
                            _currentUser = FindById(userIDStr) ?? throw new HttpException(System.Net.HttpStatusCode.Unauthorized);
                            ThreadHelper.SetData(IMSystem.Global.CurrentUserSlot, IM.BusinessLayer.User.Get(_currentUser.User.ID));
                        }
                return _currentUser;
            }
        }

        private string GetUserName() => _identity.Claims.SingleOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;

        public Guid? CurrentUserID
        {
            get
            {
                if (_identity == null)
                {
                    Logger.Trace("AuthenticationHelper.CurrentUserID: userIdentity is null");
                    return null;
                }
                //
                string userIDStr = _identity.GetUserId();
                if (string.IsNullOrWhiteSpace(userIDStr))
                {
                    Logger.Trace("AuthenticationHelper.CurrentUserID: userID is empty");
                    return null;
                }
                //
                Logger.Trace("AuthenticationHelper.CurrentUserID userID={0}, userName={1}",
                    userIDStr,
                    GetUserName());
                //            
                return new Guid(userIDStr);
            }
        }

        public ApplicationUser FindById(string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return null;
            //                
            Logger.Trace("CustomUserManager.FindByIdAsync(id), id={0}", userID);
            User userIM = null;
            try
            {
                userIM = User.Get(userID);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения пользователя по идентификатору.");
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized);
            }
            if (userIM == null)
            {
                SignOut();
                return null;
            }

            //
            Logger.Trace("CustomUserManager.FindByIdAsync(id), id={0} exists", userID);
            return new ApplicationUser(_httpContext, userIM);
        }

        public static User FindUserByLogin(string userLogin)
        {
            try
            {
                return User.GetByLoginName(userLogin);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения пользователя по логину.");                
            }

            return null;
        }

        public static bool ValidateLoginPassword(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            //
            Logger.Trace("AuthenticationHelper.ValidateLoginPassword userName={0}", user.FullName);
            //
            byte[] passwordBytes = user.WebPasswordHash;
            if (passwordBytes == null)
                return false;
            //
            byte[] passwordBytes2 = CalculatePassword(password);
            if (passwordBytes.Length != passwordBytes2.Length)
                return false;
            //
            for (int i = 5; i < passwordBytes.Length - 11; i++)
                if (passwordBytes[i] != passwordBytes2[i])
                    return false;
            //
            return true;
        }

        private static byte[] CalculatePassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            //
            Logger.Trace("AuthenticationHelper.CalculatePassword");
            //
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash = new MD5CryptoServiceProvider().ComputeHash(passwordBytes);
            byte[] guidBytes = Guid.NewGuid().ToByteArray();
            //
            byte[] retval = new Byte[passwordHash.Length + guidBytes.Length];
            Array.Copy(guidBytes, 0, retval, 0, 5);
            Array.Copy(passwordHash, 0, retval, 5, passwordHash.Length);
            Array.Copy(guidBytes, 5, retval, passwordHash.Length + 5, 11);
            //
            return retval;
        }

        public CustomUserManager UserManager { get; }

        public bool RestorePassword(string login, out string message)
        {
            Logger.Trace("AuthenticationHelper.RestorePassword login={0}", login);
            message = string.Empty;
            //
            if (string.IsNullOrWhiteSpace(login))
            {
                message = Resources.UserLoginNotFound;
                return false;
            }
            //
            User user = null;
            try
            {
                user = User.GetByLoginName(login);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения пользователя по логину.");
            }
            if (user == null)
            {
                message = Resources.UserLoginNotFound;
                return false;
            }
            //
            if (!user.WebAccessIsGranted)
            {
                message = Resources.UserNoRightsToWebAccess;
                return false;
            }
            //
            string email = user.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                message = Resources.UserEmailNotFound;
                return false;
            }
            //
            string password = SecurityHelper.GeneratePassword();
            byte[] passwordData = CalculatePassword(password);
            bool passwordUpdated = false;
            try
            {
                passwordUpdated = User.UpdateWebPassword(user.ID, passwordData);
            }
            catch { }
            if (!passwordUpdated)
            {
                message = Resources.PasswordNotRestored;
                return false;
            }
            //
            string msg = string.Format("{0}{1}{2}: {3}",
                Resources.YourPasswordIsRestored,
                Environment.NewLine,
                Resources.YourNewPassword,
                password);
            if (!User.SendUserEmail(email, Resources.PasswordRecovery, msg))
            {
                message = Resources.UserEmailIncorrect;
                return false;
            }
            //
            message = Resources.PasswordWasSendedToEmail;
            return true;
        }
    }
}
