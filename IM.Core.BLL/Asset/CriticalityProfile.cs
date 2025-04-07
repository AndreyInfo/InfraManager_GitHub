using AutoMapper;
using InfraManager.DAL.Asset;
using System;

namespace InfraManager.BLL.Asset
{
    public class CriticalityProfile : Profile
    {
        public CriticalityProfile()
        {
            CreateMap<LookupData, Criticality>()
                .ConstructUsing(data => new Criticality(data.Name));
            CreateMap<Criticality, LookupDetails<Guid>>();
        }
    }
}
