using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceAttendances;

internal sealed class ServiceAttendanceProfile : Profile
{
    public ServiceAttendanceProfile()
    {
        CreateMap<ServiceAttendance, ServiceAttendanceDetails>()
            .ForMember(
                c => c.ClassID,
                m => m.MapFrom(scr => ObjectClass.ServiceAttendance))
            .ForMember(
                c => c.StateName,
                m => m
                    .MapFrom<LocalizedEnumResolver<ServiceAttendance, ServiceAttendanceDetails, CatalogItemState>,
                        CatalogItemState>(
                        entity => entity.State ?? entity.Service.State))
            .ForMember(
                dst => dst.Summary,
                m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Summary) ? "" : scr.Summary))
            .ForMember(
                dst => dst.CategoryID,
                m => m.MapFrom(x => x.Service.Category.ID))
            .ForMember(
                dst => dst.State,
                m => m.MapFrom(x => x.State ?? x.Service.State))
            .ForMember(dst => dst.WorkflowSchemeIdentifier,
                m => m.MapFrom(x => x.WorkflowSchemeIdentifier));


        CreateMap<ServiceAttendanceData, ServiceAttendance>()
           .ConstructUsing(c => new ServiceAttendance(c.Name));

        CreateMap<ServiceAttendance, ServiceItemDetailsModel>()
            .ForMember(
                model => model.ClassID,
                mapper => mapper.MapFrom(entity => ObjectClass.ServiceAttendance))
            .ForMember(
                model => model.ServiceCategoryID,
                mapper => mapper.MapFrom(entity => entity.Service.Category.ID))
            .ForMember(
                model => model.ServiceCategoryName,
                mapper => mapper.MapFrom(entity => entity.Service.Category.Name))
            .ForMember(
                model => model.ServiceID,
                mapper => mapper.MapFrom(entity => entity.Service.ID))
            .ForMember(
                model => model.ServiceName,
                mapper => mapper.MapFrom(entity => entity.Service.Name))
            .ForMember(
                model => model.FullName,
                mapper => mapper.MapFrom(entity =>
                    $"{entity.Service.Category.Name} \\ {entity.Service.Name} \\ {entity.Name}"))
            .ForMember(
                model => model.Summary,
                mapper => mapper.MapFrom(entity => entity.Summary));
    }
}