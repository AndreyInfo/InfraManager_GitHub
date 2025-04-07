using AutoMapper;
using Inframanager.BLL;
using System;

namespace InfraManager.BLL.AutoMapper
{
    public class NullablePropertyProfile : Profile
    {
        public NullablePropertyProfile()
        {
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);           
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);            
            CreateMap<long?, long>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<short?, short>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<NullablePropertyWrapper<bool>, bool?>().ConvertUsing((src, dest) => src.Ignore ? dest : src.Value);
            CreateMap<NullablePropertyWrapper<Guid>, Guid?>().ConvertUsing((src, dest) => src.Ignore ? dest : src.Value);
            CreateMap<NullablePropertyWrapper<Guid>, Guid>().ConvertUsing((src, dest) => src.Ignore ? dest : (src.Value ?? Guid.Empty));
            CreateMap<NullablePropertyWrapper<int>, int?>().ConvertUsing((src, dest) => src.Ignore ? dest : src.Value);
            CreateMap<NullablePropertyWrapper<long>, long?>().ConvertUsing((src, dest) => src.Ignore ? dest : src.Value);
            CreateMap<NullableString, string>()                
                .ConvertUsing((src, dest) => (src == null || src.Ignore) ? dest : src.Value);
        }
    }
}
