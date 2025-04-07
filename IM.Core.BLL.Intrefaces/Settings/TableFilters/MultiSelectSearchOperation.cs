using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Settings.TableFilters
{
    public enum MultiSelectSearchOperation : byte
    {
        [Display(nameof(Resources.FilterOperationEqual))]
        Equal = 0,
        [Display(nameof(Resources.FilterOperationNotEqual))]
        NotEqual
    }
}
