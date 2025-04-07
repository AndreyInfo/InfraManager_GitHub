using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL.KnowledgeBase;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    public class KBArticleShortProfile : Profile
    {
        public KBArticleShortProfile()
        {
            CreateMap<KBArticleShortItem, KBArticleShortDetails>();

            CreateMap<KBArticleShortItem, KBArticleDependency>()
                .ForMember(dst => dst.KBArticleDependencyID, m => m.MapFrom(src => src.ID))
                .ForMember(dst => dst.KBArticleDependencyName, m => m.MapFrom(src => src.Name));
        }
    }
}
