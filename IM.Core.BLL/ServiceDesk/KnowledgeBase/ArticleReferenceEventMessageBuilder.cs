using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    internal class ArticleReferenceEventMessageBuilder : IBuildEventMessage<KBArticleReference, KBArticleReference>
    {
        private readonly string _action;
        private readonly IFinder<KBArticle> _articleFinder;

        public ArticleReferenceEventMessageBuilder(string action, IFinder<KBArticle> articleFinder)
        {
            _action = action;
            _articleFinder = articleFinder;
        }

        public string Build(KBArticleReference entity, KBArticleReference subject)
        {
            var article = _articleFinder.Find(entity.ArticleId);

            return $"Связь со статьей БЗ '{article.Name}' {_action}.";
        }
    }
}
