using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{

    /// <summary>
    /// CRUD операции по присоединяемым документам
    /// </summary>
    public interface IDocumentReferenceBLL
    {
        public Task<DocumentReferenceDetails[]> AddReferencesAsync(ObjectClass classID, Guid entityID, Guid[] docIDs, CancellationToken cancellationToken);
        public Task DeleteReferenceAsync(ObjectClass classID, Guid entityID, Guid docID, CancellationToken cancellationToken);
    }
}
