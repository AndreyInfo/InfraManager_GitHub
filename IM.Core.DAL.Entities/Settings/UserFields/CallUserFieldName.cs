namespace InfraManager.DAL.Settings.UserFields;

public class CallUserFieldName : IUserFieldName
{
    public FieldNumber ID { get; init; }

    public string Name { get; set; }
}
