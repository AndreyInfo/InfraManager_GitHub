using Inframanager.BLL.ListView;
using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentReferencesListViewFilter : ListViewPageFilter
    {
        public Guid[] IDList { get; init; }
    }
}
