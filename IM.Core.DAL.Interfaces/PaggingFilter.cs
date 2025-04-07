namespace InfraManager.DAL;

public class PaggingFilter
{
    public int Take { get; init; }

    public int Skip { get; init; }

    public string SearchString { get; init; }
}
