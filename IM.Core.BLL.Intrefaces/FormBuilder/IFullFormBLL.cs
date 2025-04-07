using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;

namespace InfraManager.BLL.FormBuilder
{
    public interface  IFullFormBLL
    {
        /// <summary>
        /// Получить шаблон формы по уникальному идентификатору асинхронно.
        /// </summary>
        /// <param name="formID">Уникальный идентификатор шаблона формы.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Экземпляр <see cref="FormBuilderFullFormDetails"/>.</returns>
        public Task<FormBuilderFullFormDetails> GetAsync(Guid formID, CancellationToken cancellationToken = default);

        /// <summary>
        /// Клонирует схему и создает новую с такими же параметрами
        /// </summary>
        /// <param name="description">Описание новой формы</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <param name="formID">ID формы, которую нужно клонировать</param>
        /// <param name="name">Название для новой формы</param>
        /// <param name="identifier">Новый Идентификатор формы</param>
        /// <returns>ID новой формы</returns>
        public Task<Guid> CloneAsync(Guid formID, string name, string identifier, string description,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Возвращает json строку для экспорта схемы
        /// </summary>
        /// <param name="id">ID экспортируемой схемы</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Сериализованную строку схемы</returns>
        Task<string> ExportAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Преобразует json строку с formBuilder в объект и сохраняет его
        /// </summary>
        /// <param name="formBuilderJson">Сериализованная строка с formBuilder</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task ImportAsync(string formBuilderJson, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет доп. поля формы
        /// </summary>
        /// <param name="data">Данные для формы</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <param name="id">ID формы</param>
        Task<Guid> SaveAsync(Guid id, FormBuilderFullFormData data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить шаблон формы доп. параметров по заданному классу объекта и его глобальному идентификатор асинхронно.
        /// </summary>
        /// <param name="classID">Класс объекта.</param>
        /// <param name="objectID">Уникальный идентификатор объекта.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Экземпляр <see cref="FormBuilderFullFormDetails"/>.</returns>
        Task<FormBuilderFullFormDetails> GetAsync(ObjectClass classID, Guid objectID, CancellationToken cancellationToken = default);
    }
}
