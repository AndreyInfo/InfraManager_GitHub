using InfraManager.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface INotesBLL<TNote>
    {
        Task<NoteDetails[]> GetNotesAsync(Guid parentObjectID, SDNoteType? noteType,
            CancellationToken cancellationToken);

        Task<NoteDetails> UpdateNotesAsync(InframanagerObject parentObject, Guid noteId, NoteData noteData,
            CancellationToken cancellationToken);

        Task<NoteDetails> InsertAsync(InframanagerObject parentObject, NoteData noteData,
            CancellationToken cancellationToken);

        Task<NoteDetails> GetNoteAsync(Guid parentObjectID, Guid noteId, CancellationToken cancellationToken);
        
        NoteDetails[] GetNotes(Guid parentObjectID, SDNoteType? noteType);

        [Obsolete("Выпилить")]
        Task SetAllNotesReadStateAsync(Guid[] noteIDs, bool isRead, CancellationToken cancellationToken);
    }
}