using AutoMapper;
using InfraManager.BLL.Asset.dto;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Mapping
{
    internal class InfrastructureSegmentProfile : Profile
    {
        public InfrastructureSegmentProfile()
        {
            CreateMap<InfrastructureSegment, InfrastructureSegmentDTO>()
                .ReverseMap();
        }
    }
}
