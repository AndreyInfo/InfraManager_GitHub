using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase
{
    internal class ArticleReferenceEventSubjectBuilder : IBuildEventSubject<KBArticleReference, KBArticleReference>
    {
        private readonly IFinder<KBArticle> _articleFinder;

        public ArticleReferenceEventSubjectBuilder(IFinder<KBArticle> articleFinder)
        {
            _articleFinder = articleFinder;
        }

        public EventSubject Build(KBArticleReference subject)
        {
            var article = _articleFinder.Find(subject.ArticleId);
            return new EventSubject(
                "Ссылка на статью КБ", 
                article.Name, 
                new InframanagerObject(subject.ObjectId, subject.ObjectClassID));
        }
    }
}
