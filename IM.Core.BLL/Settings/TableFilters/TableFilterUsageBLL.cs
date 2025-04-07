using InfraManager.DAL;
using InfraManager.DAL.Settings;
using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class TableFilterUsageBLL : ITableFilterUsageBLL, ISelfRegisteredService<ITableFilterUsageBLL>
    {
        private readonly IRepository<WebFilterUsing> _repository;
        private readonly IFinder<WebFilterUsing> _finder;
        private readonly IUnitOfWork _saveChangesCommand;

        public TableFilterUsageBLL(
            IRepository<WebFilterUsing> repository, 
            IFinder<WebFilterUsing> finder, 
            IUnitOfWork saveChangesCommand)
        {
            _repository = repository;
            _finder = finder;
            _saveChangesCommand = saveChangesCommand;
        }

        public async Task InsertAsync(Guid userId, Guid filterId)
        {
            var usage = await _finder.FindAsync(new object[] { filterId, userId });

            if (usage == null)
            {
                usage = new WebFilterUsing
                {
                    FilterId = filterId,
                    UserId = userId
                };
                _repository.Insert(usage);
            }

            usage.UtcDateLastUsage = DateTime.UtcNow;
            await _saveChangesCommand.SaveAsync();
        }
    }
}
