namespace InfraManager.DAL.Location;

public class BuildingSubnet
{
    public int ID { get; init; }
    
    public int BuildingID { get; init; }

    public string Subnet { get; init; }

    public virtual Building Building { get; }
}
