
using InfraManager.DAL.Location;

namespace IM.Core.Import.BLL.Interface
{    
    /// <summary>
     /// Интерфейс для сущности Рабочего места пользователя
     /// </summary>
    public interface IWorkplaceBLL
    {
        /// <summary>
        /// Метод производит создание рабочего места в БД
        /// </summary>
        /// <param name="workplaceModel">модель рабочего места для сохранения</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> рабочеее место </returns>
        Task<Workplace> CreateAsync(WorkplaceModel workplaceModel, CancellationToken cancellationToken);
        /// <summary>
        /// Метод производит создание или получение рабочего места в БД
        /// </summary>
        /// <param name="workplaceModel">модель рабочего места для сохранения</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> рабочеее место </returns>
        Task<Workplace?> GetOrCreateByNameAsync(WorkplaceModel workplaceModel, CancellationToken cancellationToken);
        /// <summary>
        /// Метод производит поиск рабочего места в БД по имени
        /// </summary>
        /// <param name="name">имя рабочего места</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> рабочеее место </returns>
        Task<Workplace> GetWorkplaceByName(string name, CancellationToken cancellationToken);
        /// <summary>
        /// Метод производит поиск рабочего места в БД по внешнему идентификатору
        /// </summary>
        /// <param name="id">внешний идентификатор рабочего места</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> рабочеее место </returns>
        Task<Workplace> GetWorkplaceByExternalIDAsync(string id, CancellationToken cancellationToken);
        /// <summary>
        /// Метод производит поиск рабочего места в БД по внешнему идентификатору или имени
        /// </summary>
        /// <param name="id">внешний идентификатор или имя рабочего места</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> рабочеее место </returns>
        Task<Workplace> GetWorkplaceByExternalIDOrNameAsync(string nameOrId, CancellationToken cancellationToken);
        /// <summary>
        /// Метод производит поиск рабочего места в БД по imObjID
        /// </summary>
        /// <param name="imObjID">imObjID</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> рабочеее место </returns>
        Task<Workplace> GetAsync(Guid? imObjID, CancellationToken cancellationToken);

        /// <summary>
        /// Производит поиск рабочего места по критерию, и, если не находит рабочее место, то его создает 
        /// </summary>
        /// <param name="workplaceModel">Критерий поиска</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        Task<Workplace> GetOrCreateByModelAsync(WorkplaceModel workplaceModel, CancellationToken token);
        
        /// <summary>
        /// Производит поиск рабочего места по критерию, возвращает null, если не найдено 
        /// </summary>
        /// <param name="model">Критерий поиска</param>
        /// <param name="token">Токен отмены</param>
        /// <returns></returns>
        Task<Workplace?> GetWorkplaceByModelAsync(WorkplaceModel model, CancellationToken token);
    }
}
