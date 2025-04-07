using AutoMapper;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentCauseProfile : Profile
    {
        public MassIncidentCauseProfile()
        {
            CreateMap<MassIncidentCause, MassIncidentCauseDetails>();
            CreateMap<LookupData, MassIncidentCause>();
        }
    }
}
