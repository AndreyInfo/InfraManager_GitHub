using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL.KnowledgeBase;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    public class KBArticleProfile : Profile
    {
        public KBArticleProfile()
        {
            CreateMap<KBArticle, KBArticleDetails>()
                .ForMember(dst => dst.TypeID, m => m.MapFrom(src => src.ArticleTypeID))
                .ForMember(dst => dst.StatusID, m => m.MapFrom(src => src.ArticleStatusID))
                .ForMember(dst => dst.AccessID, m => m.MapFrom(src => src.ArticleAccessID));

            CreateMap<KBArticleItem, KBArticleDetails>();

            CreateMap<KBArticleEditData, KBArticle>()
                .ForMember(dst => dst.ArticleTypeID, m => m.MapFrom(src => src.TypeID))
                .ForMember(dst => dst.ArticleStatusID, m => m.MapFrom(src => src.StatusID))
                .ForMember(dst => dst.ArticleAccessID, m => m.MapFrom(src => src.AccessID));
        }
    }
}
