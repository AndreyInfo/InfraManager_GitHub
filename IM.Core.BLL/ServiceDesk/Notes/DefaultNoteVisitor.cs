using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Notes
{
    internal class DefaultNoteVisitor<TObject> : IVisitNewEntity<Note<TObject>>
        where TObject : class, IHaveUtcModifiedDate, IGloballyIdentifiedEntity
    {
        private readonly IFindEntityByGlobalIdentifier<TObject> _finder;
        private readonly ILogger _logger;

        public DefaultNoteVisitor(IFindEntityByGlobalIdentifier<TObject> finder, ILogger<DefaultNoteVisitor<TObject>> logger)
        {
            _finder = finder;
            _logger = logger;
        }

        public void Visit(Note<TObject> entity)
        {
            var noteObject = _finder.Find(entity.ParentObjectID);
            SetModified(noteObject);
        }

        public async Task VisitAsync(Note<TObject> entity, CancellationToken cancellationToken)
        {
            var noteObject = await _finder.FindAsync(entity.ParentObjectID, cancellationToken);
            SetModified(noteObject);
        }

        private void SetModified(TObject noteObject)
        {
            if (noteObject != null)
            {
                noteObject.UtcDateModified = DateTime.UtcNow;
            }
            else
            {
                _logger.LogWarning($"Note added / deleted for not existing or deleted object (ID = {noteObject.IMObjID})");
            }
        }
    }
}
