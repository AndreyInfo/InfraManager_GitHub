using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Settings.TableFilters
{
    public enum PatternSearchOperation : byte
    {
        [Display(nameof(Resources.FilterOperationContains))]
        Contains = 0,
        [Display(nameof(Resources.FilterOperationEqual))]
        Equal
    }
}
