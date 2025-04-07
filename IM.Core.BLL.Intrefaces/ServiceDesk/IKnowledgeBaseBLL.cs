using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.KnowledgeBase;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IKnowledgeBaseBLL
    {
        Task<KBArticleFolderDetails[]> GetAllFoldersAsync(Guid? parentId, bool visible, CancellationToken cancellationToken);

        Task<KBArticleFolderDetails[]> GetFolderHierarchyAsync(Guid? parentId, bool visible, CancellationToken cancellationToken);

        /// <summary>
        /// Получаем статьи входящие в каталог
        /// </summary>
        /// <param name="folderId">Идентификатор каталога</param>
        /// <param name="seeInvisible">Фильтр видимости</param>
        /// <param name="cancellationToken">Сигнализатор отмены операции</param>
        /// <returns></returns>
        Task<KBArticleShortDetails[]> GetArticlesAsync(Guid folderId, bool seeInvisible, CancellationToken cancellationToken);

        /// <summary>
        /// Получаем статью по идентификатору для просмотра
        /// </summary>
        /// <param name="articleId">Идентификатор статьи</param>
        /// <param name="cancellationToken">Сигнализатор отмены операции</param>
        /// <returns></returns>
        Task<KBArticleDetails> GetArticleAsync(Guid articleId, CancellationToken cancellationToken);

        /// <summary>
        /// Получаем ID статьи по его номеру
        /// </summary>
        /// <param name="number">Номер статьи</param>
        /// <returns>ID статьи</returns>
        Task<Guid> GetArticleIDByNumberAsync(int number, CancellationToken cancellationToken);

        Task<KBArticleTypeDetails[]> GetArticleTypesAsync(CancellationToken cancellationToken);

        Task<KBArticleStatusDetails[]> GetArticleStatusesAsync(CancellationToken cancellationToken);

        Task<KBArticleAccessModel[]> GetArticleAccessAsync(CancellationToken cancellationToken);

        Task<KBArticleDetails> AddArticleAsync(KBArticleEditData article, Guid? folderId, Guid TmpID, CancellationToken cancellationToken);

        Task<KBArticleFolderDetails> AddFolderAsync(KBArticleFolderDetails folderModel, bool seeInvisible, CancellationToken cancellationToken);

        Task<bool> CheckSearchAccessAsync(bool seeInvisible, CancellationToken cancellationToken);

        Task<KBArticleDetails> EditArticleAsync(Guid articleId, KBArticleEditData article, CancellationToken cancellationToken);

        Task EditFolderAsync(Guid folderId, KBArticleFolderDetails folderModel, bool seeInvisible, CancellationToken cancellationToken);


        /// <summary>
        /// Получаем список статей по указанным Ids
        /// </summary>
        /// <param name="ids">Идентификаторы статей</param>
        /// <param name="cancellationToken">Сигнализатор отмены операции</param>
        /// <returns></returns>
        Task<KBArticleShortDetails[]> GetArticlesByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

        /// <summary>
        /// Получаем список привязанных статей к объекту
        /// </summary>
        /// <param name="objectID">Идентификатор обьекта</param>
        /// <param name="objectClassID">Идентификатор класса объекта</param>
        /// <param name="seeInvisible">Флаг фильтра видимости</param>
        /// <param name="cancellationToken">Сигнализатор отмены операции</param>
        /// <returns></returns>
        Task<KBArticleShortDetails[]> GetObjectArticlesAsync(Guid objectID, ObjectClass objectClassID, bool seeInvisible, CancellationToken cancellationToken);

        /// <summary>
        /// Вытаскиваем информацию по статьям KB с учетом заданного сервиса
        /// </summary>
        /// <param name="foundArticleIds">Идентификаторы интересуемых статей</param>
        /// <param name="serviceItemAttendanceID">Идентификатор сервиса для отфильтровки</param>
        /// <param name="cancellationToken">Сигнализатор отмены операции</param>
        /// <returns></returns>
        Task<KBArticleInfoDetails[]> GetArticleInfoByIdsAsync(Guid[] foundArticleIds, Guid? serviceItemAttendanceID, CancellationToken cancellationToken);

        /// <summary>
        /// Добавление/Удаление референса на статью 
        /// </summary>
        /// <param name="kBArticleID">Идентификатор статьи</param>
        /// <param name="objectID">Идентификатор обьекта</param>
        /// <param name="objectClassID">Тип обьекта</param>
        /// <param name="creating">Флаг создания обьекта</param>
        /// <param name="cancellationToken">Сигнализатор отмены операции</param>
        /// <returns></returns>
        [Obsolete("Use AddReferenceAsync / RemoveReferenceAsync instead")]
        Task EditReferenceAsync(Guid kBArticleID, Guid objectID, ObjectClass objectClass, bool creating, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет ссылку на статью и сохраняет изменения
        /// </summary>
        /// <param name="articleID">Идентификатор статьи</param>
        /// <param name="reference">Ссылка</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default);
        /// <summary>
        /// Добавляет ссылку на статью без сохранения
        /// </summary>
        /// <param name="articleID">Идентификатор статьи</param>
        /// <param name="reference">Ссылка</param>
        Task AttachReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет ссылку на статью и сохраняет изменения
        /// </summary>
        /// <param name="articleID">Идентификатор статьи</param>
        /// <param name="reference">Ссылка</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет ссылку на статью без сохранения
        /// </summary>
        /// <param name="articleID">Идентификатор статьи</param>
        /// <param name="reference">Ссылка</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DetachReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default);

        /// <summary>
        /// Каскадное удаление папок Базы знаний
        /// </summary>
        /// <param name="ID">идентификатор папки базы знаний</param>
        /// <param name="seeInvisible">параметр видимлсти папки</param>
        /// <returns></returns>
        Task DeleteFolderAsync(Guid ID, bool seeInvisible, CancellationToken cancellationToken = default);
    }
}
