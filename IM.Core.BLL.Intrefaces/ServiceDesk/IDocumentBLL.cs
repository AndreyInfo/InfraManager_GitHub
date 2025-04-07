using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{

    /// <summary>
    /// CRUD операции по присоединяемым документам
    /// </summary>
    public interface IDocumentBLL
    {
        /// <summary>
        /// Проверка переключения сохранения на локальное хранилище
        /// </summary>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns>результат было ли переключение</returns>
        Task<bool> IsLocalSaveFilesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Удаление присоединенного документа
        /// </summary>
        /// <param name="objectID">ID родительского объекта</param>
        /// <param name="documentId">ID удаляемого документа</param>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns></returns>
        Task DeleteObjectDocumentsAsync(Guid objectID, Guid? documentId, CancellationToken cancellationToken);

        /// <summary>
        /// Получение данных документа
        /// </summary>
        /// <param name="documentId">ID документа</param>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns></returns>
        Task<DocumentDataDetails> GetDocumentDataAsync(Guid documentId, CancellationToken cancellationToken);

        /// <summary>
        /// Получение данных документов
        /// </summary>
        /// <param name="documentIds">ID документов</param>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns></returns>
        Task<DocumentInfoDetails[]> GetDocumentsDataAsync(Guid[] documentIds, CancellationToken cancellationToken);

        /// <summary>
        /// Получение метаданных по присоединенному документу
        /// </summary>
        /// <param name="documentId">ID документа</param>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns></returns>
        Task<DocumentInfoDetails> GetObjectDocumentAsync(Guid documentId, CancellationToken cancellationToken);

        /// <summary>
        /// Получение списка присоединненых документов
        /// </summary>
        /// <param name="objectID">ID родительского объекта</param>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns></returns>
        Task<DocumentInfoDetails[]> GetObjectDocumentsAsync(Guid objectID, CancellationToken cancellationToken);
        /// <summary>
        /// Добавление нового документа к объекту
        /// </summary>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="data">Данные файла</param>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        /// <returns></returns>
        Task<Guid> InsertDocumentAsync(string fileName, byte[] data, CancellationToken cancellationToken);
        /// <summary>
        /// Запуск миграции файлов с БД на локальное хранилище
        /// </summary>
        /// <param name="cancellationToken">Токен оповещение о прекращении операции</param>
        Task MigrateFromDBToLocalSaveAsync(CancellationToken cancellationToken);


    }
}
