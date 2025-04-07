
namespace IM.Core.Import.BLL.Interface
{
    public class WorkplaceModel
    {
        public WorkplaceModel(string name, Guid? subdivisionID, int? roomID)
        {
            Name = name;
            IMObjID = Guid.NewGuid();
            SubdivisionID = subdivisionID;
            RoomID = roomID;
        }

        public WorkplaceModel(string name)
        {
            Name=name;
            ExternalId = Guid.NewGuid().ToString();
            IMObjID = Guid.NewGuid();
        }
        
        public string? Name { get; set; }
        public Guid? IMObjID { get; set; }
        public string? Note { get; set; }
        public string? ExternalId { get; set; }
        public Guid? SubdivisionID { get; set; }
        public Guid? PeripheralDatabaseId { get; set; }
        public int? ComplementaryId { get; set; }
        public int? RoomID { get; set; }
        
        public Guid? OrganizationID { get; set; }
    }
}
