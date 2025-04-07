using Inframanager;
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceCatalogue;


[ObjectClassMapping(ObjectClass.ServiceCategory)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class ServiceCategory : Lookup
{
    public ServiceCategory(string name) : base(name)
    {
        Note = string.Empty;
        ExternalId = string.Empty;
        Services = new List<Service>();
    }
    public string Note { get; init; }
    public byte[] Icon { get; init; }
    public string ExternalId { get; init; }
    public string IconName { get; set; }
    public virtual ICollection<Service> Services { get; } //TODO: Убрать навигационное свойство
}
