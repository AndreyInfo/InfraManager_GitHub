using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Notes
{
    internal class NoteEventMessageBuilder<TNote> : IBuildEventMessage<TNote, TNote>
        where TNote : Note
    {
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;

        public NoteEventMessageBuilder(IFindEntityByGlobalIdentifier<User> userFinder)
        {
            _userFinder = userFinder;
        }

        public string Build(TNote entity, TNote subject)
        {
            var user = _userFinder.Find(entity.UserID);

            return entity.Type == SDNoteType.Message 
                ? $"Сообщение '{entity.NoteText}' от {user.FullName} добавлено."
                : $"Заметка '{entity.NoteText}' от {user.FullName} добавлена.";
        }
    }
}
