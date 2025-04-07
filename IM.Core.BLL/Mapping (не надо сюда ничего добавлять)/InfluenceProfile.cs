using AutoMapper;
using InfraManager.BLL.Asset.dto;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Mapping
{
    public class InfluenceProfile : Profile
    {
        public InfluenceProfile()
        {
            CreateMap<InfluenceDetails, Influence>()
                .ForMember(dst => dst.ID, m=> m.MapFrom(scr=> scr.ID))
                .ForMember(dst => dst.Name, m=> m.MapFrom(scr=> scr.Name))
                .ForMember(dst => dst.Sequence, m=> m.MapFrom(scr=> scr.Sequence))
                .ForMember(dst => dst.RowVersion, m=> m.MapFrom(scr=> scr.RowVersion));

            CreateMap<Influence, InfluenceDetails>()
                .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.ID))
                .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.Name))
                .ForMember(dst => dst.Sequence, m => m.MapFrom(scr => scr.Sequence))
                .ForMember(dst => dst.RowVersion, m => m.MapFrom(scr => scr.RowVersion));

            CreateMap<Influence, Influence>();
        }
    }
}
