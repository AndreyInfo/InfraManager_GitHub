using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure
{
    public interface ISubdivisionBLL
    {
        /// <summary>
        /// Возвращает список подразделений, удовлетворяющих критерию
        /// </summary>
        /// <param name="filterBy">Критерий отбора подразделений</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных подразделений</returns>
        Task<SubdivisionDetails[]> GetDetailsArrayAsync(SubdivisionListFilter filterBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает страницу списка подразделений
        /// </summary>
        /// <param name="filterBy">Критерий отбора подразделений</param>
        /// <param name="pageFilter">Параметры страницы</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных подразделений</returns>
        Task<SubdivisionDetails[]> GetDetailsPageAsync(SubdivisionListFilter filterBy, ClientPageFilter<Subdivision> pageFilter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение подразделения по ключу
        /// </summary>
        /// <param name="id">ID подразделения</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные подразделения</returns>
        Task<SubdivisionDetails> GetDetailsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление подраздеения по IMObjId
        /// </summary>
        /// <param name="subdivisionID"></param>
        /// <param name="cancellationToken"></param>
        Task DeleteByIDAsync(Guid subdivisionID, CancellationToken cancellationToken);

        /// <summary>
        /// Получаем цепочку родителей от подразделения
        /// </summary>
        /// <param name="subdivisionID">ID подразделения</param>
        Task<Subdivision[]> GetPathToSubdivisionAsync(Guid subdivisionID, CancellationToken cancellationToken);
        
        /// <summary>
        /// Обновление подразделения в базе
        /// </summary>
        /// <param name="subdivision">DTO подразделения</param>
        /// <param name="id">Идентификатор подразделения</param>
        /// <param name="cancellationToken"></param>
        Task UpdateAsync(Guid id, SubdivisionData subdivision, CancellationToken cancellationToken);

        /// <summary>
        /// Добавление подразделения в базу
        /// </summary>
        /// <param name="subdivision">DTO подразделения</param>
        Task<Guid> AddAsync(SubdivisionData subdivision, CancellationToken cancellationToken);

        /// <summary>
        /// Получение таблицы с поиском, пагинацией и сортировкой
        /// </summary>
        /// <param name="filter">фильтр для поиска и пагинации</param>
        /// <param name="cancellationToken"></param>
        /// <returns>модели подразделений</returns>
        Task<SubdivisionDetails[]> GetTableAsync(BaseFilter filter, CancellationToken cancellationToken);

        /// <summary>
        /// Получение всех дочерних узлов подразделения(любой уровень вложенности)
        /// </summary>
        /// <param name="parentID">идентификатор родительского подразделения</param>
        /// <param name="cancellationToken"></param>
        /// <returns>дочерние узлы</returns>
        Task<SubdivisionDetails[]> GetAllSubSubdivisionsAsync(Guid parentID, CancellationToken cancellationToken);
    }
}
