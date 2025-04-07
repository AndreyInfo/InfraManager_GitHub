using AutoMapper;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;

namespace InfraManager.BLL;

internal class FilterProfile : Profile
{
    public FilterProfile()
    {
        CreateMap<BaseFilter, PaggingFilter>()
            .ForMember(dst => dst.Skip, m => m.MapFrom(scr => scr.StartRecordIndex))
            .ForMember(dst => dst.Take, m => m.MapFrom(scr => scr.CountRecords));
    }
}
