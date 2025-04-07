using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceDesk
{
    internal class ServiceDeskListFilterPredicatesBuilder<TEntity> : 
        IBuildListViewFilterPredicates<TEntity, ServiceDeskListFilter>
        where TEntity : class, IWorkflowEntity
    {
        public ServiceDeskListFilterPredicatesBuilder()
        {
        }

        public IEnumerable<Expression<Func<TEntity, bool>>> Build(Guid userID, ServiceDeskListFilter filter)
        {
            var idList = filter.IDList ?? Array.Empty<Guid>();
            DateTime? modifiedAfter;
            if (!DateTimeExtensions
                .TryConvertFromMillisecondsAfterMinimumDate(
                    filter.AfterModifiedMilliseconds,
                    out modifiedAfter))
            {
                modifiedAfter = null;
            }

            return Array.Empty<Expression<Func<TEntity, bool>>>()
                .UnionIf(!filter.WithFinishedWorkflow, p => p.EntityStateID != null || p.WorkflowSchemeID != null || p.WorkflowSchemeVersion == null) // фильтр по WithFinishedWorkflow
                .UnionIf(modifiedAfter.HasValue, p => p.UtcDateModified >= modifiedAfter) // фильтр по ModifiedAfter
                .UnionIf(idList.Any(), p => filter.IDList.Contains(p.IMObjID));
        }
    }
}
