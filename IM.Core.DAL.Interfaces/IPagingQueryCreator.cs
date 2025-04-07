using System.Linq;

namespace InfraManager.DAL
{
    public interface IPagingQueryCreator
    {
        IPagingQuery<T> Create<T>(IOrderedQueryable<T> initialQuery);
    }
}
