using Inframanager.BLL.ListView;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот интерфейс описывает сервис сущности "Массовый инцидент"
    /// </summary>
    public interface IMassIncidentBLL
    {
        #region Стандартный crud

        /// <summary>
        /// Получает список массовых инцидентов, удовлетворяющих условию выборки
        /// </summary>
        /// <param name="filterBy">Ссылка на объект с условиями выборки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных массовых инцидентов</returns>
        Task<MassIncidentDetails[]> GetDetailsArrayAsync(MassIncidentListFilter filterBy, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ищет массовый инцидент по ключу
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные массового инцидента</returns>
        Task<MassIncidentDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Создает новый массовый инцидент
        /// </summary>
        /// <param name="data">Данные нового массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные созданного массового инцидента</returns>
        Task<MassIncidentDetails> AddAsync(NewMassIncidentData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Изменяет состояние массового инцидента
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="data">Контракт с новыми данными массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные созданного массового инцидента</returns>
        Task<MassIncidentDetails> UpdateAsync(int id, MassIncidentData data, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет массовый инцидент
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        #endregion

        #region Заявки

        /// <summary>
        /// Возвращает ссылки на заявки, связанные с массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на массив идентификаторов связанных заявок</returns>
        Task<MassIncidentReferenceDetails[]> GetCallsAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ассоциирует заявку к массовый инцидент
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="callID">Идентификатор заяки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные ссылки массового инцидента на заявку</returns>
        Task<MassIncidentReferenceDetails> AddCallAsync(int id, Guid callID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет ассоциацию массового инцидента и заявки
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="callID">Идентификатор заявки</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveCallAsync(int id, Guid callID, CancellationToken cancellationToken = default);

        #endregion

        #region Проблемы

        /// <summary>
        /// Возвращает список ссылок на проблемы, связанные с массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив ссылок на проблемы</returns>
        Task<MassIncidentReferenceDetails[]> GetProblemsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ассоциирует проблему к массовый инцидент
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="problemID">Идентификатор проблемы</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные ссылки массового инцидента на заявку</returns>
        Task<MassIncidentReferenceDetails> AddProblemAsync(int id, Guid problemID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет ассоциацию массового инцидента и проблемы
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="problemID">Идентификатор проблемы</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveProblemAsync(int id, Guid problemID, CancellationToken cancellationToken = default);

        #endregion

        #region Запросы на изменения

        Task<MassIncidentReferenceDetails[]> GetChangeRequestAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ассоциирует запрос на изменения к массовый инцидент
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="changeRequestID">Идентификатор запроса на изменение</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные ссылки массового инцидента на запрос на изменение</returns>
        Task<MassIncidentReferenceDetails> AddChangeRequestAsync(int id, Guid changeRequestID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет ассоциацию массового инцидента и запроса на изменения
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="changeRequestID">Идентификатор запроса на изменение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveChangeRequestAsync(int id, Guid changeRequestID, CancellationToken cancellationToken = default);

        #endregion

        #region Затронутые сервисы

        /// <summary>
        /// Возвращает ссылки на все сервисы, затронутые массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных ссылок на сервисы</returns>
        Task<MassIncidentReferenceDetails[]> GetAffectedServicesAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет затронутый сервис к массовому инциденту
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="serviceID">Идентификатор сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MassIncidentReferenceDetails> AddAffectedServiceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Удаляет сервис из перечня затронутых сервисов массового инцидента
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="serviceID">Идентификатор сервиса</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveAffectedServiceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default);

        #endregion

        #region Отчеты

        /// <summary>
        /// Отчет "Все массовые инциденты"
        /// </summary>
        /// <param name="filterBy">Условия выборки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных отчета "Все массовые инциденты"</returns>
        Task<AllMassIncidentsReportItem[]> AllMassIncidentsAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Список связанных заявок с массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="pageFilter">Базовый фильтр списка</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных для списка</returns>
        Task<MassIncidentReferencedCallListItem[]> GetReferencedCallsReportAsync(
            int id,
            MassIncidentReferencesListViewFilter pageFilter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Список связанных РФЦ с массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="pageFilter">Базовый фильтр списка</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных для списка</returns>
        Task<MassIncidentReferencedChangeRequestListItem[]> GetReferencedChangeRequestsReportAsync(
            int id,
            MassIncidentReferencesListViewFilter pageFilter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Список связанных проблем с массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="pageFilter">Базовый фильтр списка</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных для списка</returns>
        Task<MassIncidentReferencedProblemListItem[]> GetReferencedProblemsReportAsync(
            int id,
            MassIncidentReferencesListViewFilter pageFilter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Список связанных заданий с массовым инцидентом
        /// </summary>
        /// <param name="id">Идентификатор массового инцидента</param>
        /// <param name="pageFilter">Базовый фильтр списка</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив данных для списка</returns>
        Task<MassIncidentReferencedWorkOrderListItem[]> GetReferencedWorkOrdersReportAsync(
            int id,
            MassIncidentReferencesListViewFilter pageFilter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Отчет "Массовые инциденты для ассоциации".
        /// </summary>
        /// <param name="filterBy">Условия выборки.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Массив данных отчета "Массовые инциденты для ассоциации".</returns>
        Task<MassIncidentsToAssociateReportItem[]> GetMassIncidentsToAssociateAsync(
            ListViewFilterData<MassIncidentsToAssociateFilter> filterBy,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Отчет "Массовые инциденты ассоциированные с проблемой".
        /// </summary>
        /// <param name="filterBy">Условия выборки.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Массив данных отчета "Массовые инциденты ассоциированные с проблемой".</returns>
        Task<ProblemMassIncidentsReportItem[]> GetProblemMassIncidentsAsync(
            ListViewFilterData<ProblemMassIncidentFilter> filterBy,
            CancellationToken cancellationToken = default);

        #endregion
    }
}
