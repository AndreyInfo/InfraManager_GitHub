using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL.KnowledgeBase;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    public class KBArticleInfoProfile : Profile
    {
        public KBArticleInfoProfile()
        {
            CreateMap<KBArticleInfoItem, KBArticleInfoDetails>()
                .ForMember(dst => dst.TagString, m => m.MapFrom(src => src.Tags));
        }
    }
}
