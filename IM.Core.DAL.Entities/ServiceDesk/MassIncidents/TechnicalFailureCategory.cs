using Inframanager;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents;

[ObjectClassMapping(ObjectClass.TechnicalFailuresCategoryType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.TechnicalFailuresCategoryType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.TechnicalFailuresCategoryType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.TechnicalFailuresCategoryType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.TechnicalFailuresCategoryType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.TechnicalFailuresCategoryType_Properties)]
public class TechnicalFailureCategory
{
    protected TechnicalFailureCategory()
    {
    }

    public TechnicalFailureCategory(string name) : this()
    {
        Name = name;
    }

    public int ID { get; }
    public Guid IMObjID { get; }
    public string Name { get; set; }
    public virtual ICollection<ServiceTechnicalFailureCategory> Services { get; } = new HashSet<ServiceTechnicalFailureCategory>();
    public static IBuildSpecification<TechnicalFailureCategory, Guid> AvailableForService =>
        new SpecificationBuilder<TechnicalFailureCategory, Guid>((x, serviceID) => x.Services.Any(s => s.Reference.ID == serviceID));
}