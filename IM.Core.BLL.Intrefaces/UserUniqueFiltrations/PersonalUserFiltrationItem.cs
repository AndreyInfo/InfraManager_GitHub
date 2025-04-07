namespace InfraManager.BLL.UserUniqueFiltrations;

public class PersonalUserFiltrationItem
{
    public string SearchColumnName { get; init; }
    public ComparisonType FiltrationAction { get; init; }
    public string[] SearchValues { get; init; }
}