using Inframanager.BLL.ListView;
using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceDesk
{
    internal class CallFromMeListFilterPredicatesBuilder<TEntity> : 
        IBuildListViewFilterPredicates<TEntity, CallFromMeListFilter>
        where TEntity : IHaveUtcModifiedDate, IWorkflowEntity
    {
        public IEnumerable<Expression<Func<TEntity, bool>>> Build(Guid userID, CallFromMeListFilter filter)
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

            if (!filter.WithFinishedWorkflow) yield return p =>
                p.EntityStateID != null || p.WorkflowSchemeID != null || p.WorkflowSchemeVersion == null;

            if (modifiedAfter.HasValue) yield return p => p.UtcDateModified >= modifiedAfter;
            if (idList.Any()) yield return p => filter.IDList.Contains(p.IMObjID);
        }
    }
}