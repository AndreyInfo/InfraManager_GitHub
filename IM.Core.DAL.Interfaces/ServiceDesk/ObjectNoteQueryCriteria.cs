using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    public class ObjectNoteQueryCriteria
    {
        public Guid? CurrentUserID { get;init;}
        public Guid ObjectID { get;init;}
        public Guid? NoteID { get;init;}
        public SDNoteType? NoteType { get;init;}
    }
}
