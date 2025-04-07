using Inframanager;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceCatalogue;

[ObjectClassMapping(ObjectClass.ServiceAttendance)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class ServiceAttendance : PortfolioServiceItemAbstract, IServiceItem, IFormBuilder
{
    public ServiceAttendance(string name) : base(name)
    { }
    public ServiceAttendance() : base()
    { }
    public string Parameter { get; init; }

    public bool Agreement { get; init; }

    public AttendanceType Type { get; init; }

    public byte[] RowVersion { get; init; }

    public string WorkflowSchemeIdentifier { get; init; }

    public string Note { get; init; }

    public string ExternalID { get; init; }

    public string Summary { get; init; }

    public Guid? FormID { get; init; }

    public virtual Form Form { get; }


    public virtual ICollection<CallSummary> CallSummary { get; }
    public static Expression<Func<ServiceAttendance, bool>> ServiceIsExternal =>
        x => x.Service.Type == ServiceType.External;
    public static Expression<Func<ServiceAttendance, bool>> ServiceWorkedOrBlocked =>
        x => Service.WorkedOrBlockedStates.Contains(x.Service.State);
}

