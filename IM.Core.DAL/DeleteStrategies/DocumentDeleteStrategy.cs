using InfraManager.DAL.Documents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.DeleteStrategies
{
    internal class DocumentDeleteStrategy : IDeleteStrategy<Document>
    , ISelfRegisteredService<IDeleteStrategy<Document>>
    {
        private readonly IRepository<DocumentReference> _documentReferences;
        private readonly DbSet<Document> _documents;

        public DocumentDeleteStrategy(
            IRepository<DocumentReference> documentReferences,
            DbSet<Document> documents)
        {
            _documentReferences = documentReferences;
            _documents = documents;
        }

        public void Delete(Document entity)
        {
            _documentReferences.Where(x => x.DocumentID == entity.ID).ForEach(x => _documentReferences.Delete(x));
            _documents.Remove(entity);
        }
    }
}
