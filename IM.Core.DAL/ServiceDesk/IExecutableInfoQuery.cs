using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{ 
    internal interface IExecutableInfoQuery<T>
    {
        IQueryable<ExecutableInfo> Query(IQueryable<T> initialQuery);
    }
}
