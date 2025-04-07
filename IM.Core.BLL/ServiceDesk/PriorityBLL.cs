using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.DAL;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk
{
    //TODO переписать
    internal class PriorityBLL : IPriorityBLL, ISelfRegisteredService<IPriorityBLL>
    {
        private readonly IRepository<Priority> _repository;
        private readonly IFinder<Priority> _finder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IWorkOrderDataProvider _workOrderDataProvider;
        private readonly IMapper _mapper;
        private readonly IPagingQueryCreator _paging;


        public PriorityBLL(
            IRepository<Priority> repository,
            IFinder<Priority> finder,
            IUnitOfWork saveChangesCommand,
            IWorkOrderDataProvider workOrderDataProvider,
            IMapper mapper,
            IPagingQueryCreator paging)
        {
            _repository = repository;
            _finder = finder;
            _saveChangesCommand = saveChangesCommand;
            _workOrderDataProvider = workOrderDataProvider;
            _mapper = mapper;
            _paging = paging;
        }

        public async Task<PriorityDetailsModel[]> ListAsync(LookupListFilterModel filterBy, CancellationToken cancellationToken = default)
        {

            var query = _repository.Query(x => !x.Removed);

            if (!string.IsNullOrEmpty(filterBy.SearchName))
                query = query.Where(s => s.Name.ToLower().Contains(filterBy.SearchName.ToLower()));

            var paggingQuery = _paging.Create(query.OrderBy(x => x.Sequence));

            var result = await paggingQuery.PageAsync(filterBy.Skip, filterBy.Take, cancellationToken);

            return _mapper.Map<PriorityDetailsModel[]>(result);
        }

        public async Task<PriorityDetailsModel> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var priority = await FindOrRaiseErrorAsync(id, cancellationToken);
            return _mapper.Map<PriorityDetailsModel>(priority);
        }

        public Task<PriorityDetailsModel> AddAsync(PriorityModel model, CancellationToken cancellationToken = default)
        {
            var newPriority = _mapper.Map<Priority>(model);
            _repository.Insert(newPriority);

            return CreateEventAndSaveAsync(newPriority, oldValue: null);
        }

        public async Task<PriorityDetailsModel> UpdateAsync(Guid id, PriorityModel model, CancellationToken cancellationToken = default)
        {
            var priority = await FindOrRaiseErrorAsync(id, cancellationToken);
            _mapper.Map(model, priority);

            return await CreateEventAndSaveAsync(priority, model);
        }

        private async Task<PriorityDetailsModel> CreateEventAndSaveAsync(Priority entity, object oldValue)
        {
            //var eventResult = await _eventMaker.CreateEvent(oldValue, entity); //CRASHING CUZ NO COMPARE ATTRIBUTES
            //if (!eventResult.Success)
            //    throw new Exception($"EventMaker crashed with fault = {eventResult.Fault}");
            //_events.Insert(eventResult.Result);

            await _saveChangesCommand.SaveAsync();
            return _mapper.Map<PriorityDetailsModel>(entity);
        }

        private async Task<Priority> FindOrRaiseErrorAsync(Guid id, CancellationToken cancellationToken)
        {
            var priority = await _finder.FindAsync(id, cancellationToken);
            return priority ?? throw new ObjectNotFoundException($"Priority (ID = {id})");
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var priority = await FindOrRaiseErrorAsync(id, cancellationToken);
            var oldValue = _mapper.Map<Priority>(priority);
            _repository.Delete(priority); // будет помечено на удаление, а не удалено

            var allPriorities = await _repository.ToArrayAsync();
            var defaultPriority = allPriorities.FirstOrDefault(p => p.Default)
                ?? allPriorities.OrderBy(p => p.Sequence).First();
            await _workOrderDataProvider.UpdateToDefultPriorityByPriorityIdAsync(id, defaultPriority.ID);

            //var eventResult = await _eventMaker.CreateEvent(priority, oldValue); //CRASHING CUZ NO COMPARE ATTRIBUTES
            //if (!eventResult.Success)
            //    throw new Exception($"EventMaker crashed with fault = {eventResult.Fault}");
            //_events.Insert(eventResult.Result);
        }

        public async Task<bool> SaveOrUpdateAsync(PriorityDetailsModel priority, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _repository.FirstOrDefaultAsync(x => x.ID != priority.ID && x.Name == priority.Name, cancellationToken);

            if (existingEntity != null)
            {
                throw new InvalidObjectException("Приоритет с таким именем уже существует");
            }

            if (priority.ID == Guid.Empty)
            {
                await AddAsync(priority, cancellationToken);
            }
            else
            {
                await UpdateAsync(priority.ID, priority, cancellationToken);
            }

            return true;
        }
    }
}