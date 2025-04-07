using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentReferenceProfile : Profile
    {
        public MassIncidentReferenceProfile()
        {
            CreateManyToManyMap<Call>();
            CreateManyToManyMap<Problem>();
            CreateManyToManyMap<WorkOrder>();
            CreateManyToManyMap<ChangeRequest>();
            CreateManyToManyMap<User>();
            CreateMap<ManyToMany<MassIncident, Service>, MassIncidentReferenceDetails>()
                .ForMember(details => details.ReferenceID, mapper => mapper.MapFrom(entity => entity.Reference.ID));
            this.CreateCallReferenceMap<MassIncidentReferencedCallListItem>();
            this.CreateChangeRequestReferenceMap<MassIncidentReferencedChangeRequestListItem>();
            this.CreateProblemReferenceMap<MassIncidentReferencedProblemListItem>();
            this.CreateWorkOrderReferenceMap<MassIncidentReferencedWorkOrderListItem>();
        }

        private void CreateManyToManyMap<T>() where T : class, IGloballyIdentifiedEntity =>
            CreateMap<ManyToMany<MassIncident, T>, MassIncidentReferenceDetails>()
                .ForMember(details => details.ReferenceID, mapper => mapper.MapFrom(entity => entity.Reference.IMObjID));
    }
}
