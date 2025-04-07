using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

public interface ISupportBLL<T> where T : class
{
    /// <summary>
    /// Получение поддержки линии
    /// </summary>
    /// <param name="expression">выражение поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SupportLineResponsibleDetails> GetSupportByIdAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
}
