using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Settings.TableFilters
{
    public enum DateTimeSearchOperation : byte
    {
        [Display(nameof(Resources.FilterOperationEqual))]
        Equal = 0,
        [Display(nameof(Resources.FilterOperationBefore))]
        Before = 1,
        [Display(nameof(Resources.FilterOperationAfter))]
        After = 2,
        [Display(nameof(Resources.FilterOperationBetween))]
        Between = 3,
        [Display(nameof(Resources.FilterOperationExcept))]
        Except = 4,
        [Display(nameof(Resources.DateRanges_PresentHour))]
        PresentHour = 5,
        [Display(nameof(Resources.DateRanges_LastHour))]
        LastHour = 6,
        [Display(nameof(Resources.DateRanges_PastHour))]
        PastHour = 7,
        [Display(nameof(Resources.DateRanges_Today))]
        Today = 8,
        [Display(nameof(Resources.DateRanges_Yesterday))]
        Yesterday = 9,
        [Display(nameof(Resources.DateRanges_PresentWeek))]
        PresentWeek = 10,
        [Display(nameof(Resources.DateRanges_LastWeek))]
        LastWeek = 11,
        [Display(nameof(Resources.DateRanges_PastWeek))]
        PastWeek = 12,
        [Display(nameof(Resources.DateRanges_PresentMonth))]
        PresentMonth = 13,
        [Display(nameof(Resources.DateRanges_LastMonth))]
        LastMonth = 14,
        [Display(nameof(Resources.DateRanges_PastMonth))]
        PastMonth = 15,
        [Display(nameof(Resources.DateRanges_PresentQuarter))]
        PresentQuarter = 16,
        [Display(nameof(Resources.DateRanges_LastQuarter))]
        LastQuarter = 17,
        [Display(nameof(Resources.DateRanges_PastQuarter))]
        PastQuarter = 18,
        [Display(nameof(Resources.DateRanges_PresentYear))]
        PresentYear = 19,
        [Display(nameof(Resources.DateRanges_LastYear))]
        LastYear = 20,
        [Display(nameof(Resources.DateRanges_PastYear))]
        PastYear = 21,
        [Display(nameof(Resources.DateRanges_NextMonth))]
        NextMonth = 22,
        [Display(nameof(Resources.DateRanges_NextQuarter))]
        NextQuarter = 23,
        [Display(nameof(Resources.DateRanges_NextHalfYear))]
        NextHalfYear = 24,
        [Display(nameof(Resources.DateRanges_NextYear))]
        NextYear = 25
    }
}
