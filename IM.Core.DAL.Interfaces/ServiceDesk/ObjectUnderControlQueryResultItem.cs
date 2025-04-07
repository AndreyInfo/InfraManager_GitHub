using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class ObjectUnderControlQueryResultItem : IssueQueryResultItem
    {        
        public Guid? ClientSubdivisionID { get; init; }
        public Guid? ClientOrganizationID { get; init; }
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public bool CanBePicked { get; init; }
    }
}
