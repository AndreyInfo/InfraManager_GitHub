using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Settings
{
    public class ExceptionsListItem
    {
        [ColumnSettings(0, 100)]
        [Display(nameof(Resources.Email))]
        public string Email { get { return default; } }
        [ColumnSettings(1, 100)]
        [Display(nameof(Resources.Action))]
        public string PolicyType { get { return default; } }
    }
}
