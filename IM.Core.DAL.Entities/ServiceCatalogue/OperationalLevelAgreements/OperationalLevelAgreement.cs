using System;
using System.Collections.Generic;
using System.Linq;
using Inframanager;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.OrganizationStructure;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;

[ObjectClassMapping(ObjectClass.OperationalLevelAgreement)]
[OperationIdMapping(ObjectAction.Delete, OperationID.OperationalLevelAgreement_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.OperationalLevelAgreement_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.OperationalLevelAgreement_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.OperationalLevelAgreement_View)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.OperationalLevelAgreement_View)]
public class OperationalLevelAgreement : IGloballyIdentifiedEntity, ITimeZoneObject
{
    public static int MaxNameLength = 255;
    public static int MaxNumberLength = 255;
    public static int MaxNoteLength = 4000;
    
    protected OperationalLevelAgreement()
    {
        
    }
    
    public OperationalLevelAgreement(string name)
    {
        Name = name;
    }

    public int ID { get; }
    public Guid IMObjID { get; }
    
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTime? UtcStartDate { get; set; }
    public DateTime? UtcFinishDate { get; set; }
    public string TimeZoneID { get; set; }
    public Guid? CalendarWorkScheduleID { get; set; }
    public string Note { get; set; }
    public Guid? FormID { get; set; }
    public byte[] RowVersion { get; set; }
    
    public virtual Form Form { get; init; }
    public virtual ICollection<ManyToMany<OperationalLevelAgreement, Service>> Services { get; private set; } =
        new HashSet<ManyToMany<OperationalLevelAgreement, Service>>();
    public virtual ICollection<OrganizationItemGroup> ConcludedWith { get; init; }
    public virtual TimeZone TimeZone { get; init; }
    public virtual CalendarWorkSchedule CalendarWorkSchedule { get; init; }
    public virtual ICollection<Rule> Rules { get; init; }
    
    public static Specification<OperationalLevelAgreement> IsActive { get; } = new (ola => 
        (ola.UtcStartDate == null || ola.UtcStartDate <= DateTime.UtcNow)
        && (ola.UtcFinishDate == null || ola.UtcFinishDate > DateTime.UtcNow));

    public static IBuildSpecification<OperationalLevelAgreement, Guid> IsRelatedToService { get; } =
        new SpecificationBuilder<OperationalLevelAgreement, Guid>((ola, serviceID) =>
            ola.Services.Any(s => s.Reference.ID == serviceID));
}