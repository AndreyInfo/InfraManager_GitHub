using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    internal interface INoteQuery<TNote>
    {
        IQueryable<NoteData> Query();
    }
}
