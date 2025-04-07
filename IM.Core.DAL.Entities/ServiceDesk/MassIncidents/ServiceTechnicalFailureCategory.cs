using System;
using Inframanager;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;

[OperationIdMapping(ObjectAction.Insert, OperationID.ServiceCatalog_Update)]
[OperationIdMapping(ObjectAction.Update, OperationID.ServiceCatalog_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ServiceCatalog_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ServiceCatalog_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ServiceCatalog_Properties)]
public class ServiceTechnicalFailureCategory : ManyToMany<TechnicalFailureCategory, Service>
{
    protected ServiceTechnicalFailureCategory()
    {
    }

    public ServiceTechnicalFailureCategory(Service service, Guid groupID) : base(service)
    {
        GroupID = groupID;
    }

    [Obsolete("Абсолютно нет никакой необходимости в этом ключе, так как сущности many-to-many отлично идентифицируется парой FK основных таблиц")]
    public Guid IMObjID { get; }
    public Guid GroupID { get; }
    public virtual Group Group { get; }
}
