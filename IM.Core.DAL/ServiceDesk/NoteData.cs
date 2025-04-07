using System;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NoteData
    {
        public Guid ParentObjectID { get; init; }
        public Guid UserID { get; init; }
        public SDNoteType Type { get; init; }
    }
}
