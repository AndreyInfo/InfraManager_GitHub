using AutoMapper;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentTypeProfile : Profile
    {
        public MassIncidentTypeProfile()
        {
            CreateMap<MassIncidentType, MassIncidentTypeDetails>();
            CreateMap<MassIncidentTypeData, MassIncidentType>()
                .ConstructUsing(data => new MassIncidentType(data.Name));
        }
    }
}
