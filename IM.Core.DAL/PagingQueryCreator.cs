using System.Linq;

namespace InfraManager.DAL
{
    internal class PagingQueryCreator : IPagingQueryCreator
    {
        public IPagingQuery<T> Create<T>(IOrderedQueryable<T> initialQuery)
        {
            return new PagingQuery<T>(initialQuery);
        }
    }
}
