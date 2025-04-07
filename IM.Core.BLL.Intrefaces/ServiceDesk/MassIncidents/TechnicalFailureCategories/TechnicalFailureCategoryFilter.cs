using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;

public class TechnicalFailureCategoryFilter
{
    [Obsolete]
    public Guid? ServiceReferenceID { get; init; }
    public Guid? GlobalIdentifiers { get; init; }
    public Guid? ServiceID { get; init; }
}