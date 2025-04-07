using AutoMapper;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Mapping
{
    public class PriorityMatrixCellProfile : Profile
    {
        public PriorityMatrixCellProfile()
        {
            CreateMap<Concordance, ConcordanceDetails>()
                .ForMember(dst => dst.InfluenceId, m => m.MapFrom(scr => scr.InfluenceId))
                .ForMember(dst => dst.UrgencyId, m => m.MapFrom(scr => scr.UrgencyId))
                .ForMember(dst => dst.PriorityId, m => m.MapFrom(scr => scr.PriorityID))
                .ForMember(dst => dst.Priority, m => m.MapFrom(scr => scr.Priority))
                ;

            CreateMap<ConcordanceDetails, Concordance>()
                .ForMember(dst => dst.InfluenceId, m => m.MapFrom(scr => scr.InfluenceId))
                .ForMember(dst => dst.UrgencyId, m => m.MapFrom(scr => scr.UrgencyId))
                .ForMember(dst => dst.PriorityID, m => m.MapFrom(scr => scr.PriorityId))
                ;
        }
    }
}
