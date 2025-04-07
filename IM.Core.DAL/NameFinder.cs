using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    public class NameFinder<TEntity> : IFindName where TEntity : INamedEntity
    {
        private readonly IFinder<TEntity> _finder;

        public NameFinder(IFinder<TEntity> finder)
        {
            _finder = finder;
        }

        public string Find(params object[] keys)
        {
            var entity = _finder.Find(keys);
            return GetNameOrDefault(entity);
        }

        public async Task<string> FindAsync(object[] keys, CancellationToken token = default)
        {
            var entity = await _finder.FindAsync(keys, token);
            return GetNameOrDefault(entity);
        }

        internal static string GetNameOrDefault(TEntity entity) => entity == null ? null : entity.GetName() ?? string.Empty;
    }
}
