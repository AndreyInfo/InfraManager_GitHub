using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.Documents
{
    internal class DocumentCountExpressionCreator : ISelfRegisteredService<DocumentCountExpressionCreator>
    {
        private readonly DbSet<DocumentReference> _docReferences;

        public DocumentCountExpressionCreator(DbSet<DocumentReference> docReferences)
        {
            _docReferences = docReferences;
        }

        public Expression<Func<T, int>> Create<T>() where T : IGloballyIdentifiedEntity =>
            obj => _docReferences.Count(docRef => docRef.ObjectID == obj.IMObjID);
    }
}
