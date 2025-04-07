using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public class NoteDetails
    {
        public Guid ID { get; init; }
        public DateTime UtcDate { get; init; }
        public Guid UserID { get; init; }
        public string Message { get; init; }
        public string UserName { get; init; }
        public bool IsRead { get; init; }
        public bool IsNote { get; init; }
        public DAL.SDNoteType Type { init { IsNote = (value == DAL.SDNoteType.Note); } get { return IsNote ? DAL.SDNoteType.Note : DAL.SDNoteType.Message; } }
    }
}
