using InfraManager.Core.Logging;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.BLL;
using InfraManager.Web.Helpers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers
{
    public class ResourceApiController : ControllerBase
    {
        #region fields
        //TODO move __languageFlags to resourceArea
        private static Dictionary<string, string> __languageFlags = new Dictionary<string, string>() { { "ru-RU", @"/images/controls/flags/flag_russia.svg" }, { "en-US", @"/images/controls/flags/flag_uk.svg" } };
        private static List<LanguageInfo> __languages = LoadLanguages();
        private static Dictionary<string, string> __resourceSet = LoadResourceSet();//[{ru-RU, [{resourceName, localizedResourceValue},]},]
        private static ConcurrentDictionary<string, List<string>> __fileResources = new ConcurrentDictionary<string, List<string>>();//[{filePath, [resourceName1, ]},]
        private static string[] __searchPatterns = new string[] { "restext:", "resattr:", "getTextResource(" };
        #endregion

        #region constructor

        private readonly IHubContext<EventHub> _hubContext;

        public ResourceApiController(
            IHubContext<EventHub> hubContext)
        {
            _hubContext = hubContext;
        }
        #endregion

        #region method GetResourceSet
        public static async Task<string> GetResourceSetAsync(HttpContext httpContext, IUserLanguageChecker userLanguageChecker)
        {
            Logger.Trace("ResourceApiController.GetResourceSet");

            await userLanguageChecker.CheckAsync(httpContext);
            var cultureName = httpContext?.GetCurrentCulture() ?? CultureInfo.CurrentUICulture.Name;

            string resourceSet = null;
            if (__resourceSet.ContainsKey(cultureName))
                resourceSet = __resourceSet[cultureName];
            else
            {
                Logger.Trace("ResourceApiController.GetResourceSet finded unsupported language '{0}'", cultureName);
                resourceSet = __resourceSet[BLL.Global.EN]; //for other unsupported languages
            }
            //
            return resourceSet;
        }
        #endregion

        #region method GetLanguages
        [HttpGet]
        [Route("resourceApi/GetLanguages", Name = "GetLanguages")]
        public List<LanguageInfo> GetLanguages()
        {
            return __languages;
        }
        #endregion


        #region static method LoadResourceSet
        private static Dictionary<string, string> LoadResourceSet()
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();
            //
            foreach (LanguageInfo li in __languages)
            {
                var set = new StringBuilder();
                set.AppendLine(@"{");
                //
                var rs = Resources.ResourceManager.GetResourceSet(CultureInfo.GetCultureInfo(li.ID), true, true);
                foreach (DictionaryEntry pair in rs)
                    set.AppendLine(String.Format(@" {0}: ""{1}"" ,",
                                HttpUtility.JavaScriptStringEncode(((string)pair.Key).Replace(@"'", @"""")),
                                HttpUtility.JavaScriptStringEncode(((string)pair.Value).Replace(@"'", @""""))
                                ));
                set.AppendLine(@"}");
                //
                retval.Add(li.ID, set.ToString());
            }
            //
            return retval;
        }
        #endregion

        #region static method LoadLanguages
        private static List<LanguageInfo> LoadLanguages()
        {
            return ResourcesArea.Global.AvailableLanguages.
                Select(x => CultureInfo.GetCultureInfo(x)).
                Select(y =>
                    new LanguageInfo
                    {
                        ID = y.Name,
                        Name = y.NativeName,
                        Description = y.EnglishName,
                        Image = __languageFlags[y.Name]
                    }).
                ToList();
        }
        #endregion

        #region helper class LanguageInfo
        public sealed class LanguageInfo
        {
            #region constructor
            public LanguageInfo()
            {
                this.ID = string.Empty;
                this.Name = string.Empty;
                this.Description = string.Empty;
                this.Image = string.Empty;
            }
            #endregion

            #region properties
            public string ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
            #endregion
        }
        #endregion


        #region method GetCreepingLineString
        //метод здесь из проверок доступа доступа
        [HttpGet]
        [Route("resourceApi/GetCreepingLineString", Name = "GetCreepingLineString")]
        public string GetCreepingLineString()
        {
            return string.Empty;
            
            try
            {
                var creepingLineList = SimpleDictionary.GetCreepingLineList();
                //
                var msgArray = creepingLineList.Select(x => x.Name).ToArray();
                var retval = string.Join(" ", msgArray);
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения бегущей строки");
                return null;
            }
        }
        #endregion
    }
}