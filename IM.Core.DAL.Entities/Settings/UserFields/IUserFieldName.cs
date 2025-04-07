namespace InfraManager.DAL.Settings.UserFields;

public interface IUserFieldName
{
    public FieldNumber ID { get; }
    public string Name { get; set; }
}
