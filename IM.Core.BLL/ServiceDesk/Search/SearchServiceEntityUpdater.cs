using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchServiceEntityUpdater<T, TNote> : SearchServiceEntityUpdater<T>,
        IVisitModifiedEntity<T>
        where T : class, IGloballyIdentifiedEntity
        where TNote : Note<T>
    {
        private readonly INotesBLL<T> _notes;
        private readonly ISearchService _searchService;

        public SearchServiceEntityUpdater(INotesBLL<T> notes, ISearchService searchService) : base (searchService)
        {
            _notes = notes;
            _searchService = searchService;
        }

        void IVisitModifiedEntity<T>.Visit(IEntityState originalState, T currentState)
        {
            var notes = _notes.GetNotes(currentState.IMObjID, null);
            if (currentState is IMarkableForDelete deletedEntity && deletedEntity.Removed)
            {
                _searchService.Delete(currentState.IMObjID);
            }
            else
            {
                _searchService.Update(currentState, notes);
            }
        }

        async Task IVisitModifiedEntity<T>.VisitAsync(IEntityState originalState, T currentState, CancellationToken cancellationToken)
        {
            var notes = await _notes.GetNotesAsync(currentState.IMObjID, null, cancellationToken);
            if (currentState is IMarkableForDelete deletedEntity && deletedEntity.Removed)
            {
                _searchService.Delete(currentState.IMObjID);
            }
            else
            {
                _searchService.Update(currentState, notes);
            }
        }
    }

    public class SearchServiceEntityUpdater<T> :
        IVisitNewEntity<T>,
        IVisitModifiedEntity<T>,
        IVisitDeletedEntity<T>
        where T : class, IGloballyIdentifiedEntity
    {
        private readonly ISearchService _searchService;

        public SearchServiceEntityUpdater(ISearchService searchService)
        {
            _searchService = searchService;
        }

        void IVisitModifiedEntity<T>.Visit(IEntityState originalState, T currentState)
        {
            if (currentState is IMarkableForDelete deletedEntity && deletedEntity.Removed)
            {
                _searchService.Delete(currentState.IMObjID);
            }
            else
            {
                _searchService.Update(currentState);
            }
        }

        async Task IVisitModifiedEntity<T>.VisitAsync(IEntityState originalState, T currentState, CancellationToken cancellationToken)
        {
            if (currentState is IMarkableForDelete deletedEntity && deletedEntity.Removed)
            {
                _searchService.Delete(currentState.IMObjID);
            }
            else
            {
                _searchService.Update(currentState);
            }
        }

        void IVisitNewEntity<T>.Visit(T entity)
        {
            _searchService.Insert(entity);
        }

        void IVisitDeletedEntity<T>.Visit(IEntityState originalState, T entity)
        {
            _searchService.Delete(entity.IMObjID);
        }

        Task IVisitNewEntity<T>.VisitAsync(T entity, CancellationToken cancellationToken)
        {
            _searchService.Insert(entity);
            return Task.CompletedTask;
        }

        Task IVisitDeletedEntity<T>.VisitAsync(IEntityState originalState, T entity, CancellationToken cancellationToken)
        {
            _searchService.Delete(entity.IMObjID);
            return Task.CompletedTask;
        }
    }
}