using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NoteCountExpressionCreator<T> : ISelfRegisteredService<NoteCountExpressionCreator<T>>
        where T : IGloballyIdentifiedEntity
    {
        private readonly DbSet<Note<T>> _notes;

        public NoteCountExpressionCreator(DbSet<Note<T>> notes)
        {
            _notes = notes;
        }

        public Expression<Func<T, int>> Create(SDNoteType noteType) =>
            obj => _notes.Count(note => note.Type == noteType && note.ParentObjectID == obj.IMObjID);
    }
}
