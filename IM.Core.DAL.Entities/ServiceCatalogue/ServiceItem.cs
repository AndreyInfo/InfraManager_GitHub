using Inframanager;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceCatalogue;

[ObjectClassMapping(ObjectClass.ServiceItem)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class ServiceItem : PortfolioServiceItemAbstract, IServiceItem, IFormBuilder
{
    public ServiceItem(string name) : base(name) 
    {
    }
    public ServiceItem() : base()
    { }
    public byte[] RowVersion { get; init; }
    public string Parameter { get; init; }
    public string Note { get; init; }
    public string ExternalID { get; init; }
    public Guid? FormID { get; init; }
    public virtual Form Form { get; }

    public virtual ICollection<CallSummary> CallSummary { get; }
    public static Expression<Func<ServiceItem, bool>> ServiceIsExternal =>
        x => x.Service.Type == ServiceType.External;
    public static Expression<Func<ServiceItem, bool>> ServiceWorkedOrBlocked =>
        x => Service.WorkedOrBlockedStates.Contains(x.Service.State);
}
