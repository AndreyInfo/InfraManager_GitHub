using AutoMapper;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.BLL.ServiceDesk.Calls;

public class CallSummaryMap : Profile
{
    public CallSummaryMap()
    {
        CreateMap<CallSummary, CallSummaryDetails>()
             .ForMember(dst => dst.ServiceName, m => m.MapFrom(scr => GetServiceName(scr)))
             .ForMember(dst => dst.ServiceCategoryName, m => m.MapFrom(scr => GetServiceCategoryName(scr)))
             .ForMember(dst => dst.ServiceItemName, m=> m.MapFrom(scr => scr.ServiceItemID.HasValue 
                                                                            ? scr.ServiceItem.Name 
                                                                            : string.Empty))
             .ForMember(dst => dst.ServiceAttendanceName, m=> m.MapFrom(scr => scr.ServiceAttendanceID.HasValue 
                                                                                 ? scr.ServiceAttendance.Name 
                                                                                 : string.Empty))
            .ReverseMap();


        CreateMap<CallSummaryModelItem, CallSummaryDetails>();
    }

    private static string GetServiceName(CallSummary model)
    {
        if (model.ServiceItem?.Service is not null)
            return model.ServiceItem?.Service.Name;

        if (model.ServiceAttendance?.Service is not null)
            return model.ServiceAttendance?.Service.Name;

        return string.Empty;
    }

    private static string GetServiceCategoryName(CallSummary model)
    {
        if (model.ServiceItem?.Service?.Category is not null)
            return model.ServiceItem.Service.Category.Name;

        if (model.ServiceAttendance?.Service?.Category is not null)
            return model.ServiceAttendance.Service.Category.Name;

        return string.Empty;
    }
}
