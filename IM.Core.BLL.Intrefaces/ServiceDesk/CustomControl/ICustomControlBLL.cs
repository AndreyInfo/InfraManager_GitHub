using Inframanager.BLL.ListView;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    public interface ICustomControlBLL
    {
        /// <summary>
        /// Получает информации о состоянии контроля объекта текущим пользователем
        /// </summary>
        /// <param name="inframanagerObject">Идентификатор объекта</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные о состоянии контроля</returns>
        Task<CustomControlDetails> GetCustomControlDetailsAsync(
            InframanagerObject inframanagerObject,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Устанавливает состояние контроля над объектом текущим пользователем
        /// </summary>
        /// <param name="inframanagerObject">Идентификатор объекта</param>
        /// <param name="data">Состояние контроля</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные о состоянии контроля</returns>
        Task<CustomControlDetails> SetCustomControlDetailsAsync(
            InframanagerObject inframanagerObject,
            CustomControlData data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает данные для списка "На контроле"
        /// </summary>
        /// <param name="filterBy">Фильтр данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Массив строк списка</returns>
        Task<ObjectUnderControl[]> GetListAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default);
    }
}
