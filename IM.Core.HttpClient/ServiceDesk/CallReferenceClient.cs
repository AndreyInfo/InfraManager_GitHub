using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.WebApi.Contracts.Models;

namespace IM.Core.HttpClient.ServiceDesk;

/// <summary>
/// Клиент для работы со связями заявки
/// </summary>
public class CallReferenceClient : ClientWithAuthorization
{
    public CallReferenceClient(string baseUrl)
        : base(baseUrl)
    {
    }

    /// <summary>
    /// Получить ссылки Заявки указанного класса асинхронно. 
    /// </summary>
    /// <param name="callID">Уникальный идентификатор Заявки.</param>
    /// <param name="classID">Класс объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список ссылок Заявки.</returns>
    public async Task<CallReferenceData[]> GetAsync(Guid callID, ObjectClass classID, CancellationToken cancellationToken = default)
    {
        return await GetAsync<CallReferenceData[], object>(Uri(callID, classID), new object(), null, cancellationToken);
    }

    /// <summary>
    /// Добавить ссылку Заявки асинхронно.
    /// </summary>
    /// <param name="callID">Уникальный идентификатор Заявки.</param>
    /// <param name="objectID">Уникальный идентификатор связываемого объекта.</param>
    /// <param name="classID">Класс объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="CallReferenceData"/>, содержащий детали созданной связи.</returns>
    public async Task<CallReferenceData> AddAsync(Guid callID, Guid objectID, ObjectClass classID, CancellationToken cancellationToken = default)
    {
        // todo: Убрать костыль, когда будет исправлен Route в CallReferencesController. 
        var uri = Uri(callID, classID).Replace("/refs", string.Empty);
        var request = new ObjectID
        {
            ID = objectID
        };
        return await PostAsync<CallReferenceData, ObjectID>(uri, request, null, cancellationToken);
    }

    private static string Uri(Guid callID, ObjectClass classID) => $"calls/{callID}/refs/{(classID == ObjectClass.Problem ? "problems" : "changeRequests")}";
}