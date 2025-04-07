using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ObjectIcons
{
    /// <summary>
    /// Этот интерфейс описывает сервис получения / сохранения иконок объектов системы
    /// </summary>
    public interface IObjectIconBLL
    {
        /// <summary>
        /// Получение иконки объекта
        /// </summary>
        /// <param name="objectID">Идентификатор объекта (глобальный)</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на объект с данными иконки</returns>
        Task<ObjectIconDetails> GetAsync(InframanagerObject objectID, CancellationToken cancellationToken = default);

        /// <summary>
        /// Установка иконки для определенного объекта
        /// </summary>
        /// <param name="objectID">Идентификатор объекта (глобальный)</param>
        /// <param name="data">Ссылка на объект с данными иконки</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Ссылка на объект с данными иконки после изменения</returns>
        Task<ObjectIconDetails> SetAsync(InframanagerObject objectID, ObjectIconData data, CancellationToken cancellationToken = default);
    }
}
