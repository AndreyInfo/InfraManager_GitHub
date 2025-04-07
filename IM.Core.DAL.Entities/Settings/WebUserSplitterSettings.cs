using System;

namespace InfraManager.DAL.Settings
{
    /// <summary>
    /// Этот класс представляет сущность WebUserSplitterSettings (Ширина области. отделенная разделителем в интерфейсе пользователя)
    /// </summary>
    public class WebUserSplitterSettings
    {
        public WebUserSplitterSettings(Guid userID, string name)
        {
            UserID = userID;
            Name = name;
        }
        public Guid UserID { get; private set; }
        public string Name { get; private set; }
        public int Distance { get; set; }
    }
}
