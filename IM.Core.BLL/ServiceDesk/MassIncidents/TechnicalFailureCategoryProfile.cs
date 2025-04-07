using AutoMapper;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;

namespace InfraManager.BLL.ServiceDesk.TechnicalFailuresCategories;

public class TechnicalFailureCategoryProfile : Profile
{
    public TechnicalFailureCategoryProfile()
    {
        CreateMap<TechnicalFailureCategoryData, TechnicalFailureCategory>();
        CreateMap<ServiceReferenceUpdatableData, ServiceTechnicalFailureCategory>();

        CreateMap<ServiceTechnicalFailureCategory, ServiceReferenceDetails>()
            .ForMember(reference => reference.ServiceID, mapper => mapper.MapFrom(entity => entity.Reference.ID));
        CreateMap<TechnicalFailureCategory, TechnicalFailureCategoryDetails>()
            .ForMember(details => details.ServiceReferences, mapper => mapper.MapFrom(entity => entity.Services));
    }
}