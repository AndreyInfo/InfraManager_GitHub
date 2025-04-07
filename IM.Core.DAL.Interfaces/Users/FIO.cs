namespace InfraManager.DAL.Users;

public record FIO
{
    public string Name { get; set; }
    
    public string Patronymic { get; set; }
    
    public string Surname { get; set; }
}