using IM.Core.Import.BLL.Interface.Import.Models;

namespace IM.Core.Import.BLL.Interface.Import;

public interface IUserDetails
{
    string Name { get; set; }
    string Patronymic { get; set; }
    string Surname { get; set; }
    string LoginName { get; set; }
    int? PositionID { get; set; }
    int? RoomID { get; set; }
    string Phone { get; set; }
    string Fax { get; set; }
    string Pager { get; set; }
    string Email { get; set; }
    string Note { get; set; }
    string SID { get; set; }
    
    IEnumerable<string> SubdivisionName { get; set; }
    
    string SubdivisionExternalID { get; set; }
    
    Guid? SubdivisionID { get; set; }
    int? WorkPlaceID { get; set; }
    string Number { get; set; }
    Guid? ManagerID { get; set; }
    string ManagerIdentifier { get; set; }
    
    string ParentOrganizationName { get; set; }
    
    string ExternalID { get; set; }
    string Phone1 { get; set; }
    string FullName { get; }
    
    public ImportModel? ImportModel { get; set; }
    string ParentOrganizationExternalID { get; set; }
    IUserDetails Manager { get; set; }

    bool HasManager();
}