using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Inframanager;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.DAL.ServiceCatalogue
{
    /// <summary>
    /// Этот класс представляет сущность SLA
    /// </summary>

    [ObjectClassMapping(ObjectClass.SLA)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.SLA_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.SLA_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.SLA_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.SLA_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SLA_Properties)]
    public class ServiceLevelAgreement : ITimeZoneObject
    {
        protected ServiceLevelAgreement()
        {
        }

        public ServiceLevelAgreement(string name)
        {
            ID = Guid.NewGuid();
            Name = name;
        }

        public ServiceLevelAgreement(string name, Guid? formID, string note, string number, DateTime? utcStartDate,
            DateTime? utcFinishDate, string timeZoneID, Guid? calendarWorkScheduleID,
            ICollection<SLAServiceReference> serviceReferences, ICollection<SLAReference> references,
            ICollection<Rule> rules, ICollection<OrganizationItemGroup> organizationItemGroups) : this(name)
        {
            FormID = formID;
            Note = note;
            Number = number;
            UtcStartDate = utcStartDate;
            UtcFinishDate = utcFinishDate;
            TimeZoneID = timeZoneID;
            CalendarWorkScheduleID = calendarWorkScheduleID;
            ServiceReferences = serviceReferences;
            References = references;
            Rules = rules;
            OrganizationItemGroups = organizationItemGroups;
        }


        public Guid ID { get; init; }
        public Guid? FormID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Number { get; set; }
        public DateTime? UtcStartDate { get; set; }
        public DateTime? UtcFinishDate { get; set; }
        public string TimeZoneID { get; set; }
        public Guid? CalendarWorkScheduleID { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual ICollection<SLAServiceReference> ServiceReferences { get; init; }
        public virtual ICollection<SLAReference> References { get; init; }
        public virtual ICollection<Rule> Rules { get; set; }
        public virtual CalendarWorkSchedule CalendarWorkSchedule { get; init; }
        public virtual TimeZone TimeZone { get; init; }
        public virtual ICollection<OrganizationItemGroup> OrganizationItemGroups { get; set; } =
            new HashSet<OrganizationItemGroup>();
        
        public static Specification<ServiceLevelAgreement> IsActive { get; } = new (sla => 
            (sla.UtcStartDate == null || sla.UtcStartDate <= DateTime.UtcNow)
            && (sla.UtcFinishDate == null || sla.UtcFinishDate > DateTime.UtcNow));

        public static IBuildSpecification<ServiceLevelAgreement, Guid[]> IsRelatedToOrganizationGroups { get; } =
            new SpecificationBuilder<ServiceLevelAgreement, Guid[]>((sla, itemIDs)
                => sla.OrganizationItemGroups.Any(x => itemIDs.Contains(x.ItemID)));


        public ServiceLevelAgreement Clone(string name, Guid? formID, string note, string number,
            DateTime? utcStartDate, DateTime? utcFinishDate, string timeZoneID, Guid? calendarWorkScheduleID)
        {
            var clonedSLA =
                new ServiceLevelAgreement(name, formID, note, number, utcStartDate, utcFinishDate, timeZoneID,
                    calendarWorkScheduleID, ServiceReferences, References, Rules, OrganizationItemGroups);
            
            foreach (var serviceReference in clonedSLA.ServiceReferences)
            {
                serviceReference.SLAID = Guid.Empty;
            }

            foreach (var reference in clonedSLA.References)
            {
                reference.SLAID = Guid.Empty;
            }

            var clonedRules = new List<Rule>();
            foreach (var rule in Rules)
            {
                rule.ServiceLevelAgreementID = Guid.Empty;
                clonedRules.Add(new Rule(rule));
            }
            clonedSLA.Rules = clonedRules;

            var clonedItemGroups = new List<OrganizationItemGroup>();
            foreach (var organizationItemGroup in clonedSLA.OrganizationItemGroups)
            {
                clonedItemGroups.Add(new OrganizationItemGroup(Guid.Empty, organizationItemGroup.ItemID,
                    organizationItemGroup.ItemClassID));
            }
            clonedSLA.OrganizationItemGroups = clonedItemGroups;

            return clonedSLA;
        }
    }
}
