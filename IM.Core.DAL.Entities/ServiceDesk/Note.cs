using System;

namespace InfraManager.DAL.ServiceDesk
{
    public abstract class Note
    {
        protected Note() { }
        protected Note(Guid parentObjectID, Guid userID, DateTime? date)
        {
            ParentObjectID = parentObjectID;
            ID = Guid.NewGuid();
            UtcDate = date ?? DateTime.UtcNow;
            UserID = userID;
        }
        public Guid ParentObjectID { get; init; }
        public Guid UserID { get; init; }
        public DateTime UtcDate { get; init; }
        public string NoteText { get; set; }
        public string HTMLNote { get; set; }
        public SDNoteType Type { get; set; }
        public Guid ID { get; init; }

        public ObjectNote CreateObjectNote(Guid userId, bool read)
        {
            return new ObjectNote(ID, ParentObjectID, userId, read);
        }
    }

    public class Note<T> : Note
    {
        protected Note() : base()
        {
        }
        public Note(Guid parentObjectID, Guid userID, DateTime? date) : base(parentObjectID, userID, date)
        {
        }
    }
}
