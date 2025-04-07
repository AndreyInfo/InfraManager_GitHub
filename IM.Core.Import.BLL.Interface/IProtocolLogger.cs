using InfraManager.DAL.Import;
using InfraManager.ServiceBase.ImportService.Log;

namespace IM.Core.Import.BLL.Interface;

/// <summary>
/// Интерфейс для сущности Протокола импорта
/// </summary>
public interface IProtocolLogger
{
    /// <summary>
    /// Метод запускает протоколирование задачи импорта
    /// </summary>
    /// <param name="taskName">имя задачи</param>
    /// <param name="taskType">тип задачи</param>
    /// <param name="taskId">идентификатор задачи</param>
    /// <param name="taskScheduleID">идентификатор задачи планировщика</param>
    void StartTask(string taskName, ImportTaskTypeEnum taskType, Guid taskId, Guid taskScheduleID);
    /// <summary>
    /// Метод дополняет протокол данными входных параметров для задачи импорта
    /// </summary>
    /// <param name="type">тип для импортирования данных</param>
    /// <param name="taskNote">дополнительная информация о задаче</param>
    /// <param name="path">путь до файла, если выбран файл</param>
    void AddInputData(ImportInputType type, string taskNote = "", string path = "");
    /// <summary>
    /// Метод дополняет протокол данными для отладки
    /// </summary>
    /// <param name="information">сообщение для отладки</param>
    void Debug(string information);
    /// <summary>
    /// Метод дополняет протокол информационными данными
    /// </summary>
    /// <param name="information">сообщение</param>
    void Information(string information);
    /// <summary>
    /// Метод дополняет протокол расширенными данными 
    /// </summary>
    /// <param name="information">сообщение</param>
    void Verbose(string information);
    /// <summary>
    /// Метод дополняет протокол данными ошибки
    /// </summary>
    /// <param name="exception">Ошибка</param>
    /// <param name="message">сообщение</param>
    void Error(Exception exception, string message);
    /// <summary>
    /// Метод закрывает файл и освобождает память
    /// </summary>
    void FlushAndClose();
    /// <summary>
    /// Метод проверки что все данные для протокола успешно добавлены
    /// </summary>
    void CheckCreateValidProtocol();
}
