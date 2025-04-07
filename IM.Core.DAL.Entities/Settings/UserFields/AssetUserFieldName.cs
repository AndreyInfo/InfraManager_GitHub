namespace InfraManager.DAL.Settings.UserFields;

public class AssetUserFieldName : IUserFieldName
{
    public FieldNumber ID { get; init; }
    public string Name { get; set; }
}
