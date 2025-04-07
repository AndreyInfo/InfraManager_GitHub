
using System.Collections.Generic;

namespace InfraManager.DAL.DbConfiguration
{
    public interface IAppSettingsEditor
    {
        void Edit<T>(string key, T value);
        string[] GetValues(string[] keys);
        void Edit<T>(List<(string key, T value)> parameters);
    }
}