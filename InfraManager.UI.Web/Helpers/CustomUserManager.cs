using InfraManager.Core.Logging;
using InfraManager.UI.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace InfraManager.Web.Helpers
{
    public sealed class CustomUserManager
    {
        #region constructor

        private readonly HttpContext _httpContext;

        public CustomUserManager(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        #endregion

        #region override method FindByIdAsync
        public Task<ApplicationUser> FindByIdAsync(string userID)
        {
            return Task<ApplicationUser>.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(userID))
                    return null;
                //                
                Logger.Trace("CustomUserManager.FindByIdAsync(id), id={0}", userID);
                BLL.Users.User userIM = null;
                try
                {
                    userIM = BLL.Users.User.Get(userID);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка получения пользователя по идентификатору.");
                }
                if (userIM == null)
                    return null;
                //
                Logger.Trace("CustomUserManager.FindByIdAsync(id), id={0} exists", userID);
                return new ApplicationUser(_httpContext, userIM);
            });
        }
        #endregion

        #region override method FindAsync
        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return Task<ApplicationUser>.Factory.StartNew(() =>
            {
                if (login == null || string.IsNullOrWhiteSpace(login.ProviderKey))
                    return null;
                //
                Logger.Trace("CustomUserManager.FindAsync(login), login.Key={0}", login.ProviderKey);
                BLL.Users.User userIM = null;
                try
                {
                    userIM = BLL.Users.User.GetByLoginName(login.ProviderKey);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка получения пользователя по логину.");
                }
                if (userIM == null)
                    return null;
                //
                Logger.Trace("CustomUserManager.FindAsync(login), login.Key={0} exists", login.ProviderKey);
                return new ApplicationUser(_httpContext, userIM);
            });
        }
        #endregion

        #region override method FindAsync
        public Task<ApplicationUser> FindAsync(string userName, string password)
        {
            return Task<ApplicationUser>.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return null;
                //
                Logger.Trace("CustomUserManager.FindAsync(userName, password), userName={0}", userName);
                BLL.Users.User userIM = null;
                try
                {
                    userIM = BLL.Users.User.GetByLoginName(userName);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка получения пользователя по логину.");
                }
                if (userIM == null)
                    return null;
                //
                Logger.Trace("CustomUserManager.FindAsync(userName, password), userName={0} exists", userName);
                password = password ?? string.Empty;

                if (!AuthenticationHelper.ValidateLoginPassword(userIM, password))
                    return null;
                //
                return new ApplicationUser(_httpContext, userIM);
            });
        }
        #endregion
    }
}