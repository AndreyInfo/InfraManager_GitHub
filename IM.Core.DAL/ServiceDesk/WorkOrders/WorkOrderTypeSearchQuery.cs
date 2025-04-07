using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    internal class WorkOrderTypeSearchQuery :
        IWorkOrderTypeSearchQuery,
        ISelfRegisteredService<IWorkOrderTypeSearchQuery>
    {
        private readonly DbSet<WorkOrderType> _workOrderTypes;

        public WorkOrderTypeSearchQuery(DbSet<WorkOrderType> workOrderTypes)
        {
            _workOrderTypes = workOrderTypes;
        }

        public IQueryable<ObjectSearchResult> Query(Guid currentUserId, WorkOrderTypeSearchCriteria searchBy)
        {
            var searchQuery = _workOrderTypes
                .AsNoTracking()
                .AsQueryable();

            if (searchBy.TypeClass.HasValue)
            {
                searchQuery = searchQuery.Where(x => x.TypeClass == searchBy.TypeClass);
            }

            if (!string.IsNullOrWhiteSpace(searchBy.Text))
            {
                var searchText = searchBy.Text.ToContainsPattern();
                searchQuery = searchQuery.Where(
                    x => EF.Functions.Like(x.Name, searchText));
            }

            return searchQuery
                .Select(
                    x => new ObjectSearchResult
                    {
                        ID = x.ID,
                        ClassID = ObjectClass.WorkOrderType,
                        FullName = x.Name
                    });
        }
    }
}
