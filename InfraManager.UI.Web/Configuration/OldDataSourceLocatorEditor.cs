using System;
using System.IO;
using InfraManager.DAL;
using InfraManager.DAL.DbConfiguration;

namespace InfraManager.UI.Web.Configuration
{
    public class OldDataSourceLocatorEditor : IOldDataSourceLocatorEditor
    {
        public void Edit(string connectionString)
        {
            var fillepath =
                File.Exists(Path.Combine(AppContext.BaseDirectory, "settings", "old-datasource-locator.xml"))
                    ? Path.Combine(AppContext.BaseDirectory, "settings", "old-datasource-locator.xml")
                    : Path.Combine(AppContext.BaseDirectory, "old-datasource-locator.xml");

            File.WriteAllText(fillepath, connectionString);
        }
    }
}