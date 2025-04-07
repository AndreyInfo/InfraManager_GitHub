namespace InfraManager.UI.Web.Models.Filters
{
    public enum FilterType
    {
        SimpleValue = 0,
        SliderValue, //не задействован
        SliderRange,
        DatePick,
        SelectorValue, //ушел в обработку деревьев фильтрации
        SelectorMultiple,
        LikeValue,
        FuncSelectorValue, //ушел в обработку деревьев фильтрации
    }
}
