using Inframanager.BLL.ListView;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public interface IProblemBLL
    {
        Task<ProblemListItem[]> AllProblemsArrayAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy, 
            CancellationToken cancellationToken = default);
        Task<ProblemDetails[]> GetDetailsArrayAsync(ProblemListFilter filter, CancellationToken cancellationToken = default);
        Task<ProblemDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ProblemDetails> AddAsync(ProblemData problem, CancellationToken cancellationToken = default);
        Task<ProblemDetails> UpdateAsync(Guid id, ProblemData problem, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавить ассоциацию проблемы и запроса на изменения асинхронно.
        /// </summary>
        /// <param name="problemID">Уникальный идентификатор проблемы.</param>
        /// <param name="changeRequestID">Уникальный идентификатор запроса на изменения.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Экземпляр <see cref="ProblemToChangeRequestReferenceDetails"/>, содержащий сведения о созданной связи.</returns>
        Task<ProblemToChangeRequestReferenceDetails> AddChangeRequestAsync(Guid problemID, Guid changeRequestID, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удалить ассоциацию проблемы и запроса на изменения асинхронно.
        /// </summary>
        /// <param name="problemID">Уникальный идентификатор проблемы.</param>
        /// <param name="changeRequestID">Уникальный идентификатор запроса на изменения.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        Task RemoveChangeRequestAsync(Guid problemID, Guid changeRequestID, CancellationToken cancellationToken);

        /// <summary>
        /// Список проблем, доступных для связывания с массовым инцидентом
        /// </summary>
        /// <param name="filterBy">Критерии выборки и сортировки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных списка</returns>
        Task<ProblemReferenceListItem[]> GetAvailableMassIncidentReferencesAsync(InframanagerObjectListViewFilter filterBy, CancellationToken cancellationToken = default);
    }
}
