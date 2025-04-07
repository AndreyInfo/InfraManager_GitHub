using IM.Core.Import.BLL.Interface.Import.Models;

namespace IM.Core.Import.BLL.Interface.Import
{
    public record UserDetails : IUserDetails
    {
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string LoginName { get; set; }
        public int? PositionID { get; set; }
        public int? RoomID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Pager { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string SID { get; set; }
        public IEnumerable<string> SubdivisionName { get; set; }

        public string SubdivisionExternalID { get; set; }

        public Guid? SubdivisionID { get; set; }
        public int? WorkPlaceID { get; set; }
        public string Number { get; set; }
        
        public Guid? ManagerID { get; set; }

        public string ManagerIdentifier { get; set; }

        public string ParentOrganizationName { get; set; }
        
        public string ParentOrganizationExternalID { get; set; }

        private string? _externalID = string.Empty;

        public string ExternalID { get; set; }

        public string Phone1 { get; set; }
        public string FullName => string.Join(" ", Surname, Name, Patronymic);

        public ImportModel? ImportModel { get; set; }

        public IUserDetails? Manager { get; set; }

        public bool HasManager()
        {
            return ManagerID.HasValue;
        }
    }
}
