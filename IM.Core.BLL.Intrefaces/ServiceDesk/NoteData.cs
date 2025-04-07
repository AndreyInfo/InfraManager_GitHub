using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public class NoteData
    {
        public Guid? UserID { get; init; }
        public string Message { get; init; }
        public bool? IsRead { get; init; }
        public bool? IsNote { get; init; }
        public DateTime? Date { get; init; }
        public SDNoteType? Type { get => IsNote == null ? null : (IsNote.Value ? SDNoteType.Note : SDNoteType.Message); }
    }
}
