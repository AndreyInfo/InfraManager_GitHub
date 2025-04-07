using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.KnowledgeBase
{
    public interface IKnowledgeBaseQuery
    {
        Task<KBArticleFolderItem[]> GetFoldersAsync(Guid? parentID, bool visible, CancellationToken cancellationToken);

        /// <summary>
        /// Список папок, на которые у пользователя есть доступ
        /// </summary>
        /// <param name="userID">Идентификатор пользователя</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<KBArticleFolderItem[]> GetAccessFoldersAsync(Guid userID, CancellationToken cancellationToken);

        Task<KBArticleFolderItem[]> GetAllFoldersAsync(CancellationToken cancellationToken);

        Task<KBArticleShortItem[]> GetArticlesAsync(Guid folderID, Guid userID, CancellationToken cancellationToken);

        Task<KBArticleShortItem[]> GetArticlesAsync(Guid[] folderIDs, Guid userID, CancellationToken cancellationToken);

        Task<KBArticleItem> GetArticleAsync(Guid articleID, Guid userID, CancellationToken cancellationToken);

        Task<KBArticleTypeItem[]> GetArticleTypesAsync(CancellationToken cancellationToken);

        Task<KBArticleStatusItem[]> GetArticleStatusesAsync(CancellationToken cancellationToken);

        Task<KBArticleAccessItem[]> GetArticleAccessAsync(CancellationToken cancellationToken);

        Task<KBArticleShortItem[]> GetArticlesByIdsAsync(Guid[] articleIDs, Guid userID, CancellationToken cancellationToken);

        Task<KBArticleInfoItem[]> GetArticleInfoByIdsAsync(Guid[] articleIDs, Guid? serviceID, CancellationToken cancellationToken);

        Task<KBArticleShortItem[]> GetObjectArticlesAsync(Guid objectID, ObjectClass objectClassID, bool seeInvisible, CancellationToken cancellationToken);

        Task<KBArticleShortItem> GetArticleByNumberAsync(int number, CancellationToken cancellationToken);

        Task<Guid[]> CheckArticlesAccessAsync(Guid[] articleIDs, Guid userID, CancellationToken cancellationToken);
    }
}
