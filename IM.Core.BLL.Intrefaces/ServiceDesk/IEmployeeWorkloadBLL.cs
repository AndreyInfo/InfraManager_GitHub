using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.BLL.Users;

namespace InfraManager.BLL.ServiceDesk;

public interface IEmployeeWorkloadBLL
{
    /// <summary>
    /// Получить список сотрудников с данными рабочей нагрузки асинхронно. 
    /// </summary>
    /// <param name="data">Предоставляет данные для выполнения расчетов.</param>
    /// <param name="pageBy">Пагинация.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Список сотрудников с данными рабочей нагрузки.</returns>
    Task<EmployeeWorkloadListItem[]> GetEmployeeWorkloadReportAsync(WorkloadListData data, ClientPageFilter pageBy, CancellationToken cancellationToken = default);
}