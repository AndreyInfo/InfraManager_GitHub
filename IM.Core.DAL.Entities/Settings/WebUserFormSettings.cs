using System;

namespace InfraManager.DAL.Settings
{
    public class WebUserFormSettings
    {
        protected WebUserFormSettings()
        {
        }

        public WebUserFormSettings(Guid userId, string formName) : this()
        {
            UserId = userId;
            FormName = formName;
        }

        public Guid UserId { get; init; }
        public string FormName { get; init; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public FormSizeMode Mode { get; set; }
    }
}
