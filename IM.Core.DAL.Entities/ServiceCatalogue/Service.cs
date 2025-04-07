using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Inframanager;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;

namespace InfraManager.DAL.ServiceCatalogue;

[ObjectClassMapping(ObjectClass.Service)]
[OperationIdMapping(ObjectAction.Insert, OperationID.None)]
[OperationIdMapping(ObjectAction.Update, OperationID.None)]
[OperationIdMapping(ObjectAction.Delete, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
public class Service : Catalog<Guid>
{
    public Service()
    {
        Type = ServiceType.External;
    }

    public Service(string name, ServiceType serviceType, CatalogItemState state, string externalID, Guid categoryID)
    {
        ID = Guid.NewGuid();
        Name = name;
        Type = serviceType;
        State = state;
        ExternalID = externalID;
        CategoryID = categoryID;
    }

    public Guid? CategoryID { get; set; }

    public ServiceType Type { get; init; }

    public CatalogItemState State { get; init; }

    public ObjectClass? OrganizationItemClassID { get; init; }

    public Guid? OrganizationItemObjectID { get; init; }

    public byte[] RowVersion { get; init; }

    public string Note { get; init; }

    public Guid? CriticalityID { get; init; }

    public string ExternalID { get; init; }

    public ObjectClass? OrganizationItemClassIDCustomer { get; init; }

    public Guid? OrganizationItemObjectIDCustomer { get; init; }

    public string IconName { get; init; }


    public virtual ServiceCategory Category { get; init; }

    public virtual ICollection<ServiceDependency> ServiceChildDependency { get; init; }

    public virtual ICollection<ServiceDependency> ServiceParentDependency { get; init; }


    public string FullLocation => Category == null ? Name : $"{Category.Name} \\ {Name}";
    
    public static Expression<Func<Service, bool>> IsExternal =>
        x => x.Type == ServiceType.External;


    public static CatalogItemState[] WorkedOrBlockedStates = 
        new[] { CatalogItemState.Blocked, CatalogItemState.Worked };
    public static Expression<Func<Service, bool>> WorkedOrBlocked =>
        x => WorkedOrBlockedStates.Contains(x.State);

    private Func<Service, bool> _workedOrBlocked = null;
    public bool IsAvailable => (_workedOrBlocked ?? (_workedOrBlocked = WorkedOrBlocked.Compile()))(this);
}