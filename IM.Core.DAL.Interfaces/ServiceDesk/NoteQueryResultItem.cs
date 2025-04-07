using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    public class NoteQueryResultItem
    {
        public Note NoteEntity { get; init; }
        public string UserName { get; init; }
        public bool IsRead { get; init; }
    }
}
