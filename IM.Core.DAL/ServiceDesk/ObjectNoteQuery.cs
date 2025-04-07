using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    internal class ObjectNoteQuery<TNote> : IObjectNoteQuery<TNote>
    {
        private readonly DbSet<ObjectNote> _objectNotes;
        private readonly DbSet<Note<TNote>> _notes;

        public ObjectNoteQuery(DbSet<Note<TNote>> notes, DbSet<ObjectNote> objectNotes)
        {
            _objectNotes = objectNotes;
            _notes = notes;
        }

        public Task<NoteQueryResultItem[]> ExecuteAsync(ObjectNoteQueryCriteria objectNoteQueryCriteria,
            CancellationToken cancellationToken = default)
        {
            return CreateQueryable(objectNoteQueryCriteria).ToArrayAsync(cancellationToken);
        }

        public NoteQueryResultItem[] Execute(ObjectNoteQueryCriteria objectNoteQueryCriteria)
        {
            return CreateQueryable(objectNoteQueryCriteria).ToArray();
        }

        private IQueryable<NoteQueryResultItem> CreateQueryable(ObjectNoteQueryCriteria objectNoteQueryCriteria)
        {
            var noteInfo = _objectNotes.AsQueryable();
            if (objectNoteQueryCriteria.CurrentUserID != null)
                noteInfo = noteInfo.Where(x => x.UserID == objectNoteQueryCriteria.CurrentUserID.Value);

            var query = from note in _notes
                join ni in noteInfo
                    on note.ID equals ni.ID into grouping
                from r in grouping.DefaultIfEmpty()
                where note.ParentObjectID == objectNoteQueryCriteria.ObjectID
                select new NoteQueryResultItem
                {
                    NoteEntity = note,
                    UserName = User.GetFullName(note.UserID),
                    IsRead = r == null ? false : r.Read,
                };
            if (objectNoteQueryCriteria.NoteID != null)
                query = query.Where(x => x.NoteEntity.ID == objectNoteQueryCriteria.NoteID.Value);
            if (objectNoteQueryCriteria.NoteType != null)
                query = query.Where(x => x.NoteEntity.Type == objectNoteQueryCriteria.NoteType.Value);

            return query;
        }
    }
}