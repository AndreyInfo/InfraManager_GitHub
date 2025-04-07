using AutoMapper;
using System;

namespace InfraManager.UI.Web.AutoMapper
{
    public class RowVersionProfile : Profile
    {
        public RowVersionProfile()
        {
            CreateMap<byte[], string>().ConvertUsing(arr => arr == null ? string.Empty : Convert.ToBase64String(arr));
            CreateMap<string, byte[]>().ConvertUsing(str => string.IsNullOrWhiteSpace(str) ? null : Convert.FromBase64String(str));
        }
    }
}
