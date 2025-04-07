using System;

namespace InfraManager.DAL.ServiceDesk
{
    //TODO: Переписать этот класс (пофиксить инкапсуляцию: Не должно быть возможности в коллекцию KbArticle добавить референс с ArticleId неравным Id статьи)
    public class KBArticleReference
    {
        public Guid ObjectId { get; init; }

        public ObjectClass ObjectClassID { get; init; }

        public Guid ArticleId { get; init; }
    }
}
