using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchServiceNoteUpdater<T> :
        IVisitNewEntity<Note<T>>,
        IVisitModifiedEntity<Note<T>>,
        IVisitDeletedEntity<Note<T>>
        where T : class, IGloballyIdentifiedEntity
    {
        private readonly IReadonlyRepository<T> _repository;
        private readonly INotesBLL<T> _notes;
        private readonly ISearchService _searchService;

        public SearchServiceNoteUpdater(
            IReadonlyRepository<T> repository, 
            INotesBLL<T> notes,
            ISearchService searchService)
        {
            _repository = repository;
            _notes = notes;
            _searchService = searchService;
        }

        void IVisitModifiedEntity<Note<T>>.Visit(IEntityState originalState, Note<T> currentState)
        {
            Update(currentState.ParentObjectID);
        }

        void IVisitNewEntity<Note<T>>.Visit(Note<T> entity)
        {
            Update(entity.ParentObjectID);
        }

        void IVisitDeletedEntity<Note<T>>.Visit(IEntityState originalState, Note<T> entity)
        {
            Update(entity.ParentObjectID);
        }

        Task IVisitModifiedEntity<Note<T>>.VisitAsync(IEntityState originalState, Note<T> currentState,
            CancellationToken cancellationToken)
        {
            return UpdateAsync(currentState.ParentObjectID, cancellationToken);
        }

        Task IVisitNewEntity<Note<T>>.VisitAsync(Note<T> entity, CancellationToken cancellationToken)
        {
            return UpdateAsync(entity.ParentObjectID, cancellationToken);
        }

        Task IVisitDeletedEntity<Note<T>>.VisitAsync(IEntityState originalState, Note<T> entity, CancellationToken cancellationToken)
        {
            return UpdateAsync(entity.ParentObjectID, cancellationToken);
        }

        private async Task UpdateAsync(Guid objectId, CancellationToken cancellationToken)
        {
            var notes = await _notes.GetNotesAsync(objectId, null, cancellationToken);
            var entity = await _repository.FirstOrDefaultAsync(e => e.IMObjID == objectId, cancellationToken);
            if (entity == null) return;
            _searchService.Update(entity, notes);
        }

        private void Update(Guid objectId)
        {
            var notes = _notes.GetNotes(objectId, null);
            var entity = _repository.FirstOrDefault(e => e.IMObjID == objectId);
            if (entity == null) return;
            _searchService.Update(entity, notes);
        }
    }
}