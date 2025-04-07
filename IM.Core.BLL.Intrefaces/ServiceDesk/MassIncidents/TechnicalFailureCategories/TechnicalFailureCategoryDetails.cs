using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;

public class TechnicalFailureCategoryDetails : TechnicalFailureCategoryData
{
    public int ID { get; init; }
    public Guid IMObjID { get; init; }
    public ServiceReferenceDetails[] ServiceReferences { get; init; }
}