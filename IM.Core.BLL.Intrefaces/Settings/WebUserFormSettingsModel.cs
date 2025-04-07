using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings
{
    public class WebUserFormSettingsModel
    {
        public WebUserFormSettingsModel()
        {
        }

        public WebUserFormSettingsModel(WebUserFormSettings settings) : this()
        {
            X = settings.X;
            Y = settings.Y;
            Width = settings.Width;
            Height = settings.Height;
            Mode = settings.Mode;
        }

        public int? X { get; init; }
        public int? Y { get; init; }
        public int? Width { get; init; }
        public int? Height { get; init; }
        public FormSizeMode? Mode { get; init; }
    }
}
