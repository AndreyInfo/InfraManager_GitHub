using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.DeleteStrategies
{
    public class WebFilterDeleteStrategy : 
        IDeleteStrategy<WebFilter>, 
        ISelfRegisteredService<IDeleteStrategy<WebFilter>>
    {
        private readonly IRepository<WebFilterElement> _filterElements;
        private readonly IRepository<WebFilterUsing> _filterUsages;
        private readonly DbSet<WebFilter> _filters;

        public WebFilterDeleteStrategy(
            IRepository<WebFilterElement> filterElements,
            IRepository<WebFilterUsing> filterUsages,
            DbSet<WebFilter> filters)
        {
            _filterElements = filterElements;
            _filterUsages = filterUsages;
            _filters = filters;
        }

        public void Delete(WebFilter entity) // TODO: This is better be done at database level
        {
            foreach (var element in entity.Elements)
            {
                _filterElements.Delete(element);
            }

            foreach (var usage in _filterUsages.Query().Where(x => x.FilterId == entity.Id).ToArray())
            {
                _filterUsages.Delete(usage);
            }

            _filters.Remove(entity);
        }
    }
}
