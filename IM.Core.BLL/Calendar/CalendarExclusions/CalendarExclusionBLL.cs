using AutoMapper;
using InfraManager.BLL.CrudWeb;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InfraManager.DAL.CalendarWork;
using InfraManager.DAL.ServiceCatalogue;
using Inframanager.BLL;
using Inframanager;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using InfraManager;

namespace InfraManager.BLL.Calendar.CalendarExclusions
{
    internal class CalendarExclusionBLL : ICalendarExclusionBLL,
        ISelfRegisteredService<ICalendarExclusionBLL>
    {
        private readonly IRepository<CalendarExclusion> _repository;
        private readonly IRepository<ServiceReference> _serviceReferenceRepository;
        private readonly IReadonlyRepository<CalendarExclusion> _query;
        private readonly IFinder<CalendarExclusion> _finder;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IMapper _mapper;
        private readonly ICalendarExclusionQuery _calendarExclusionQuery;
        private readonly IValidatePermissions<CalendarExclusion> _validatePermissions;
        private readonly ILogger<CalendarExclusionBLL> _logger;
        private readonly ICurrentUser _currentUser;

        public CalendarExclusionBLL(IRepository<CalendarExclusion> repository,
            IReadonlyRepository<CalendarExclusion> query,
            IFinder<CalendarExclusion> finder,
            IUnitOfWork saveChangesCommand,
            IMapper mapper,
            ICalendarExclusionQuery calendarExclusionQuery,
            IRepository<ServiceReference> serviceReferenceRepository,
            IValidatePermissions<CalendarExclusion> validatePermissions,
            ILogger<CalendarExclusionBLL> logger,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _query = query;
            _finder = finder;
            _mapper = mapper;
            _saveChangesCommand = saveChangesCommand;
            _calendarExclusionQuery = calendarExclusionQuery;
            _serviceReferenceRepository = serviceReferenceRepository;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<CalendarExclusionDetails> UpdateAsync(CalendarExclusionDetails calendarExclusion, Guid id,
            CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

            var foundEntity = await _finder.FindAsync(id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, "Not found CalendarExclusion");

            _mapper.Map(calendarExclusion, foundEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return calendarExclusion;
        }

        public async Task<CalendarExclusionDetails> AddAsync(CalendarExclusionInsertDetails calendarExclusion, CancellationToken cancellationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

            var entity = _mapper.Map<CalendarExclusion>(calendarExclusion);
            _repository.Insert(entity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return _mapper.Map<CalendarExclusionDetails>(entity);
        }

        public async Task<List<string>> DeleteAsync(DeleteModel<Guid>[] deleteModels, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

            var ids = deleteModels.Select(x => x.ID).ToList();
            var entitiesForDelete = _query.Query().Where(x => ids.Contains(x.ID)).ToList();
            var notDeletedNames = new List<string>();

            foreach (var calendarExclusion in entitiesForDelete)
            {
                var deleteModel = deleteModels.First(x => x.ID == calendarExclusion.ID);

                try
                {
                    _repository.Delete(calendarExclusion);
                }
                catch
                {
                    notDeletedNames.Add(deleteModel.Name);
                }
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);
            return notDeletedNames;
        }

        public async Task<CalendarExclusionDetails> GetByIDAsync(Guid exclusionId, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

            var foundEntity = await _finder.FindAsync(exclusionId, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(exclusionId, "Not found CalendarExclusion");

            if (foundEntity == null)
                return null;

            var result = _mapper.Map<CalendarExclusionDetails>(foundEntity);

            return result;
        }

        public async Task<CalendarExclusionDetails[]> GetByFilterAsync(BaseFilterWithClassIDAndID<Guid> filter, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var calendarExlusions = GetCalendarExclusions(filter);
            var result = await FullMappingToCalendarExclusionDTOAsync(calendarExlusions, cancellationToken);
            return result;
        }

        private CalendarExclusion[] GetCalendarExclusions(BaseFilterWithClassIDAndID<Guid> filter)
        {
            CalendarExclusion[] result;
            if (filter.ClassID.HasValue && filter.ObjectID.HasValue)
                result = GetCalendarExclusionsByFilterAsync(filter, c => c.ObjectClassID == filter.ClassID && c.ObjectID == filter.ObjectID);
            else
                result = GetCalendarExclusionsByFilterAsync(filter);

            return result;
        }

        private CalendarExclusion[] GetCalendarExclusionsByFilterAsync(BaseFilter filter, Expression<Func<CalendarExclusion, bool>> predicate = null)
        {
            var query = _query.With(c => c.Exclusion)
                              .With(c => c.ServiceReference)
                              .AsQueryable();

            query = predicate is not null ? query.Where(predicate) : query;

            if (!string.IsNullOrEmpty(filter.SearchString))
                query = query.Where(c => c.Exclusion.Name.Contains(filter.SearchString));

            var calendarExlusions = query.Skip(filter.StartRecordIndex).Take(filter.CountRecords).ToArray();

            return calendarExlusions;
        }

        private async Task<CalendarExclusionDetails[]> FullMappingToCalendarExclusionDTOAsync(CalendarExclusion[] calendarExclusions, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<CalendarExclusionDetails[]>(calendarExclusions);
            await InitializateRelatedObjectNameAsync(mapped, cancellationToken);
            return mapped;
        }


        private async Task InitializateRelatedObjectNameAsync(CalendarExclusionDetails[] calendarExclusions, CancellationToken cancellationToken)
        {
            foreach (var item in calendarExclusions)
            {
                if (!item.ServiceReferenceID.HasValue)
                    continue;

                // TODO подумать как вызов хранимки на уровне бизнес логики, сейчас вызывается только в самом query
                // на уровне вызов функции не работает, добавить в select в формирование query в GetCalendarExclusionsByFilterAsync тоже не работает
                item.RelatedObjectName = await _calendarExclusionQuery.GetNameReferenceServiceAsync(item.ServiceReferenceID.Value, cancellationToken);
            }
        }

    }
}
