using Inframanager;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class AllMassIncidentsReportUserSpecificationBuilder : ReportUserSpecificationBuilder<MassIncident>,
        ISelfRegisteredService<AllMassIncidentsReportUserSpecificationBuilder>
    {
        public AllMassIncidentsReportUserSpecificationBuilder(
            IFindEntityByGlobalIdentifier<User> userFinder, 
            ServiceDeskObjectAccessIsNotRestricted accessNotRestricted,
            UserIsSupervisor<MassIncident> userIsSupervisor,
            MassIncidentIsAvailableViaToz availableViaToz) 
            : base(SpecificationBuilder<MassIncident, User>.Any(
                MassIncident.UserIsCreator,
                MassIncident.UserIsExecutor,
                MassIncident.UserIsOwner,
                MassIncident.UserIsInGroup,
                userIsSupervisor,
                availableViaToz), userFinder, accessNotRestricted)
        {
        }
    }
}
