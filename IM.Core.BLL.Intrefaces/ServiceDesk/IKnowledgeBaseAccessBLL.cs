using InfraManager.BLL.KnowledgeBase;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IKnowledgeBaseAccessBLL
    {
        /// <summary>
        /// Добавить новую сущность KBArticleAccessList в БД
        /// </summary>
        Task AddAsync(KBArticleAccessListModel accessListModel, CancellationToken cancellationToken);

        /// <summary>
        /// Удалить сущность KBArticleAccessList из БД
        /// </summary>
        Task DeleteAsync(KBArticleAccessListModel accessListModel, CancellationToken cancellationToken);

        /// <summary>
        /// Получить список связанных сущностей KBArticleAccessList для статьи БЗ с пагинацией
        /// </summary>
        Task<KBArticleAccessListModel[]> GetListAsync(KBArticleAccessListFilter filter, CancellationToken cancellationToken);
    }
}
