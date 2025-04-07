using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    [Obsolete("В этом интерфейсе больше нет смысла")]
    public interface IObjectNoteQuery<TNote> 
    {
        Task<NoteQueryResultItem[]> ExecuteAsync(ObjectNoteQueryCriteria objectNoteQueryCriteria, CancellationToken cancellationToken = default);
        NoteQueryResultItem[] Execute(ObjectNoteQueryCriteria objectNoteQueryCriteria);
    }
}
