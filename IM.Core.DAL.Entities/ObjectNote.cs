using System;

namespace InfraManager.DAL
{
    public class ObjectNote
    {
        protected ObjectNote() { }
        internal ObjectNote(Guid id, Guid objectID, Guid userID, bool read = false)
        {
            ID = id;
            ObjectID = objectID;
            UserID = userID;
            Read = read;
        }
        #region Properties

        public Guid ID { get; init; }
        public Guid ObjectID { get; init; }
        public Guid UserID { get; init; }
        public bool Read { get; set; }

        #endregion

        #region Db Functions

        public static int GetUnreadObjectNoteCount(Guid id, Guid userId) => throw new NotSupportedException();
        public static int GetObjectNoteCount(ObjectClass objectClass, Guid objectId, int type) => throw new NotSupportedException();

        #endregion
    }
}
