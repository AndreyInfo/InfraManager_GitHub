using System;

namespace InfraManager.UI.Web.Models.Settings
{
    public class SplitterSettingsModel 
    {
        public SplitterSettingsModel(Guid userID, string name)
        {
            UserID = userID;
            Name = name;
        }
        public Guid UserID { get; private set; }
        public string Name { get; private set; }
        public int Distance { get; set; }
    }
}
