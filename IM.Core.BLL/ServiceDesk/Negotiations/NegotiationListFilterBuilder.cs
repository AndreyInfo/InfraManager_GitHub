using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationListFilterBuilder :
        IBuildListViewFilterPredicates<Negotiation, NegotiationListFilter>,
        ISelfRegisteredService<IBuildListViewFilterPredicates<Negotiation, NegotiationListFilter>>
    {
        public IEnumerable<Expression<Func<Negotiation, bool>>> Build(Guid userID, NegotiationListFilter filter)
        {
            if (!filter.WithFinishedWorkflow)
            {
                yield return Negotiation.NotFinished;
            }

            if (filter.IDList != null && filter.IDList.Any())
            {
                yield return n => filter.IDList.Contains(n.IMObjID);
            }
        }
    }

    internal class NegotiationListFilterBuilder<TEntity> :
        IBuildListViewFilterPredicates<TEntity, NegotiationListFilter>
        where TEntity : IGloballyIdentifiedEntity
    {
        private readonly IBuildListViewFilterPredicates<TEntity, ServiceDeskListFilter> _baseFilterPredicatesBuilder;
        private readonly IObjectClassProvider<TEntity> _objectClassProvider;

        public NegotiationListFilterBuilder(
            IBuildListViewFilterPredicates<TEntity, ServiceDeskListFilter> baseFilterPredicatesBuilder,
            IObjectClassProvider<TEntity> objectClassProvider)
        {
            _baseFilterPredicatesBuilder = baseFilterPredicatesBuilder;
            _objectClassProvider = objectClassProvider;
        }

        public IEnumerable<Expression<Func<TEntity, bool>>> Build(
            Guid userID, 
            NegotiationListFilter filter)
        {
            var classId = _objectClassProvider.GetObjectClass();

            return _baseFilterPredicatesBuilder.Build(
                userID, 
                new ServiceDeskListFilter 
                { 
                    AfterModifiedMilliseconds = filter.AfterModifiedMilliseconds,
                    WithFinishedWorkflow = filter.WithFinishedWorkflow 
                    // IDList не передаем, так как это IDList согласований, а не родительских объектов
                })
                .UnionIf(
                    filter.Parent.HasValue && filter.Parent.Value.ClassId == classId,
                    x => x.IMObjID == filter.Parent.Value.Id)
                .UnionIf(
                    filter.Parent.HasValue && filter.Parent.Value.ClassId != classId,
                    x => false); // Если это другой класс, то фильтруем все
        }
    }
}
