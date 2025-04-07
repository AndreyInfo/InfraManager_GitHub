using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal class DocumentReferenceBLL : IDocumentReferenceBLL, ISelfRegisteredService<IDocumentReferenceBLL>
    {
        private readonly IUnitOfWork _saveChanges;
        private readonly IRepository<DocumentReference> _documentReferenceRepository;
        private readonly IMapper _mapper;

        public DocumentReferenceBLL(
            IRepository<DocumentReference> documentReferenceRepository,
            IUnitOfWork saveChanges,            
            IMapper mapper)
        {
            _saveChanges = saveChanges;
            _documentReferenceRepository = documentReferenceRepository;
            _mapper = mapper;
        }

        public async Task<DocumentReferenceDetails[]> AddReferencesAsync(ObjectClass classID, Guid entityID, Guid[] docIDs, CancellationToken cancellationToken)
        {
            if (!docIDs.Any())
                return Array.Empty<DocumentReferenceDetails>();

            var docReferences = docIDs.Select(id => new DocumentReference(id, entityID, classID));
            foreach (var doc in docReferences)
                _documentReferenceRepository.Insert(doc);

            await _saveChanges.SaveAsync(cancellationToken);
            
            return _mapper.Map<DocumentReferenceDetails[]>(docReferences);
        }

        public async Task DeleteReferenceAsync(ObjectClass classID, Guid entityID, Guid docID, CancellationToken cancellationToken)
        {
            var refs = _documentReferenceRepository
                .Where(x => x.DocumentID == docID && x.ObjectID == entityID)
                .ToArray();
            
            if (!refs.Any()) 
                throw new ObjectNotFoundException($"Not found {nameof(DocumentReference)}: DocumentId: {docID}, ObjectId: {entityID}");

            refs.ForEach(x =>
            {
                _documentReferenceRepository.Delete(x);
            });

            await _saveChanges.SaveAsync(cancellationToken);
        }
    }
}
