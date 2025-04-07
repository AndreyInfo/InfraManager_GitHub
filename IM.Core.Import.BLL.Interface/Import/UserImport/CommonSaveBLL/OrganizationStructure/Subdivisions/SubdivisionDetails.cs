using InfraManager.DAL.Import;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IM.Core.Import.BLL.Interface.Import
{
    public record SubdivisionDetails : ISubdivisionDetails
    {
        public Guid? OrganizationID { get; set; }
        
        public string OrganisationName { get; set; }
        
        public string OrganisationExternalId { get; set; }
        
        //решил сделать разными классами, чтобы не было зависимости между организациями и подразделениями
        public OrganizationNameThenExternalIdKey OrganizationOrganizationNameThenExternalIdKey { get; set; }
        
        public SubdivisionNameThenExternalIdKey SubdivisionOrganizationNameThenExternalIdKey { get; set; }
            
        public string Name { get; set; }
        public Guid? SubdivisionID { get; set; }
        
        public string ParentExternalID { get; set; }

        public ISubdivisionDetails ParentSubdivision { get; set; }

        public string Note { get; set; }
        public string ExternalID { get; set; }
        public Guid? PeripheralDatabaseID { get; set; }
        public Guid? ComplementaryID { get; set; }
        public Guid? CalendarWorkScheduleID { get; set; }
        public bool? IsLockedForOsi { get; set; }
        public bool It { get; set; }
        
        public IEnumerable<string> SubdivisionParent { get; set; }

        public bool IsValid()
        {
            var anyOrganization = !(string.IsNullOrWhiteSpace(OrganisationName) &&
                                    string.IsNullOrWhiteSpace(OrganisationExternalId)
                                    && string.IsNullOrWhiteSpace(OrganizationOrganizationNameThenExternalIdKey?.Name) &&
                                    string.IsNullOrWhiteSpace(OrganizationOrganizationNameThenExternalIdKey?.ExternalID));
            if (!anyOrganization)
                return false;

            return !(string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(ExternalID)
                                                     && string.IsNullOrWhiteSpace(
                                                         SubdivisionOrganizationNameThenExternalIdKey.Name)
                                                     && string.IsNullOrWhiteSpace(
                                                         SubdivisionOrganizationNameThenExternalIdKey.ExternalID));
        }

        public bool IsNoSubdivision()
        {
            return SubdivisionID == null;
        }

        public IEnumerable<string> ParentFullName { get; init; }
    }
}
