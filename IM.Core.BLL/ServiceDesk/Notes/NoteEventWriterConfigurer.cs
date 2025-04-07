using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Notes
{
    internal class NoteEventWriterConfigurer<TNote> : IConfigureEventWriter<TNote, TNote>
        where TNote : Note
    {
        private readonly ObjectClass _objectClassID;

        public NoteEventWriterConfigurer(ObjectClass objectClassID)
        {
            _objectClassID = objectClassID;
        }

        public void Configure(IEventWriter<TNote, TNote> writer)
        {
            writer.HasParentObject(note => new InframanagerObject(note.ParentObjectID, _objectClassID));
        }
    }
}
