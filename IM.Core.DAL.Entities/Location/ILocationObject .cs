namespace InfraManager.DAL.Location
{
    public interface ILocationObject : IGloballyIdentifiedEntity
    {
        int ID { get; }
        string Name { get; set; }
        public string GetFullPath();
    }
}
