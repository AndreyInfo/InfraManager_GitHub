
namespace IM.Core.Import.BLL.Interface.Import
{
    public class OrganizationDetails 
    {
        public string? Name { get;set; }
        public string Note { get; set; }
        public string? ExternalId { get; set; }
        public Guid? PeripheralDatabaseId { get; set; }
        public Guid? ComplementaryId { get; set; }
        public Guid? CalendarWorkScheduleId { get; set; }
    }
}
