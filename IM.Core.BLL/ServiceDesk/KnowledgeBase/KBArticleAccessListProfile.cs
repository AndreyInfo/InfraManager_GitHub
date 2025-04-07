using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    internal class KBArticleAccessListProfile : Profile
    {
        public KBArticleAccessListProfile()
        {
            CreateMap<KBArticleAccessListModel, KBArticleAccessList>();

            CreateMap<KBArticleAccessList, KBArticleAccessListModel>();
        }
    }
}
