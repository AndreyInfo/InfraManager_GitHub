using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD.Calls;
using InfraManager.Web.BLL.SD.MyWorkplace;
using InfraManager.Web.BLL.SD.Problems;
using InfraManager.Web.BLL.SD.WorkOrders;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InfraManager.BLL;

namespace InfraManager.Web.Helpers
{
    public class ApplicationUser
    {
        #region consts

        public const string SecurityClaimName = "SecurityStamp";

        #endregion

        #region constructor

        private readonly HttpContext _httpContext;

        public ApplicationUser(HttpContext httpContext, BLL.Users.User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            //
            //for string cache
            _httpContext = httpContext;
            this.Id = user.ID.ToString();
            this.UserName = user.FullName;
            //
            this.User = user;
        }

        #endregion

        #region properties

        public string Id { get; private set; }
        public string UserName { get; set; }

        public BLL.Users.User User { get; private set; }

        public string DefaultUserCulture
        {
            get { return _httpContext.GetSupportedBrowserCultureName(); }
        }

        #endregion

        #region static method GenerateNewSecurityStamp

        public static string GenerateNewSecurityStamp()
        {
            return GetRandomString(151);
        }

        #endregion


        #region static method GetRandomString

        private static string GetRandomString(int length,
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars))
                throw new ArgumentException("allowedChars may not be empty.");
            //
            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length)
                throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.",
                    byteSize));
            //
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }

                return result.ToString();
            }
        }

        #endregion


        #region method IsClientView

        public bool IsClientView(Core.Data.DataSource dataSource)
        {
            var userSettings = BLL.Settings.UserSettings.TryGetOrCreate(this.User, this.DefaultUserCulture, dataSource);
            return IsClientView(userSettings.ViewNameSD);
        }

        public bool IsClientView(string viewName)
        {
            if (viewName == BLL.SD.Calls.ClientCallForTable.VIEW_NAME ||
                viewName == BLL.SD.MyWorkplace.CustomControlForTable.VIEW_NAME ||
                viewName == BLL.SD.MyWorkplace.CommonWithNegotiationsForTable.VIEW_NAME)
                return true;
            //
            return false;
        }

        #endregion

        #region method IsEngineerView

        public bool IsEngineerView(string viewName)
        {
            if (viewName == BLL.SD.Calls.CallForTable.VIEW_NAME ||
                viewName == BLL.SD.MyWorkplace.CommonForTable.VIEW_NAME ||
                viewName == BLL.SD.WorkOrders.WorkOrderForTable.VIEW_NAME ||
                viewName == BLL.SD.Problems.ProblemForTable.VIEW_NAME ||
                viewName == BLL.SD.MyWorkplace.CommonWithNegotiationsForTable
                    .VIEW_NAME) //так нужно. На согласовании - полная форма в режиме чтения
                return true;
            //
            return false;
        }

        #endregion

        #region method ActionIsGranted

        public bool ActionIsGranted(BLL.Modules module, string viewName)
        {
            if (module == BLL.Modules.Asset)
            {
                if (!this.User.HasRoles)
                    return false;
                else return true;
            }

            if (module == BLL.Modules.Finance)
            {
                if (!this.User.HasRoles)
                    return false;
                else return true;
            }
            else if (module == BLL.Modules.SD)
            {
                if (!this.User.HasRoles && !this.IsClientView(viewName))
                    return false;
            }

            //
            return true;
        }

        #endregion

        #region private method GetClasses

        public static ObjectClass[] GetClasses(List<int> classes, string viewName)
        {
            if (classes == null)
                return Array.Empty<ObjectClass>();
            else
                return classes.Count == 0 || classes[0] == 0
                    ? GetClasses(viewName)
                    : classes.Select(c => (ObjectClass)c).ToArray();
        }

        public static ObjectClass[] GetClasses(string viewName)
        {
            if (viewName == null)
                return new[]
                {
                    ObjectClass.Call,
                    ObjectClass.WorkOrder,
                    ObjectClass.Problem,
                    ObjectClass.KBArticle,
                    ObjectClass.MassIncident
                };

            switch (viewName)
            {
                case ClientCallForTable.VIEW_NAME:
                case CallForTable.VIEW_NAME:
                    return new[] { ObjectClass.Call };
                case CommonForTable.VIEW_NAME:
                case CommonWithNegotiationsForTable.VIEW_NAME:
                    return new[]
                    {
                        ObjectClass.Call,
                        ObjectClass.WorkOrder,
                        ObjectClass.Problem,
                        ObjectClass.KBArticle,
                        ObjectClass.MassIncident
                    };
                case WorkOrderForTable.VIEW_NAME:
                    return new[] { ObjectClass.WorkOrder };
                case ProblemForTable.VIEW_NAME:
                    return new[] { ObjectClass.Problem };
                case CustomControlForTable.VIEW_NAME:
                    return new[]
                    {
                        ObjectClass.Call,
                        ObjectClass.WorkOrder,
                        ObjectClass.Problem
                    };
                case ListView.AllMassIncidents:
                    return new[]
                    {
                        ObjectClass.MassIncident
                    };
                default:
                    throw new NotSupportedException("viewName");
            }
        }

        #endregion
    }
}