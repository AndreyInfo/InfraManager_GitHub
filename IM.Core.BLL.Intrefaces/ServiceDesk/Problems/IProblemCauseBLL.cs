using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems;

/// <summary>
/// Бизнес логика для Причин проблем
/// </summary>
public interface IProblemCauseBLL
{
    /// <summary>
    /// Получение списка данных, с возможность поиска
    /// если строка поиска null выдает все подряд
    /// иначе идет поиск по name
    /// </summary>
    /// <param name="search">строка поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>массив моделек для фронта причин проблем</returns>
    Task<ProblemCauseDetails[]> GetListAsync(string search, CancellationToken cancellationToken);
}
