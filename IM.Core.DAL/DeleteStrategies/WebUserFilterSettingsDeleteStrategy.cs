using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class WebUserFilterSettingsDeleteStrategy :
        IDeleteStrategy<WebUserFilterSettings>,
        ISelfRegisteredService<IDeleteStrategy<WebUserFilterSettings>>
    {
        private readonly IRepository<WebFilter> _filters;
        private readonly DbSet<WebUserFilterSettings> _userFilters;

        public WebUserFilterSettingsDeleteStrategy(
            IRepository<WebFilter> filters, 
            DbSet<WebUserFilterSettings> userFilters)
        {
            _filters = filters;
            _userFilters = userFilters;
        }

        public void Delete(WebUserFilterSettings entity)
        {
            if (entity.Filter != null)
            {
                _filters.Delete(entity.Filter);
            }

            _userFilters.Remove(entity);
        }
    }
}
