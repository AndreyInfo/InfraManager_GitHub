using AutoMapper;
using System;

namespace InfraManager.BLL.AutoMapper
{
    public class DateTimeProfile : Profile
    {
        public DateTimeProfile()
        {
            CreateMap<DateTime, string>()
                .ConstructUsing(dt => dt.ConvertToMillisecondsAfterMinimumDate());
            CreateMap<DateTime?, string>()
                .ConstructUsing(
                    dt => dt.HasValue 
                        ?  dt.Value.ConvertToMillisecondsAfterMinimumDate() 
                        : string.Empty);
            CreateMap<string, DateTime>()
                .ConvertUsing(
                    (src, dest) => string.IsNullOrWhiteSpace(src) ? dest : DateTimeExtensions.ConvertFromMillisecondsAfterMinimumDate(src));
            CreateMap<string, DateTime?>()
                .ConstructUsing(
                    s => DateTimeExtensions.NullableConvertFromMillisecondsAfterMinimumDate(s));
        }
    }
}
