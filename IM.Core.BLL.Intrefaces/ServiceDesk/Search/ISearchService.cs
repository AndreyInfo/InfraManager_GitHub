using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.Services.SearchService;

namespace InfraManager.BLL.ServiceDesk.Search
{
    /// <summary>
    /// Сервис полнотекстового поиска
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Поиск объектов указанных типов, содержащих указанный текст и помеченных указанными тэгами
        /// </summary>
        /// <param name="searchText">Текст для поиска</param>
        /// <param name="mode">Тип поиска</param>
        /// <param name="classes">Типы объектов</param>
        /// <param name="tags">Тэги</param>
        /// <param name="shouldSearchFinished">Должен ли результат содержать объекты с  заверённым workflow</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Найденные объекты</returns>
        Task<IReadOnlyList<FoundObject>> SearchAsync(string searchText, SearchHelper.SearchMode mode,
            IReadOnlyList<ObjectClass> classes,
            IReadOnlyList<string> tags, bool shouldSearchFinished, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление информации из сервиса поиска
        /// </summary>
        /// <param name="id">Идентификатор объекта, по которому нужно удалить информацию</param>
        void Delete(Guid id);

        /// <summary>
        /// Обновление информации в сервисе поиска
        /// </summary>
        /// <param name="entity">Сущность Service Desk</param>
        /// /// <param name="notes">Заметки</param>
        /// <returns></returns>
        void Update<T, TNote>(T entity, TNote[] notes) where T : IGloballyIdentifiedEntity;

        /// <summary>
        /// Обновление информации в сервисе поиска
        /// </summary>
        /// <param name="entity">Сущность Service Desk</param>
        /// /// <param name="notes">Заметки</param>
        /// <returns></returns>
        void Update<T>(T entity) where T : IGloballyIdentifiedEntity;

        /// <summary>
        /// Добавление информации в сервис поиска
        /// </summary>
        /// <param name="entity">Сущность Service Desk</param>
        void Insert<T>(T entity) where T : IGloballyIdentifiedEntity;

        /// <summary>
        /// Полностью перестраивает индекс полнотекстового поиска
        /// </summary>
        /// <returns></returns>
        void RebuildIndex();

        /// <summary>
        /// Оптимизирует индекс полнотекстового поиска
        /// </summary>
        /// <returns></returns>
        void OptimizeIndex();

        /// <summary>
        /// Получает текущий статус сервиса полнотекстового поиска
        /// </summary>
        /// <returns></returns>
        SearchServiceStatus GetStatus();
    }
}