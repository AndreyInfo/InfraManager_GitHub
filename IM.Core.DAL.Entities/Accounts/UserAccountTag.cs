namespace InfraManager.DAL.Accounts;

public class UserAccountTag
{
    public int UserAccountID { get; init; }
    public int TagID { get; init; }

    public virtual Tag Tag { get; init; }
}
