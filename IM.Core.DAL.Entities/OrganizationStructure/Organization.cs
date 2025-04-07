using Inframanager;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.DAL.OrganizationStructure
{
    [ObjectClassMapping(ObjectClass.Organizaton)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Organization_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Organization_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Organization_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Organization_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Organization_Properties)]
    public partial class Organization : Catalog<Guid>, ILockableForOsi
    {
        private static readonly Guid EmptyOrganizationGlobalIdentifier = new("00000000-0000-0000-0000-000000000001");

        public string Note { get; set; }
        public string ExternalId { get; set; }
        public Guid? PeripheralDatabaseId { get; set; }
        public Guid? ComplementaryId { get; set; }
        public Guid? CalendarWorkScheduleId { get; set; }
        public bool? IsLockedForOsi { get; set; }

        [Obsolete("Use IRepository<Subdivision>.Where(x => x.Organization.ID == organizationID) instead")]
        public virtual ICollection<Subdivision> Subdivisions { get; init; }

        [Obsolete("Use IRepository<Building>.Where(x => x.Organization.ID == organizationID) instead")]
        public virtual ICollection<Building> Buildings{ get; init; }

        public static Expression<Func<Organization, bool>> ExceptEmptyOrganization =>
            organization => organization.ID != EmptyOrganizationGlobalIdentifier;
    }
}