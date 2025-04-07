using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Settings.TableFilters
{
    public enum RangeSearchOperation : byte
    {
        [Display(nameof(Resources.FilterOperationEqual))]
        Equal = 0,
        [Display(nameof(Resources.FilterOperationNotEqual))]
        NotEqual = 1,
        [Display(nameof(Resources.FilterOperationMore))]
        More = 2,
        [Display(nameof(Resources.FilterOperationLess))]
        Less = 3,
        [Display(nameof(Resources.FilterOperationBetween))]
        Between = 4,
        [Display(nameof(Resources.FilterOperationExcept))]
        Except = 5,
    }
}
