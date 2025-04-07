using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public class ConcordanceProfile : Profile
    {
        public ConcordanceProfile()
        {
            CreateMap<Concordance, ConcordanceModel>()
                .ForMember(x => x.PriorityId, m => m.MapFrom(x => x.Priority.ID));

            CreateMap<Concordance, Concordance>();
        }
    }
}
