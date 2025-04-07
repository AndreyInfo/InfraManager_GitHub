using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    internal class MassIncidentsNoteQuery : INoteQuery<MassIncident>, ISelfRegisteredService<INoteQuery<MassIncident>>
    {
        public IQueryable<NoteData> Query()
        {
            return Array.Empty<NoteData>().AsQueryable();
        }
    }
}
