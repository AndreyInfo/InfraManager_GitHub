using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal abstract class DocumentCountAggregateUpdater<T> : IVisitNewEntity<DocumentReference>, IVisitDeletedEntity<DocumentReference>
        where T : IGloballyIdentifiedEntity
    {
        private readonly ObjectClass _objectClass;
        protected IFinder<T> Finder { get; }

        protected DocumentCountAggregateUpdater(ObjectClass objectClass, IFinder<T> finder)
        {
            _objectClass = objectClass;
            Finder = finder;

        }

        public void Visit(IEntityState originalState, DocumentReference entity)
        {
            UpdateDocumentCount(entity, docCount => docCount - 1);
        }

        public void Visit(DocumentReference entity)
        {
            UpdateDocumentCount(entity, docCount => docCount + 1);
        }

        private void UpdateDocumentCount(DocumentReference documentReference, Func<int, int> func)
        {
            if (documentReference.ObjectClassID == _objectClass)
            {
                var entity = Finder.Find(documentReference.ObjectID);
                UpdateDocumentCount(entity, func);
            }
        }

        protected abstract void UpdateDocumentCount(T entity, Func<int, int> func);

        public Task VisitAsync(IEntityState originalState, DocumentReference entity, CancellationToken cancellationToken)
        {
            Visit(originalState, entity);
            return Task.CompletedTask;
        }

        public Task VisitAsync(DocumentReference entity, CancellationToken cancellationToken)
        {
            Visit(entity);
            return Task.CompletedTask;
        }
    }
}
