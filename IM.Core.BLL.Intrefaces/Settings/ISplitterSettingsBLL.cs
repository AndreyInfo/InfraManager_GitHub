using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    /// <summary>
    /// Этот интерфейс описывает бизнес логику работы с настройками ширины областей, отделяемых разделителем в интерфейсе
    /// </summary>
    public interface ISplitterSettingsBLL
    {
        /// <summary>
        /// Возвращает значение ширины определенной области интерфейса, отделенной разделитем, для определенного пользователя, которое было ранее сохранено,
        /// либо значение по умолчанию
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Наименование области разделителя</param>
        /// <returns>Ширина области</returns>
        Task<int> GetAsync(Guid userId, string name);

        /// <summary>
        /// Сохраняет значение ширины определенной области интерфейса, отделенной разделителем, для определенного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Наименование области разделителя</param>
        /// <param name="distance">Ширина области</param>
        /// <returns></returns>
        Task SetAsync(Guid userId, string name, int distance);
    }
}
