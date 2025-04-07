using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    internal class UnreadMessageCountExpressionCreator : ISelfRegisteredService<UnreadMessageCountExpressionCreator>
    {
        private readonly DbSet<ObjectNote> _objectNotes;

        public UnreadMessageCountExpressionCreator(DbSet<ObjectNote> objectNotes)
        {
            _objectNotes = objectNotes;
        }

        public Expression<Func<T, int>> Create<T>(Guid userID) where T : IGloballyIdentifiedEntity =>
            obj => _objectNotes.Count(note => !note.Read && note.ObjectID == obj.IMObjID && note.UserID == userID);
    }
}
