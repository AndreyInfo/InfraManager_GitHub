using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;

public interface IKnowledgeBaseClassifier
{
        /// <summary>
        ///     Получает список классификаторов статей базы знаний
        /// </summary>
        /// <param name="filterBy">Ссылка на объект с условиями выборки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<KnowledgeBaseClassifierDetails[]> GetDetailsArrayAsync(KnowledgeBaseClassifierFilter filterBy,
                CancellationToken cancellationToken = default);

        /// <summary>
        ///     Возвращает классификатор статьи базы знаний
        /// </summary>
        /// <param name="id">Идентификатор классификатора статьи базы знаний</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<KnowledgeBaseClassifierDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Создает новый классификатор статьи базы знаний
        /// </summary>
        /// <param name="data">Данные нового классификатора статьи базы знаний</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<KnowledgeBaseClassifierDetails> AddAsync(KnowledgeBaseClassifierData data,
                CancellationToken cancellationToken = default);

        /// <summary>
        ///     Изменяет классификатор статьи базы знаний
        /// </summary>
        /// <param name="id">Идентификатор классификатора статьи базы знаний</param>
        /// <param name="data">Новые данные классификатора статьи базы знаний</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<KnowledgeBaseClassifierDetails> UpdateAsync(Guid id, KnowledgeBaseClassifierData data,
                CancellationToken cancellationToken = default);

        /// <summary>
        ///     Удаляет классификатор статьи базы знаний
        /// </summary>
        /// <param name="id">Идентификатор классификатора статьи базы знаний</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}