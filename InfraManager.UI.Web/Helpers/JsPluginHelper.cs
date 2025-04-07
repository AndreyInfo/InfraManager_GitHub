using InfraManager.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace InfraManager.Web.Helpers
{
    public static class JsPluginHelper
    {
        #region fields
        private static string[] __pluginFileNames;
        #endregion

        #region static method Initialize
        public static void Initialize(IWebHostEnvironment hosting)
        {
            __pluginFileNames = new string[0];
            //
            var pluginFolderPath = Path.Combine(hosting.WebRootPath ?? hosting.ContentRootPath ?? "/", "scripts/plugins");
            try
            {
                if (!Directory.Exists(pluginFolderPath))
                    Logger.Trace("JsPluginHelper.Initialize: plugin directory not found '{0}'", pluginFolderPath);
                else
                {
                    Logger.Trace("JsPluginHelper.Initialize: get plugins in '{0}'", pluginFolderPath);
                    var files = Directory.GetFiles(pluginFolderPath, "*.js", SearchOption.TopDirectoryOnly);
                    var list = new List<string>(files.Length);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var pluginName = Path.GetFileNameWithoutExtension(files[i]);
                        Logger.Trace("JsPluginHelper.Initialize: found plugin '{0}'", pluginName);
                        list.Add(pluginName);
                    }
                    __pluginFileNames = list.OrderBy(x => x).ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения наименований плагинов.");
                __pluginFileNames = new string[0];
            }
        }
        #endregion

        #region static method GetPlugins
        public static string[] GetPlugins()
        {
            return __pluginFileNames;
        }
        #endregion
    }
}