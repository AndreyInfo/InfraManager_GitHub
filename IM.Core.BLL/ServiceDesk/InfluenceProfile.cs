using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public class InfluenceResultProfile : Profile
    {
        public InfluenceResultProfile()
        {
            CreateMap<Influence, InfluenceListItemModel>();
            CreateMap<InfluenceModel, Influence>()
                .ConstructUsing(x => new Influence(x.Name, x.Sequence));
            CreateMap<Influence, InfluenceDetailsModel>();
        }
    }
}
