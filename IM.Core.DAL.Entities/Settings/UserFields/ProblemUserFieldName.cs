namespace InfraManager.DAL.Settings.UserFields;

public class ProblemUserFieldName : IUserFieldName
{
    public FieldNumber ID { get; init; }

    public string Name { get; set; }
}
