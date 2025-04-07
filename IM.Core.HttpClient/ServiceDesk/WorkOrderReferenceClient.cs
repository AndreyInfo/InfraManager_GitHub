using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;

namespace IM.Core.HttpClient.ServiceDesk;

/// <summary>
/// Клиента для работы с Заданиями.
/// </summary>
public class WorkOrderReferenceClient : ClientWithAuthorization
{
    private const string Url = "WorkOrders";

    public WorkOrderReferenceClient(string baseUrl)
        : base(baseUrl)
    {
    }

    /// <summary>
    /// Получить связи уникального Задания асинхронно.
    /// </summary>
    /// <param name="workOrderID">Уникальный идентификатор Задания.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список <see cref="InframanagerObject"/>, содержащий данные связанных с Заданием объектов.</returns>
    public Task<InframanagerObject[]> GetAsync(Guid workOrderID, CancellationToken cancellationToken = default)
    {
        return GetAsync<InframanagerObject[]>($"{Url}/{workOrderID}/references", null, cancellationToken);
    }

    /// <summary>
    /// Добавить связь с Заданием асинхронно.
    /// </summary>
    /// <param name="workOrderID">Уникальный идентификатор задания.</param>
    /// <param name="classID">Класс объекта.</param>
    /// <param name="objectID">Уникальный идентификатор привязываемого объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="WorkOrderDetailsModel"/>, содержащий детали созданной связи.</returns>
    public Task<WorkOrderDetailsModel> AddAsync(Guid workOrderID, ObjectClass classID, Guid objectID, CancellationToken cancellationToken = default)
    {
        var request = new WorkOrderDataModel { ReferencedObject = new InframanagerObject(objectID, classID), };
        return PutAsync<WorkOrderDetailsModel, WorkOrderDataModel>($"{Url}/{workOrderID}", request, null, cancellationToken);
    }
}