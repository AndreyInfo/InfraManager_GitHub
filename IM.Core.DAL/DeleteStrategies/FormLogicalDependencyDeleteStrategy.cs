using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.DAL.DeleteStrategies;

public class FormLogicalDependencyDeleteStrategy :
    IDependentDeleteStrategy<ProblemType>,
    IDependentDeleteStrategy<ChangeRequestType>,
    IDependentDeleteStrategy<LifeCycle>,
    ISelfRegisteredService<IDependentDeleteStrategy<ProblemType>>,
    ISelfRegisteredService<IDependentDeleteStrategy<ChangeRequestType>>,
    ISelfRegisteredService<IDependentDeleteStrategy<LifeCycle>>
{
    public void OnDelete(ProblemType entity)
    {
        entity.FormID = null;
    }

    public void OnDelete(ChangeRequestType entity)
    {
        entity.FormID = null;
    }

    public void OnDelete(LifeCycle entity)
    {
        entity.FormID = null;
    }
}