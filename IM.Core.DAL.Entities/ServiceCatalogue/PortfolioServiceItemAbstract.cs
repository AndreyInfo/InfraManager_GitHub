using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceCatalogue;

public abstract class PortfolioServiceItemAbstract
{
    public PortfolioServiceItemAbstract(string name)
    {
        ID = Guid.NewGuid();
        Name = name;
    }
    public PortfolioServiceItemAbstract()
    {
        ID = Guid.NewGuid();
    }

    public Guid ID { get; }
    public string Name { get; init; }
    public CatalogItemState? State { get; init; }
    public Guid? ServiceID { get; init; }

    public virtual Service Service { get; init; }
    public static Expression<Func<T, bool>> WorkedOrBlocked<T>() where T : PortfolioServiceItemAbstract
    {
        var states = Service.WorkedOrBlockedStates.Cast<CatalogItemState?>().ToArray();
        return s => s.State == null || states.Contains(s.State);
    }

    private Func<PortfolioServiceItemAbstract, bool> _workedOrBlocked;
    public bool IsAvailable => (_workedOrBlocked ?? (_workedOrBlocked = WorkedOrBlocked<PortfolioServiceItemAbstract>().Compile()))(this);
}
