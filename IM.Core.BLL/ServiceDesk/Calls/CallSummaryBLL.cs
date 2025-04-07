using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.CrudWeb;
using InfraManager.BLL.ServiceDesk.DTOs;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using InfraManager.DAL.ServiceDesk.Calls;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallSummaryBLL : ICallSummaryBLL
        , ISelfRegisteredService<ICallSummaryBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CallSummary> _repository;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly ICurrentUser _currentUser;
        private readonly IValidatePermissions<CallSummary> _validatePermissions;
        private readonly ILogger<CallSummaryBLL> _logger;
        private readonly ICallSummaryQuery _callSummaryQuery;
        private readonly IUserColumnSettingsBLL _columnBLL;
        private readonly IColumnMapper<CallSummaryModelItem, CallSummaryListItem> _columnMapper;
        
        public CallSummaryBLL(
            IMapper mapper, IRepository<CallSummary> repository,
            IUnitOfWork saveChangesCommand,
            ICurrentUser currentUser,
            IValidatePermissions<CallSummary> validatePermissions,
            ILogger<CallSummaryBLL> logger,
            ICallSummaryQuery callSummaryQuery,
            IUserColumnSettingsBLL columnBLL,
            IColumnMapper<CallSummaryModelItem, CallSummaryListItem> columnMapper)
        {
            _mapper = mapper;
            _repository = repository;
            _saveChangesCommand = saveChangesCommand;
            _currentUser = currentUser;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _callSummaryQuery = callSummaryQuery;
            _columnBLL = columnBLL;
            _columnMapper = columnMapper;
        }


        // TODO объединить метод получения всего списка и таблицы в один, в идеале все закинуть в ICallSummaryQuery
        public async Task<CallSummaryDetails[]> GetListAsync(CallSummaryFilter filter, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var query = _repository.With(c => c.ServiceAttendance).ThenWith(c => c.Service)
                                   .With(c => c.ServiceItem).ThenWith(c => c.Service)
                                   .ThenWith(c => c.Category)
                                   .Query();

            query = GetQueryWithFilter(query, filter.ClassId, filter.ServiceId);

            if (!string.IsNullOrEmpty(filter.SearchString))
                query = query.Where(cs => cs.Name.ToLower()
                                                 .Contains(filter.SearchString.ToLower())
                                    || cs.ServiceItem.Name.ToLower()
                                                          .Contains(filter.SearchString.ToLower())
                                    || cs.ServiceAttendance.Name.ToLower()
                                                          .Contains(filter.SearchString.ToLower())
                                    || cs.ServiceItem.Service.Name.ToLower()
                                                          .Contains(filter.SearchString.ToLower())
                                    || cs.ServiceAttendance.Service.Name.ToLower()
                                                          .Contains(filter.SearchString.ToLower())
                                    || cs.ServiceItem.Service.Category.Name.ToLower()
                                                          .Contains(filter.SearchString.ToLower())
                                    || cs.ServiceAttendance.Service.Category.Name.ToLower()
                                                          .Contains(filter.SearchString.ToLower()));

            if (string.IsNullOrEmpty(filter.ViewName))
            {
                if (!string.IsNullOrEmpty(filter.SearchString))
                    query = query.Where(cs => cs.Name == filter.SearchString);

                var serviceItems = await query.ExecuteAsync(cancellationToken);

                return _mapper.Map<CallSummaryDetails[]>(serviceItems);
            }

            var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
            var orderColumn = columns.GetSortColumn();
            orderColumn.PropertyName = _columnMapper.MapFirst(orderColumn.PropertyName);
            var mappedValues = _columnMapper.MapToStringArray(orderColumn.PropertyName);

            var result = await _callSummaryQuery.ExecuteAsync(query, _mapper.Map<PaggingFilter>(filter), orderColumn, mappedValues, cancellationToken);
    
            return _mapper.Map<CallSummaryDetails[]>(result);
        }

        private IExecutableQuery<CallSummary> GetQueryWithFilter(IExecutableQuery<CallSummary> query, ObjectClass classID, Guid? objectID)
        {
            switch (classID)
            {
                case ObjectClass.ServiceItem:
                    {
                        return query.Where(p => p.ServiceItemID == objectID);
                    }
                case ObjectClass.ServiceAttendance:
                    {
                        return query.Where(p => p.ServiceAttendanceID == objectID);
                    }
                case ObjectClass.Service:
                    {
                       return query.Where(p => p.ServiceItem.ServiceID == objectID
                                                 || p.ServiceAttendance.ServiceID == objectID);
                    }
                case ObjectClass.ServiceCategory:
                    {
                        return query.Where(p => p.ServiceItem.Service.CategoryID == objectID
                                                 || p.ServiceAttendance.Service.CategoryID == objectID);
                    }
                default:
                        return query;
            }
        }

        //TODO разделить на два метода Add и Update
        public async Task<Guid> AddOrUpdateAsync(CallSummaryDetails callSummaryDto,
            CancellationToken cancellationToken = default)
        {
            if (callSummaryDto.ID == Guid.Empty)
            {
                var callSummary = new CallSummary();
                var entity = _mapper.Map(callSummaryDto, callSummary);
                entity.ServiceAttendance = null;
                entity.ServiceItem = null;
                entity.ID = Guid.NewGuid();
                _repository.Insert(callSummary);

                await _saveChangesCommand.SaveAsync(cancellationToken);
                return entity.ID;
            }

            var foundEntity = await _repository.FirstOrDefaultAsync(p => p.ID == callSummaryDto.ID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(callSummaryDto.ID, ObjectClass.CallSummary);

            await SaveCallSummaryAsync(callSummaryDto, foundEntity, cancellationToken);


            return foundEntity.ID;
        }
        private async Task<Guid> SaveCallSummaryAsync(CallSummaryDetails callSummaryDto, CallSummary foundEntity, CancellationToken cancellationToken)
        {
            foundEntity.Name = callSummaryDto.Name;
            foundEntity.RowVersion = callSummaryDto.RowVersion;
            foundEntity.ServiceAttendanceID = callSummaryDto.ServiceAttendanceID;
            foundEntity.ServiceItemID = callSummaryDto.ServiceItemID;
            foundEntity.Visible = callSummaryDto.Visible;
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return callSummaryDto.ID;
        }

        //TODO Единичное удаление
        public async Task<string[]> DeleteAsync(List<DeleteModel<Guid>> deleteModels,
            CancellationToken cancellationToken = default)
        {
            var ids = deleteModels.Select(x => x.ID).ToList();
            var entitiesForDelete = _repository.Query().Where(x => ids.Contains(x.ID)).ToList();
            var notDeletedNames = new List<string>();

            foreach (var entity in entitiesForDelete)
            {
                _repository.Delete(entity);
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);

            return notDeletedNames.ToArray();
        }
    }
}
