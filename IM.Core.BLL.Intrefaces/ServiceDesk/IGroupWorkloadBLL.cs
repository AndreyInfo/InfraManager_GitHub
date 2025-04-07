using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.OrganizationStructure.Groups;

namespace InfraManager.BLL.ServiceDesk;

public interface IGroupWorkloadBLL
{
    /// <summary>
    /// Получить список групп с данными рабочей нагрузки асинхронно.
    /// </summary>
    /// <param name="data">Предоставляет данные для выполнения расчетов.</param>
    /// <param name="pageBy">Пагинация.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список групп с данными рабочей нагрузки.</returns>
    Task<GroupWorkloadListItem[]> GetGroupWorkloadReportAsync(WorkloadListData data, ClientPageFilter pageBy, CancellationToken cancellationToken = default);
}