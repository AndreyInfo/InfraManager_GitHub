using System;
using System.Collections.Generic;
using System.IO;
using InfraManager.DAL.DbConfiguration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace InfraManager.UI.Web.Configuration
{
    public class AppSettingsEditor : IAppSettingsEditor
    {
        private readonly ILogger<AppSettingsEditor> _logger;
        private readonly Object _locker = new();
        private readonly string _environment;
        private readonly string _filePath;

        public AppSettingsEditor(ILogger<AppSettingsEditor> logger,
            IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _environment = hostEnvironment.EnvironmentName;
            _filePath =
                File.Exists(Path.Combine(AppContext.BaseDirectory, "settings",
                    "appsettings.json")) //TODO посмотреть в сторону Environment переменных
                    ? Path.Combine(AppContext.BaseDirectory, "settings", $"appsettings.{_environment}.json")
                    : Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }
        

        public void Edit<T>(string key, T value)
        {
            Edit(new List<(string key, T value)> { (key, value) });
        }
        
        public void Edit<T>(List<(string key, T value)> parameters)
        {
            try
            {
                dynamic jsonObj = GetObjectFromFile();

                foreach (var el in parameters)
                {
                    var sectionPath = el.key.Split(":");
                    if (sectionPath.Length > 1)
                    {
                        var keyPath = el.key.Split(":")[1];
                        jsonObj[sectionPath[0]][keyPath] = el.value;
                    }
                    else
                    {
                        jsonObj[sectionPath[0]] = el.value;
                    }
                }
     
                lock (_locker)
                {
                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(_filePath, output);
                }

                _logger.LogInformation("appsettings.json file was changed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while save appsettings.json, path = {_filePath}");
            }
        }
        

        public string[] GetValues(string[] keys) //TODO Create model with appsettigns.json to deserialize in it
        {
            var jsonObject = GetObjectFromFile();
            List<string> result = new List<string>();

            foreach (var el in keys)
            {
                result.Add(jsonObject[el].ToString());
            }

            return result.ToArray();
        }

        private dynamic GetObjectFromFile()
        {
            string json;
            lock (_locker)
            {
                json = File.ReadAllText(_filePath);
            }
            return JsonConvert.DeserializeObject(json);
        }
    }
}
