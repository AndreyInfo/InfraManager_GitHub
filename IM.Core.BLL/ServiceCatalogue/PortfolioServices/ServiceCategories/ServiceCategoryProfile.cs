using AutoMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;

internal sealed class ServiceCategoryProfile : Profile
{
    public ServiceCategoryProfile()
    {
        CreateMap<ServiceCategory, ServiceCategoryDetailsModel>()
            .ForMember(
                model => model.HasImage,
                mapper => mapper.MapFrom(entity => entity.Icon != null));

        CreateMap<ServiceCategory, ServiceCategoryItem>()
            .ReverseMap()
            .ForMember(dst => dst.ExternalId, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.ExternalID) ? string.Empty : scr.ExternalID))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? string.Empty : scr.Note));

        CreateMap<ServiceCategory, ServiceCategoryDetails>()
            .ReverseMap()
            .ForMember(dst => dst.ExternalId, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.ExternalID) ? string.Empty : scr.ExternalID))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? string.Empty : scr.Note));

        CreateMap<ServiceCategory, ServiceCategoryData>()
            .ReverseMap()
            .ForMember(dst => dst.ExternalId, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.ExternalID) ? string.Empty : scr.ExternalID))
            .ForMember(dst => dst.Note, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Note) ? string.Empty : scr.Note));
    }
}
