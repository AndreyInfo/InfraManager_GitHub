using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.FormBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.BLL.FormBuilder.Forms;

namespace InfraManager.BLL.FormBuilder
{
    public interface IFormBLL
    {
        /// <summary>
        /// Получение формы по идентификатору
        /// </summary>
        /// <param name="formID">ID формы</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Форму</returns>
        Task<FormBuilderFormDetails> GetAsync(Guid formID, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получение списка форм по фильтру
        /// </summary>
        /// <param name="filter">Фильтр выборки</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Список форм удовлетворяющих выборке</returns>
        Task<FormBuilderFormDetails[]> ListAsync(FormBuilderFilter filter,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавление формы
        /// </summary>
        /// <param name="data">Данные для добавления формы</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Созданную форму</returns>
        Task<FormBuilderFormDetails> AddAsync(FormBuilderFormData data,
            CancellationToken cancellationToken);

        /// <summary>
        /// Обновление формы
        /// </summary>
        /// <param name="id">ID форомы для обновления</param>
        /// <param name="data">Данные для обновления формы</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Обновленную форму</returns>
        Task<FormBuilderFormDetails> UpdateAsync(Guid id, FormBuilderFormData data,
            CancellationToken cancellationToken);
        
        /// <summary>
        /// Удаление формы (удаляет также все дочерние объекты формы)
        /// </summary>
        /// <param name="id">ID формы для удаления</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Публикует форму
        /// </summary>
        /// <param name="formID">ID формы, которую нужно опубликовать</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task PublishAsync(Guid formID, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получение типов, которые могут быть использованны в Форм билдере
        /// </summary>
        /// <returns>Список возможных типов</returns>
        FormBuilderClassType[] GetAvailableTypes();

        /// <summary>
        /// Проверяет имя на уникальность
        /// </summary>
        /// <param name="name">Имя формы</param>
        /// <param name="mainID">MainID</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task ValidateNameAsync(string name, Guid? mainID = null,
            CancellationToken cancellationToken = default);
    }
}
