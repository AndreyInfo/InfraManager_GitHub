namespace InfraManager.DAL;

public interface IImportModelParameters
{

    string Note { get; set; }
    
    string ProductNumber { get; set; }
    
    bool CanBuy { get; set; }
}